using GameNetcodeStuff;
using HarmonyLib;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.MonoBehaviours
{
    [HarmonyPatch]
    public class GrabbableLandmine : GrabbableObject, IHittable
    {
        private bool mineActivated = true;

        public bool hasExploded;

        public Animator mineAnimator = null!;

        public AudioSource mineAudio = null!;

        public AudioSource mineTickSource = null!;

        public AudioClip mineDetonate = null!;

        public AudioClip mineTrigger = null!;

        public AudioClip mineDeactivate = null!;

        public AudioClip minePress = null!;

        private bool sendingExplosionRPC;

        private float pressMineDebounceTimer;

        public bool mineGrabbed = false;

        public bool onBlowUpSchedule = false;

        public float countDown = 0.0f;

        public float dropSafetyTime = 0.0f;

        private static int seed = 0;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Landmine), nameof(Landmine.Start))]
        private static void onLandmineStart(Landmine __instance)
        {
            if (Events.GrabbableLandmines.Active)
            {
                // Parent holds the doorcode object on vanilla mines
                GameObject terminalObjectRoot = __instance.transform.parent != null ? __instance.transform.parent.gameObject : __instance.gameObject;
                DisableTerminalAccessibleObjects(terminalObjectRoot);

                if (!RoundManager.Instance.IsHost) return;

                __instance.StartCoroutine(DestroySelfAndReplace(__instance));
            }
            else
            {
                if (!RoundManager.Instance.IsHost) return;

                seed++;
                System.Random rng = new(seed);
                __instance.StartCoroutine(syncTerminalCodeWhenReady(__instance, rng));
            }

            static IEnumerator DestroySelfAndReplace(Landmine landmine)
            {
                MEvent _event = Events.GrabbableLandmines.Instance;

                float rarity = _event.Getf(MEvent.ScaleType.Rarity);

                seed++;
                System.Random rng = new(StartOfRound.Instance.randomMapSeed + seed);
                if (rng.NextDouble() <= rarity)
                {
                    GameObject grabbableLandmine = Instantiate(Assets.grabbableLandmine.spawnPrefab, landmine.transform.position, Quaternion.identity);
                    NetworkObject netObject = grabbableLandmine.GetComponent<NetworkObject>();
                    netObject.Spawn();
                    Net.Instance.GenerateAndSyncTerminalCodeServerRpc(netObject, rng.Next(RoundManager.Instance.possibleCodesForBigDoors.Length));

                    int scrapValue = UnityEngine.Random.Range(Assets.grabbableLandmine.minValue, Assets.grabbableLandmine.maxValue + 1);
                    int multipliedScrapValue = Mathf.RoundToInt(scrapValue * RoundManager.Instance.scrapValueMultiplier * Manager.scrapValueMultiplier);
                    Net.Instance.SyncScrapValueServerRpc(netObject, multipliedScrapValue);

                    yield return new WaitForSeconds(5.0f);

                    NetworkObject vanillaNetObject = landmine.transform.parent != null ? landmine.transform.parent.GetComponent<NetworkObject>() : landmine.NetworkObject;
                    if (vanillaNetObject != null && vanillaNetObject.IsSpawned)
                    {
                        vanillaNetObject.Despawn(destroy: true);
                    }
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Landmine), nameof(Landmine.SpawnExplosion))]
        private static void ChainGrabbableLandmines(Vector3 explosionPosition, float damageRange)
        {
            if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsServer)
            {
                return;
            }

            GrabbableLandmine[] grabbableLandmines = Object.FindObjectsByType<GrabbableLandmine>(FindObjectsSortMode.None);
            for (int i = 0; i < grabbableLandmines.Length; i++)
            {
                GrabbableLandmine mine = grabbableLandmines[i];
                float distance = Vector3.Distance(explosionPosition, mine.transform.position);
                if (mine.hasExploded || distance >= damageRange || distance >= 6f)
                {
                    continue;
                }

                if (Physics.Linecast(explosionPosition, mine.transform.position + Vector3.up * 0.3f, out RaycastHit hitInfo, 1073742080, QueryTriggerInteraction.Ignore) && (hitInfo.collider.gameObject.layer == 30 || distance > 4f))
                {
                    continue;
                }

                mine.StartCoroutine(mine.TriggerOtherMineDelayed());
            }
        }

        private static void DisableTerminalAccessibleObjects(GameObject rootObject)
        {
            if (rootObject == null)
            {
                return;
            }

            foreach (TerminalAccessibleObject terminalAccessibleObject in rootObject.GetComponentsInChildren<TerminalAccessibleObject>(true))
            {
                if (terminalAccessibleObject == null)
                {
                    continue;
                }

                // Stops the bugged vanilla update after round ends
                terminalAccessibleObject.enabled = false;
            }
        }

        private static IEnumerator syncTerminalCodeWhenReady(Landmine landmine, System.Random rng)
        {
            float waitTime = 0f;
            while (landmine != null && (landmine.NetworkObject == null || !landmine.NetworkObject.IsSpawned) && waitTime < 2f)
            {
                waitTime += Time.deltaTime;
                yield return null;
            }

            if (landmine == null || landmine.NetworkObject == null || !landmine.NetworkObject.IsSpawned)
            {
                yield break;
            }

            Net.Instance.GenerateAndSyncTerminalCodeServerRpc(landmine.NetworkObject, rng.Next(RoundManager.Instance.possibleCodesForBigDoors.Length));
        }

        public override void Start()
        {
            StartCoroutine(StartIdleAnimation());
            base.Start();
            TerminalAccessibleObject terminalAccessibleObject = GetComponentInChildren<TerminalAccessibleObject>();
            if (terminalAccessibleObject != null)
            {
                // Should be done on the prefab if this isn't
                terminalAccessibleObject.terminalCodeEvent = new InteractEvent();
                terminalAccessibleObject.terminalCodeEvent.AddListener(DetonateFromTerminal);
                terminalAccessibleObject.terminalCodeCooldownEvent = new InteractEvent();
            }
            DisableTerminalAccessibleObjects(gameObject);
            if (IsServer)
            {
                DisableTerminalAccessibleObjectsClientRpc();
            }

            ScanNodeProperties scanNode = GetComponentInChildren<ScanNodeProperties>();
            if (scanNode != null)
            {
                // Stop the scan node collider from falling through ground
                Rigidbody scanNodeRigidbody = scanNode.gameObject.GetComponent<Rigidbody>() ?? scanNode.gameObject.AddComponent<Rigidbody>();
                scanNodeRigidbody.isKinematic = false;
                scanNodeRigidbody.useGravity = false;
                scanNodeRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }

            dropSafetyTime = 2.0f;
            mineGrabbed = true;

            IEnumerator StartIdleAnimation()
            {
                RoundManager roundManager = RoundManager.Instance;
                if (roundManager == null)
                {
                    yield break;
                }

                if (roundManager.BreakerBoxRandom != null)
                {
                    yield return new WaitForSeconds((float)roundManager.BreakerBoxRandom.NextDouble() + 0.5f);
                }
                mineAnimator.SetTrigger("startIdle");
                mineAudio.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            }
        }

        private void DetonateFromTerminal(PlayerControllerB playerWhoTriggered)
        {
            TriggerMineOnLocalClientByExiting();
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void DisableTerminalAccessibleObjectsClientRpc()
        {
            DisableTerminalAccessibleObjects(gameObject);
        }

        public override void OnDestroy()
        {
            DisableTerminalAccessibleObjects(gameObject);
            base.OnDestroy();
        }

        public override void Update()
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
                playMineTickSourceServerRpc(false);
                ExplodeMineServerRpc();
            }
            if (pressMineDebounceTimer > 0f)
            {
                pressMineDebounceTimer -= Time.deltaTime;
            }
            base.Update();
        }

        private void OnGrab()
        {
            mineGrabbed = true;
            onBlowUpSchedule = true;
            dropSafetyTime = 6.0f;
            countDown = 6.0f;
            playMineTickSourceServerRpc(true);
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        private void OnGrabServerRpc() => OnGrabClientRpc();

        [Rpc(SendTo.ClientsAndHost)]
        private void OnGrabClientRpc() => OnGrab();

        private void OnDiscard()
        {
            mineGrabbed = true;
            onBlowUpSchedule = false;
            dropSafetyTime = 1.5f;
            countDown = 0.0f;
            playMineTickSourceServerRpc(false);
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        private void OnDiscardServerRpc() => OnDiscardClientRpc();

        [Rpc(SendTo.ClientsAndHost)]
        private void OnDiscardClientRpc() => OnDiscard();

        public override void GrabItem()
        {
            base.GrabItem();
            OnGrab();
            OnGrabServerRpc();
        }

        public override void DiscardItem()
        {
            base.DiscardItem();
            OnDiscard();
            OnDiscardServerRpc();
        }

        public override void GrabItemFromEnemy(EnemyAI enemy)
        {
            base.GrabItemFromEnemy(enemy);
            OnGrab();
            OnGrabServerRpc();
        }

        public override void DiscardItemFromEnemy()
        {
            base.DiscardItemFromEnemy();
            OnDiscard();
            OnDiscardServerRpc();
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        private void playMineTickSourceServerRpc(bool toggle)
        {
            playMineTickSourceClientRpc(toggle);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void playMineTickSourceClientRpc(bool toggle)
        {
            if (toggle)
            {
                mineTickSource.Play();
            }
            else
            {
                mineTickSource.Stop();
            }
        }

        public void ToggleMine(bool enabled)
        {

            if (mineActivated != enabled)
            {
                mineActivated = enabled;
                if (!enabled)
                {
                    mineAudio.PlayOneShot(mineDeactivate);
                    WalkieTalkie.TransmitOneShotAudio(mineAudio, mineDeactivate);
                }
                ToggleMineServerRpc(enabled);
            }
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        public void ToggleMineServerRpc(bool enable)
        {
            ToggleMineClientRpc(enable);
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void ToggleMineClientRpc(bool enable)
        {
            if (mineActivated != enable)
            {
                mineActivated = enable;
                if (!enable)
                {
                    mineAudio.PlayOneShot(mineDeactivate);
                    WalkieTalkie.TransmitOneShotAudio(mineAudio, mineDeactivate);
                }
            }
        }

        private void OnTriggerEnter(Collider other) => MineCollisionEnter(other);

        private void OnCollisionEnter(Collision collision) => MineCollisionEnter(collision.collider);

        private void MineCollisionEnter(Collider other)
        {
            if (hasExploded || pressMineDebounceTimer > 0f || mineGrabbed || Events.GrabbableLandmines.LandmineDisabled || dropSafetyTime > 0.0f)
            {
                return;
            }
            if (!other.CompareTag("PhysicsProp") && !other.tag.StartsWith("PlayerRagdoll"))
            {
                return;
            }
            if ((bool)other.GetComponent<DeadBodyInfo>())
            {
                if (other.GetComponent<DeadBodyInfo>().playerScript != GameNetworkManager.Instance.localPlayerController)
                {
                    return;
                }
            }
            else if ((bool)other.GetComponent<GrabbableObject>() && !other.GetComponent<GrabbableObject>().NetworkObject.IsOwner)
            {
                return;
            }
            pressMineDebounceTimer = 0.5f;
            PressMineServerRpc();
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        public void PressMineServerRpc()
        {
            PressMineClientRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void PressMineClientRpc()
        {
            pressMineDebounceTimer = 0.5f;
            mineAudio.PlayOneShot(minePress);
            WalkieTalkie.TransmitOneShotAudio(mineAudio, minePress);
        }

        private void OnTriggerExit(Collider other) => MineCollisionExit(other);

        private void OnCollisionExit(Collision collision) => MineCollisionExit(collision.collider);

        private void MineCollisionExit(Collider other)
        {
            if (hasExploded || !mineActivated || mineGrabbed || Events.GrabbableLandmines.LandmineDisabled || dropSafetyTime > 0.0f)
            {
                return;
            }
            if (!other.tag.StartsWith("PlayerRagdoll") && !other.CompareTag("PhysicsProp"))
            {
                return;
            }
            if ((bool)other.GetComponent<DeadBodyInfo>())
            {
                if (other.GetComponent<DeadBodyInfo>().playerScript != GameNetworkManager.Instance.localPlayerController)
                {
                    return;
                }
            }
            else if ((bool)other.GetComponent<GrabbableObject>() && !other.GetComponent<GrabbableObject>().NetworkObject.IsOwner)
            {
                return;
            }
            TriggerMineOnLocalClientByExiting();
        }

        private void TriggerMineOnLocalClientByExiting()
        {
            if (hasExploded || mineGrabbed || dropSafetyTime > 0.0f)
            {
                return;
            }

            SetOffMineAnimation();
            sendingExplosionRPC = true;
            ExplodeMineServerRpc();
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        public void ExplodeMineServerRpc()
        {
            ExplodeMineClientRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void ExplodeMineClientRpc()
        {
            if (sendingExplosionRPC)
            {
                sendingExplosionRPC = false;
            }
            else
            {
                SetOffMineAnimation();
            }
        }

        public void SetOffMineAnimation()
        {
            if (hasExploded || dropSafetyTime > 0.0f || mineGrabbed) return;
            hasExploded = true;
            mineAnimator.SetTrigger("detonate");
            mineAudio.PlayOneShot(mineTrigger, 1f);
        }

        public void Detonate()
        {
            if (dropSafetyTime > 0.0f || mineGrabbed) return;

            mineAudio.pitch = UnityEngine.Random.Range(0.93f, 1.07f);
            mineAudio.PlayOneShot(mineDetonate, 1f);

            Vector3 explosionPosition = transform.position + Vector3.up;
            Landmine.SpawnExplosion(explosionPosition, spawnExplosionEffect: false, 5.7f, 6f);

            if (NetworkManager.Singleton.IsServer)
            {
                StartCoroutine(DestroyObject());
            }
        }

        private IEnumerator TriggerOtherMineDelayed()
        {
            if (hasExploded)
            {
                yield break;
            }

            // Chain mines need to go through the grabbable RPC
            mineAudio.pitch = UnityEngine.Random.Range(0.75f, 1.07f);
            yield return new WaitForSeconds(0.2f);
            dropSafetyTime = -1.0f;
            mineGrabbed = false;
            onBlowUpSchedule = false;
            countDown = 0.0f;
            SetOffMineAnimation();
            sendingExplosionRPC = true;
            ExplodeMineServerRpc();
        }

        private IEnumerator DestroyObject()
        {
            yield return new WaitForSeconds(5.0f);
            if (NetworkObject != null && NetworkObject.IsSpawned)
            {
                NetworkObject.Despawn(destroy: true);
            }
        }

        bool IHittable.Hit(int force, Vector3 hitDirection, PlayerControllerB playerWhoHit, bool playHitSFX, int hitID)
        {
            if (mineGrabbed) return false;
            SetOffMineAnimation();
            sendingExplosionRPC = true;
            ExplodeMineServerRpc();
            return true;
        }
    }
}
