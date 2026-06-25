using System;
using System.Collections;
using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BrutalCompanyMinus.Minus.MonoBehaviours
{
    public class SlayerShotgun : GrabbableObject
    {
        public AudioSource gunShootAudio = null!;

        public AudioSource gunBulletsRicochetAudio = null!;

        public AudioClip[] gunShootSFX = null!;

        private float misfireTimer = 30f;

        private bool hasHitGroundWithSafetyOff = true;

        private bool localClientSendingShootGunRPC;

        private PlayerControllerB? previousPlayerHeldBy;

        public ParticleSystem gunShootParticle = null!;

        public Transform shotgunRayPoint = null!;

        private readonly RaycastHit[] enemyColliders = new RaycastHit[32];

        private EnemyAI? heldByEnemy;

        public override void Start()
        {
            base.Start();

            misfireTimer = 10f;
            hasHitGroundWithSafetyOff = true;
        }

        public override void Update()
        {
            base.Update();
            // Infinite ammo
            if (!IsOwner || heldByEnemy != null || isPocketed)
            {
                return;
            }
            if (hasHitGround && !hasHitGroundWithSafetyOff && !isHeld)
            {
                if (Random.Range(0, 100) < 5)
                {
                    ShootGunAndSync(heldByPlayer: false);
                }
                hasHitGroundWithSafetyOff = true;
            }
            else if (misfireTimer <= 0f && !StartOfRound.Instance.inShipPhase)
            {
                if (Random.Range(0, 100) < 4)
                {
                    ShootGunAndSync(isHeld);
                }
                if (Random.Range(0, 100) < 5)
                {
                    misfireTimer = 2f;
                }
                else
                {
                    misfireTimer = Random.Range(8f, 15f);
                }
            }
            else
            {
                misfireTimer -= Time.deltaTime;
            }
        }

        public override void EquipItem()
        {
            base.EquipItem();
            previousPlayerHeldBy = playerHeldBy;
            previousPlayerHeldBy?.equippedUsableItemQE = true;
            hasHitGroundWithSafetyOff = false;
        }

        public override void GrabItemFromEnemy(EnemyAI enemy)
        {
            base.GrabItemFromEnemy(enemy);
            heldByEnemy = enemy;
            hasHitGroundWithSafetyOff = false;
        }

        public override void DiscardItemFromEnemy()
        {
            base.DiscardItemFromEnemy();
            heldByEnemy = null;
        }

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (IsOwner)
            {
                ShootGunAndSync(heldByPlayer: true);
            }
        }

        public void ShootGunAndSync(bool heldByPlayer)
        {
            Vector3 shotgunPosition;
            Vector3 forward;
            if (!heldByPlayer)
            {
                shotgunPosition = shotgunRayPoint.position;
                forward = shotgunRayPoint.forward;
            }
            else
            {
                shotgunPosition = GameNetworkManager.Instance.localPlayerController.gameplayCamera.transform.position - GameNetworkManager.Instance.localPlayerController.gameplayCamera.transform.up * 0.45f;
                forward = GameNetworkManager.Instance.localPlayerController.gameplayCamera.transform.forward;
            }
            ShootGun(shotgunPosition, forward);
            localClientSendingShootGunRPC = true;
            ShootGunServerRpc(shotgunPosition, forward);
        }


        [Rpc(SendTo.Server, RequireOwnership = false)]
        public void ShootGunServerRpc(Vector3 shotgunPosition, Vector3 shotgunForward)
        {
            ShootGunClientRpc(shotgunPosition, shotgunForward);
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void ShootGunClientRpc(Vector3 shotgunPosition, Vector3 shotgunForward)
        {
            if (localClientSendingShootGunRPC)
            {
                localClientSendingShootGunRPC = false;
            }
            else
            {
                ShootGun(shotgunPosition, shotgunForward);
            }
        }

        public void ShootGun(Vector3 shotgunPosition, Vector3 shotgunForward)
        {
            bool extraLogging;
            try
            {
                extraLogging = Configuration.ExtraLogging!.Value;
            }
            catch (NullReferenceException)
            {
                Log.LogError("Extra Logging Feature Errored.");
                extraLogging = false;
            }

            if (extraLogging)
            {
                Log.LogInfo(string.Format("SlayerShotGun shot at {0}, towards {1}", shotgunPosition, shotgunForward));
            }

            bool heldByPlayer = false;
            if (isHeld && playerHeldBy != null && playerHeldBy == GameNetworkManager.Instance.localPlayerController)
            {
                playerHeldBy.playerBodyAnimator.SetTrigger("ShootShotgun");

                heldByPlayer = true;
            }

            RoundManager.PlayRandomClip(gunShootAudio, gunShootSFX, randomize: true, 1f, 1840);
            WalkieTalkie.TransmitOneShotAudio(gunShootAudio, gunShootSFX[0]);
            gunShootParticle.Play(withChildren: true);
            PlayerControllerB localPlayerController = GameNetworkManager.Instance.localPlayerController;
            if (localPlayerController == null)
            {
                return;
            }
            float distanceToLocalPlayer = Vector3.SqrMagnitude(localPlayerController.transform.position - shotgunRayPoint.transform.position);
            bool hitPlayer = false;
            int num2 = 0;
            float num3 = 0f;
            Vector3 vector = localPlayerController.playerCollider.ClosestPoint(shotgunPosition);
            if (!heldByPlayer && !Physics.Linecast(shotgunPosition, vector, StartOfRound.Instance.collidersAndRoomMaskAndDefault, QueryTriggerInteraction.Ignore) && Vector3.Angle(shotgunForward, vector - shotgunPosition) < 30f)
            {
                hitPlayer = true;
            }
            if (distanceToLocalPlayer < 25f)
            {
                num3 = 0.8f;
                HUDManager.Instance.ShakeCamera(ScreenShakeType.Big);
                num2 = 100;
            }
            if (distanceToLocalPlayer < 225f)
            {
                num3 = 0.5f;
                HUDManager.Instance.ShakeCamera(ScreenShakeType.Big);
                num2 = 100;
            }
            else if (distanceToLocalPlayer < 529f)
            {
                HUDManager.Instance.ShakeCamera(ScreenShakeType.Small);
                num2 = 40;
            }
            else if (distanceToLocalPlayer < 900f)
            {
                num2 = 20;
            }
            if (num3 > 0f && SoundManager.Instance.timeSinceEarsStartedRinging > 16f)
            {
                StartCoroutine(DelayedEarsRinging(num3));
            }


            Ray ray = new(shotgunPosition, shotgunForward);


            if (Physics.Raycast(ray, out var hitInfo, 30f, StartOfRound.Instance.collidersAndRoomMaskAndDefault, QueryTriggerInteraction.Ignore))
            {
                gunBulletsRicochetAudio.transform.position = ray.GetPoint(hitInfo.distance - 0.5f);
                gunBulletsRicochetAudio.Play();
            }
            if (hitPlayer)
            {
                localPlayerController.DamagePlayer(num2, hasDamageSFX: true, callRPC: true, CauseOfDeath.Gunshots, 0, fallDamage: false, shotgunRayPoint.forward * 30f);
            }

            if (!IsOwner)
            {
                return;
            }

            // No SphereCastAll allocations
            int hitCount = Physics.SphereCastNonAlloc(ray, 5f, enemyColliders, 15f, 524288, QueryTriggerInteraction.Collide);
            if (extraLogging)
            {
                Log.LogInfo($"Raycast hits: {hitCount}");
            }
            for (int i = 0; i < hitCount; i++)
            {
                if (extraLogging)
                {
                    Log.LogInfo("Raycasting enemy");
                }

                if (!enemyColliders[i].transform.TryGetComponent(out EnemyAICollisionDetect enemyCollision))
                {
                    continue;
                }

                EnemyAI mainScript = enemyCollision.mainScript;
                if (heldByEnemy != null && heldByEnemy == mainScript)
                {
                    // Dont let nutslayer nut on itself
                    if (extraLogging)
                    {
                        Log.LogInfo("Shotgun is held by enemy, skipping entry");
                    }
                    continue;
                }

                if (enemyColliders[i].transform.TryGetComponent(out IHittable component))
                {
                    float hitDistance = Vector3.SqrMagnitude(shotgunPosition - enemyColliders[i].point);
                    int num6 = hitDistance < 13.69f ? 5 : !(hitDistance < 36f) ? 2 : 3;
                    if (extraLogging)
                    {
                        Log.LogInfo($"Hit enemy, hitDamage: {num6}");
                    }
                    component.Hit(num6, shotgunForward, playerHeldBy, playHitSFX: true);
                }
                else if (extraLogging)
                {
                    Log.LogInfo("Could not get hittable script from collider, transform: " + enemyColliders[i].transform.name);
                    Log.LogInfo("collider: " + enemyColliders[i].collider.name);
                }
            }

            static IEnumerator DelayedEarsRinging(float effectSeverity)
            {
                yield return new WaitForSeconds(0.6f);
                SoundManager.Instance.earsRingingTimer = effectSeverity;
            }
        }

        public override void SetControlTipsForItem()
        {
            string[] toolTips = itemProperties.toolTips;
            if (toolTips.Length <= 2)
            {
                Log.LogError("Shotgun control tips array length is too short to set tips!");
                return;
            }
            toolTips[2] = "No safety";
            HUDManager.Instance.ChangeControlTipMultiple(toolTips, holdingItem: true, itemProperties);
        }

        public override void PocketItem()
        {
            base.PocketItem();
            StopUsingGun();
        }

        public override void DiscardItem()
        {
            base.DiscardItem();
            StopUsingGun();
        }

        private void StopUsingGun()
        {
            previousPlayerHeldBy?.equippedUsableItemQE = false;
        }
    }
}
