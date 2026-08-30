using GameNetcodeStuff;
using HarmonyLib;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.MonoBehaviours
{
    [HarmonyPatch]
    public class ExplodingItemsNetScript : NetworkBehaviour, IHittable
    {
        private GrabbableObject grabbableItem = null!;

        public bool HasExploded { get; private set; } = false;
        public bool mineActivated = true;
        public bool onBlowUpSchedule = false;
        public float countDown = 0.0f;
        public float dropSafetyTime = 0.0f;
        public bool mineGrabbed = false;
        public float pressMineDebounceTimer = 0f;
        public bool sendingExplosionRPC = false;

        public AudioSource itemAudio = null!;
        private AudioClip mineTickSound = null!;
        private AudioClip mineDetonateSound = null!;
        private AudioClip mineTriggerSound = null!;
        private AudioClip minePressSound = null!;

        private void Awake()
        {
            grabbableItem = GetComponentInParent<GrabbableObject>();
            if (itemAudio == null)
            {
                itemAudio = GetComponentInParent<AudioSource>() ?? transform.parent.gameObject.AddComponent<AudioSource>();
            }
            mineTickSound = Assets.mineTickSound;
            mineDetonateSound = Assets.mineDetonateSound;
            mineTriggerSound = Assets.mineTriggerSound;
            minePressSound = Assets.minePressSound;
        }

        private void Start()
        {
            dropSafetyTime = 2.0f;
            mineGrabbed = true;
        }

        private void Update()
        {
            if (countDown > 0.0f)
            {
                countDown -= Time.deltaTime;
            }
            if (dropSafetyTime > 0.0f)
            {
                dropSafetyTime -= Time.deltaTime;
            }
            else
            {
                mineGrabbed = false;
            }

            if (countDown <= 0.0f && onBlowUpSchedule)
            {
                onBlowUpSchedule = false;
                dropSafetyTime = -1.0f;
                mineGrabbed = false;
                PlayTickAudioServerRpc(false);
                ExplodeItemServerRpc();
            }

            if (pressMineDebounceTimer > 0f)
            {
                pressMineDebounceTimer -= Time.deltaTime;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.GrabItem))]
        [HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.GrabItemFromEnemy))]
        public static void OnGrab_Postfix(GrabbableObject __instance)
        {
            if (!__instance.IsOwner)
            {
                return;
            }

            ExplodingItemsNetScript explodingItem = __instance.GetComponentInChildren<ExplodingItemsNetScript>();
            if (explodingItem != null)
            {
                explodingItem.OnGrab();
                explodingItem.SendOnGrabRpc();
            }
        }

        public void OnGrab()
        {
            mineGrabbed = true;
            onBlowUpSchedule = true;
            dropSafetyTime = 6.0f;
            countDown = 6.0f;
            PlayTickAudioServerRpc(true);
        }


        [Rpc(SendTo.NotMe, RequireOwnership = false)]
        internal void SendOnGrabRpc()
        {
            OnGrab();
        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.DiscardItem))]
        [HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.DiscardItemFromEnemy))]
        public static void OnDiscard_Postfix(GrabbableObject __instance)
        {
            if (!__instance.IsOwner)
            {
                return;
            }

            ExplodingItemsNetScript explodingItem = __instance.GetComponentInChildren<ExplodingItemsNetScript>();
            if (explodingItem != null)
            {
                explodingItem.OnDiscard();
                explodingItem.SendOnDiscardRpc();
            }
        }

        public void OnDiscard()
        {
            mineGrabbed = true;
            onBlowUpSchedule = false;
            dropSafetyTime = 1.5f;
            countDown = 0.0f;
            PlayTickAudioServerRpc(false);
        }

        [Rpc(SendTo.NotMe, RequireOwnership = false)]
        internal void SendOnDiscardRpc()
        {
            OnDiscard();
        }


        [Rpc(SendTo.Server, RequireOwnership = false)]
        private void PlayTickAudioServerRpc(bool play) => PlayTickAudioClientRpc(play);

        [Rpc(SendTo.ClientsAndHost)]
        private void PlayTickAudioClientRpc(bool play)
        {
            if (itemAudio == null) return;
            if (play)
            {
                if (mineTickSound != null && !itemAudio.isPlaying)
                {
                    itemAudio.clip = mineTickSound;
                    itemAudio.loop = true;
                    itemAudio.Play();
                }
            }
            else
            {
                itemAudio.Stop();
                itemAudio.loop = false;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (HasExploded || pressMineDebounceTimer > 0f || mineGrabbed || dropSafetyTime > 0.0f) return;

            Collider other = collision.collider;
            if (!other.CompareTag("PhysicsProp") && !other.tag.StartsWith("PlayerRagdoll")) return;

            pressMineDebounceTimer = 0.5f;
            PressItemServerRpc();
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        public void PressItemServerRpc() => PressItemClientRpc();

        [Rpc(SendTo.ClientsAndHost)]
        public void PressItemClientRpc()
        {
            pressMineDebounceTimer = 0.5f;
            if (itemAudio != null && minePressSound != null)
            {
                itemAudio.PlayOneShot(minePressSound);
                WalkieTalkie.TransmitOneShotAudio(itemAudio, minePressSound);
            }
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        public void ExplodeItemServerRpc() => ExplodeItemClientRpc();

        [Rpc(SendTo.ClientsAndHost)]
        public void ExplodeItemClientRpc()
        {
            if (HasExploded) return;
            HasExploded = true;
            StartCoroutine(DetonateSequence());
        }

        private IEnumerator DetonateSequence()
        {
            if (itemAudio != null)
            {
                itemAudio.Stop();
                if (mineTriggerSound != null) itemAudio.PlayOneShot(mineTriggerSound, 1f);
            }

            yield return new WaitForSeconds(0.2f);

            if (itemAudio != null && mineDetonateSound != null)
            {
                itemAudio.pitch = Random.Range(0.93f, 1.07f);
                itemAudio.PlayOneShot(mineDetonateSound, 1f);
            }

            Vector3 explosionPosition = transform.position + Vector3.up;
            Landmine.SpawnExplosion(explosionPosition, spawnExplosionEffect: true, 5.7f, 6f);

            if (NetworkManager.Singleton.IsServer)
            {
                StartCoroutine(DestroyObject());
            }
        }

        public IEnumerator TriggerOtherMineDelayed()
        {
            if (HasExploded) yield break;

            if (itemAudio != null)
                itemAudio.pitch = Random.Range(0.75f, 1.07f);

            yield return new WaitForSeconds(0.2f);
            dropSafetyTime = -1.0f;
            mineGrabbed = false;
            onBlowUpSchedule = false;
            countDown = 0.0f;

            sendingExplosionRPC = true;
            ExplodeItemServerRpc();
        }

        private IEnumerator DestroyObject()
        {
            yield return new WaitForSeconds(1.0f);
            if (NetworkObject != null && NetworkObject.IsSpawned)
            {
                NetworkObject.Despawn(destroy: true);
            }
        }

        public bool Hit(int force, Vector3 hitDirection, PlayerControllerB playerWhoHit, bool playHitSFX, int hitID)
        {
            if (mineGrabbed || HasExploded) return false;
            sendingExplosionRPC = true;
            ExplodeItemServerRpc();
            return true;
        }
    }
}