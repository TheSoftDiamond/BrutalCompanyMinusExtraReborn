using System.Collections;
using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.MonoBehaviours
{
    public class NutSlayerAI : EnemyAI
    {
        public int previousBehaviourState = -1;

        public int previousBehaviourStateAIInterval = -1;

        public float localPlayerTurnDistance;

        public GameObject gunPrefab = null!;

        public SlayerShotgun gun = null!;

        public Transform gunPoint = null!;

        public NetworkObjectReference gunObjectRef;

        public AISearchRoutine patrol = null!;

        public AISearchRoutine attackSearch = null!;

        public Transform torsoContainer = null!;

        public float currentTorsoRotation;

        public int targetTorsoDegrees;

        public float torsoTurnSpeed = 6f;

        public AudioSource torsoTurnAudio = null!;

        public AudioSource longRangeAudio = null!;

        public AudioClip[] torsoFinishTurningClips = null!;

        public AudioClip aimSFX = null!;

        public AudioClip kickSFX = null!;

        public bool torsoTurning;

        // -1 stops it defaulting to host
        public int lastPlayerSeenMoving = -1;

        public float timeSinceSeeingTarget;

        public float timeSinceFiringGun;

        public bool aimingGun;

        public Vector3 lastSeenPlayerPos;

        public RaycastHit rayHit;

        public Coroutine gunCoroutine = null!;

        public Vector3 positionLastCheck;

        public Vector3 strafePosition;

        public bool reachedStrafePosition;

        public bool lostPlayerInChase;

        public float timeSinceHittingPlayer;

        public float walkCheckInterval;

        public int setShotgunScrapValue;

        public int timesSeeingSamePlayer;

        public int previousPlayerSeenWhenAiming;

        public float speedWhileAiming;

        // Custom variables

        public float speedWhileMoving = 9.5f;
        public float widthSearch = 45f;
        public int rangeSearch = 30;
        public Transform target = null!;
        public bool isFiring = false;

        public int setHp = 5;
        public int Lives = 4;
        public bool Immortal = false;
        public bool onlyPlayers = false;

        public override void Start()
        {
            base.Start();
            lastPlayerSeenMoving = -1;
            if (IsServer)
            {
                InitializeNutcrackerValuesServerRpc();
            }
            rayHit = default;
            
            setHp = Configuration.nutSlayerHp!.Value;
            Lives = Configuration.nutSlayerLives!.Value;
            speedWhileMoving = Configuration.nutSlayerMovementSpeed!.Value;
            Immortal = Configuration.nutSlayerImmortal!.Value;
            onlyPlayers = Configuration.onlyPlayersAttackSlayer!.Value;
            enemyType.canDie = !Immortal;

            enemyHP = setHp;
        }

        [Rpc(SendTo.Server)]
        public void InitializeNutcrackerValuesServerRpc()
        {
            GameObject gameObject = Instantiate(gunPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity, RoundManager.Instance.spawnedScrapContainer);
            gameObject.GetComponent<NetworkObject>().Spawn();
            setShotgunScrapValue = UnityEngine.Random.Range(Configuration.slayerShotgunMinValue!.Value, Configuration.slayerShotgunMaxValue!.Value + 1);
            GrabGun(gameObject);
            InitializeNutcrackerValuesClientRpc(gameObject.GetComponent<NetworkObject>(), setShotgunScrapValue);
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void InitializeNutcrackerValuesClientRpc(NetworkObjectReference gunObject, int setShotgunValue)
        {
            setShotgunScrapValue = setShotgunValue;
            gunObjectRef = gunObject;
        }

        private void GrabGun(GameObject gunObject)
        {
            gun = gunObject.GetComponent<SlayerShotgun>();
            if (gun == null)
            {
                LogEnemyError("Gun in GrabGun function did not contain ShotgunItem component.");
                return;
            }
            gun.SetScrapValue(setShotgunScrapValue);
            RoundManager.Instance.totalScrapValueInLevel += gun.scrapValue;
            gun.parentObject = gunPoint;
            gun.isHeldByEnemy = true;
            gun.grabbableToEnemies = false;
            gun.grabbable = false;
            gun.GrabItemFromEnemy(this);
        }

        [Rpc(SendTo.Server)]
        public void DropGunServerRpc(Vector3 dropPosition)
        {
            DropGunClientRpc(dropPosition);
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void DropGunClientRpc(Vector3 dropPosition)
        {
            if (gun == null)
            {
                LogEnemyError("Could not drop gun since no gun was held!");
                return;
            }
            gun.DiscardItemFromEnemy();
            gun.isHeldByEnemy = false;
            gun.grabbableToEnemies = true;
            gun.grabbable = true;
        }

        public override void DoAIInterval()
        {
            base.DoAIInterval();
            if (isEnemyDead || stunNormalizedTimer > 0f || gun == null)
            {
                if (isEnemyDead)
                {
                    StopActiveSearches();
                }
                return;
            }
            switch (currentBehaviourStateIndex)
            {
                case 0:
                    if (previousBehaviourStateAIInterval != currentBehaviourStateIndex)
                    {
                        previousBehaviourStateAIInterval = currentBehaviourStateIndex;
                        agent.stoppingDistance = 0.02f;
                    }
                    if (!patrol.inProgress)
                    {
                        StartSearch(transform.position, patrol);
                    }
                    break;
                case 2:
                    if (previousBehaviourStateAIInterval != currentBehaviourStateIndex)
                    {
                        previousBehaviourStateAIInterval = currentBehaviourStateIndex;
                        if (patrol.inProgress)
                        {
                            StopSearch(patrol);
                        }
                    }
                    if (timeSinceSeeingTarget >= 3.0f)
                    {
                        SwitchToBehaviourState(0);

                    }
                    if (timeSinceFiringGun >= 10.0f && !attackSearch.inProgress)
                    {
                        SwitchToBehaviourState(0);
                        timeSinceFiringGun = 0.0f;
                    }
                    if (!IsOwner)
                    {
                        break;
                    }
                    if (timeSinceSeeingTarget < 0.5f)
                    {
                        if (attackSearch.inProgress)
                        {
                            StopSearch(attackSearch);
                        }
                        reachedStrafePosition = false;
                        SetDestinationToPosition(lastSeenPlayerPos);
                        agent.stoppingDistance = 1f;
                        if (lostPlayerInChase)
                        {
                            lostPlayerInChase = false;
                            SetLostPlayerInChaseServerRpc(lostPlayer: false);
                        }
                        break;
                    }
                    agent.stoppingDistance = 0.02f;
                    if (!reachedStrafePosition)
                    {
                        if (!agent.CalculatePath(lastSeenPlayerPos, path1))
                        {
                            break;
                        }
                        if (path1.corners.Length > 1)
                        {
                            Ray ray = new(path1.corners[path1.corners.Length - 1], path1.corners[path1.corners.Length - 1] - path1.corners[path1.corners.Length - 2]);
                            if (Physics.Raycast(ray, out rayHit, 5f, StartOfRound.Instance.collidersAndRoomMaskAndDefault, QueryTriggerInteraction.Ignore))
                            {
                                strafePosition = RoundManager.Instance.GetNavMeshPosition(ray.GetPoint(Mathf.Max(0f, rayHit.distance - 2f)));
                            }
                            else
                            {
                                strafePosition = RoundManager.Instance.GetNavMeshPosition(ray.GetPoint(6f));
                            }
                        }
                        else
                        {
                            strafePosition = lastSeenPlayerPos;
                        }
                        SetDestinationToPosition(strafePosition);
                        if (Vector3.SqrMagnitude(transform.position - strafePosition) < 4f)
                        {
                            reachedStrafePosition = true;
                        }
                    }
                    else
                    {
                        if (!lostPlayerInChase)
                        {
                            lostPlayerInChase = true;
                            SetLostPlayerInChaseServerRpc(lostPlayer: true);
                        }
                        if (!attackSearch.inProgress)
                        {
                            StartSearch(strafePosition, attackSearch);
                        }
                    }
                    break;
            }
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        public void SetLostPlayerInChaseServerRpc(bool lostPlayer)
        {
            SetLostPlayerInChaseClientRpc(lostPlayer);
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void SetLostPlayerInChaseClientRpc(bool lostPlayer)
        {
            lostPlayerInChase = lostPlayer;
            if (!lostPlayer)
            {
                timeSinceSeeingTarget = 0f;
            }
        }

        private void StopActiveSearches()
        {
            // Death cleanup
            if (patrol.inProgress)
            {
                StopSearch(patrol);
            }
            if (attackSearch.inProgress)
            {
                StopSearch(attackSearch);
            }
        }

        public void TurnTorsoToTargetDegrees()
        {
            currentTorsoRotation = Mathf.MoveTowardsAngle(currentTorsoRotation, targetTorsoDegrees, Time.deltaTime * torsoTurnSpeed);
            torsoContainer.localEulerAngles = new Vector3(currentTorsoRotation + 90f, 90f, 90f);
            if (Mathf.Abs(currentTorsoRotation - targetTorsoDegrees) > 5f)
            {
                if (!torsoTurning)
                {
                    torsoTurning = true;
                    torsoTurnAudio.Play();
                }
            }
            else if (torsoTurning)
            {
                torsoTurning = false;
                torsoTurnAudio.Stop();
                RoundManager.PlayRandomClip(torsoTurnAudio, torsoFinishTurningClips);
            }
            torsoTurnAudio.volume = Mathf.Lerp(torsoTurnAudio.volume, 1f, Time.deltaTime * 2f);
        }

        private void SetTargetDegreesToPosition(Vector3 pos)
        {
            pos.y = transform.position.y;
            Vector3 vector = pos - transform.position;
            targetTorsoDegrees = (int)Vector3.Angle(vector, transform.forward);
            if (Vector3.Cross(transform.forward, vector).y > 0f)
            {
                targetTorsoDegrees = 360 - targetTorsoDegrees;
            }
            torsoTurnSpeed = 1500f;
        }

        // Player

        [Rpc(SendTo.Server, RequireOwnership = false)]
        public void SeeMovingThreatServerRpc(Vector3 position, bool enterAttackFromPatrolMode = false, int playerId = -1)
        {
            SeeMovingThreatClientRpc(position, enterAttackFromPatrolMode, playerId);
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void SeeMovingThreatClientRpc(Vector3 position, bool enterAttackFromPatrolMode = false, int playerId = -1)
        {
            SwitchTargetTo(position, playerId);
            SwitchToBehaviourStateOnLocalClient(2);
        }

        public override void Update()
        {
            base.Update();
            TurnTorsoToTargetDegrees();
            if (isEnemyDead)
            {
                return;
            }
            if (gun == null && gunObjectRef.TryGet(out var networkObject))
            {
                gun = networkObject.gameObject.GetComponent<SlayerShotgun>();
                GrabGun(gun.gameObject);
            }
            if (gun == null)
            {
                return;
            }
            if (walkCheckInterval <= 0f)
            {
                walkCheckInterval = 0.1f;
                creatureAnimator.SetBool("IsWalking", (transform.position - positionLastCheck).sqrMagnitude > 0.001f);
                positionLastCheck = transform.position;
            }
            else
            {
                walkCheckInterval -= Time.deltaTime;
            }
            if (stunNormalizedTimer >= 0f)
            {
                agent.speed = 0f;
                return;
            }
            timeSinceSeeingTarget += Time.deltaTime;
            timeSinceFiringGun += Time.deltaTime;
            timeSinceHittingPlayer += Time.deltaTime;
            creatureAnimator.SetInteger("State", currentBehaviourStateIndex);
            creatureAnimator.SetBool("Aiming", aimingGun);
            switch (currentBehaviourStateIndex)
            {
                case 0:
                    if (previousBehaviourState != currentBehaviourStateIndex)
                    {
                        previousBehaviourState = currentBehaviourStateIndex;
                        lostPlayerInChase = false;
                        creatureVoice.Stop();
                    }
                    agent.speed = speedWhileMoving;
                    targetTorsoDegrees = 0;
                    torsoTurnSpeed = 525f;
                    if (CheckLineOfSightForTarget(widthSearch, rangeSearch, 1))
                    {
                        if (target == GameNetworkManager.Instance.localPlayerController.gameplayCamera.transform)
                        {
                            SeeMovingThreatServerRpc(Vector3.zero, false, (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                        }
                        else
                        {
                            SeeMovingThreatServerRpc(target.position, false, -1);
                        }
                    }
                    break;
                case 2:
                    if (previousBehaviourState != currentBehaviourStateIndex)
                    {
                        if (previousBehaviourState != 1)
                        {
                            longRangeAudio.PlayOneShot(enemyType.audioClips[3]);
                        }
                        previousBehaviourState = currentBehaviourStateIndex;
                    }
                    if (IsOwner)
                    {
                        if (aimingGun || timeSinceFiringGun < 1.2f && timeSinceSeeingTarget < 0.5f || timeSinceHittingPlayer < 1f)
                        {
                            if (aimingGun)
                            {
                                agent.speed = speedWhileAiming;
                            }
                            else
                            {
                                agent.speed = 0f;
                            }
                        }
                        else
                        {
                            agent.speed = 7f;
                        }
                    }
                    if (lostPlayerInChase)
                    {
                        targetTorsoDegrees = 0;
                    }
                    else
                    {
                        SetTargetDegreesToPosition(lastSeenPlayerPos);
                    }
                    if (lastPlayerSeenMoving != -1)
                    {
                        // Player target sync
                        PlayerControllerB playerTarget = StartOfRound.Instance.allPlayerScripts[lastPlayerSeenMoving];
                        target = playerTarget.gameplayCamera.transform;
                        if (CheckLineOfSightForPosition(target.position, widthSearch, rangeSearch, 1f))
                        {
                            timeSinceSeeingTarget = 0f;
                            lastSeenPlayerPos = playerTarget.transform.position;
                        }
                        else if (!CheckLineOfSightForPosition(GameNetworkManager.Instance.localPlayerController.gameplayCamera.transform.position, 70f, 12, 1f))
                        {
                            break;
                        }

                        if (CheckLineOfSightForPosition(target.position, widthSearch, 12, 1f) && timeSinceSeeingTarget < 3.0f)
                        {
                            SetTargetDegreesToPosition(playerTarget.transform.position);

                            TurnTorsoToTargetDegrees();
                            if (timeSinceFiringGun > gun.useCooldown && !aimingGun && timeSinceHittingPlayer > 1f)
                            {
                                timeSinceFiringGun = 0f;
                                agent.speed = 0f;
                                AimGunServerRpc(transform.position, lastPlayerSeenMoving);
                            }
                            if (lostPlayerInChase)
                            {
                                lostPlayerInChase = false;
                                SetLostPlayerInChaseServerRpc(lostPlayer: false);
                            }
                            timeSinceSeeingTarget = 0f;
                            lastSeenPlayerPos = playerTarget.transform.position;
                        }
                        else if (IsLocalPlayerMoving())
                        {
                            bool flag = (int)GameNetworkManager.Instance.localPlayerController.playerClientId == lastPlayerSeenMoving;
                            if (flag)
                            {
                                timeSinceSeeingTarget = 0f;
                            }
                            float currentTargetDistance = (transform.position - playerTarget.transform.position).magnitude;
                            float localPlayerDistance = (transform.position - GameNetworkManager.Instance.localPlayerController.transform.position).magnitude;
                            if (currentTargetDistance - localPlayerDistance > 3f || timeSinceSeeingTarget > 3f && !flag)
                            {
                                lastPlayerSeenMoving = (int)GameNetworkManager.Instance.localPlayerController.playerClientId;
                                SwitchTargetServerRpc(Vector3.zero, (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                            }
                        }

                        break;
                    }

                    bool hasCurrentTargetSight = false;
                    if (target != null)
                    {
                        Transform lineOfSightOrigin = eye ?? transform;
                        Vector3 targetPosition = target.position;
                        Vector3 toTarget = targetPosition - lineOfSightOrigin.position;
                        if (toTarget.sqrMagnitude < rangeSearch * rangeSearch && !Physics.Linecast(lineOfSightOrigin.position, targetPosition, StartOfRound.Instance.collidersAndRoomMaskAndDefault))
                        {
                            hasCurrentTargetSight = Vector3.Angle(lineOfSightOrigin.forward, toTarget) < widthSearch || Vector3.SqrMagnitude(transform.position - targetPosition) < 1f;
                        }
                    }
                    if (hasCurrentTargetSight)
                    {
                        timeSinceSeeingTarget = 0f;

                        if (target != null)
                            lastSeenPlayerPos = target.position;
                    }
                    else if (!CheckLineOfSightForTarget(70f, 12, 1))
                    {
                        break;
                    }
                    if (CheckLineOfSightForTarget(widthSearch, 12, 1) && timeSinceSeeingTarget < 3.0f)
                    {
                        if (target != null)
                            SetTargetDegreesToPosition(target.position);

                        TurnTorsoToTargetDegrees();
                        if (timeSinceFiringGun > gun.useCooldown && !aimingGun && timeSinceHittingPlayer > 1f)
                        {
                            timeSinceFiringGun = 0f;
                            agent.speed = 0f;
                            int targetPlayerId = lastPlayerSeenMoving;
                            if (target == GameNetworkManager.Instance.localPlayerController.gameplayCamera.transform)
                            {
                                targetPlayerId = (int)GameNetworkManager.Instance.localPlayerController.playerClientId;
                            }
                            AimGunServerRpc(transform.position, targetPlayerId);
                        }
                        if (lostPlayerInChase)
                        {
                            lostPlayerInChase = false;
                            SetLostPlayerInChaseServerRpc(lostPlayer: false);
                        }
                        timeSinceSeeingTarget = 0f;

                        if (target != null)
                            lastSeenPlayerPos = target.position;
                    }
                    break;
            }
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        public void AimGunServerRpc(Vector3 enemyPos, int playerId)
        {
            if (!aimingGun)
            {
                aimingGun = true;
                AimGunClientRpc(enemyPos, playerId);
            }
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void AimGunClientRpc(Vector3 enemyPos, int playerId)
        {
            if (playerId != -1)
            {
                // Set every client to the same target before firing
                SwitchTargetTo(Vector3.zero, playerId);
            }
            StopAimingGun();
            gunCoroutine = StartCoroutine(AimGun(enemyPos));

            IEnumerator AimGun(Vector3 aimEnemyPos)
            {
                aimingGun = true;
                if (lastPlayerSeenMoving == previousPlayerSeenWhenAiming)
                {
                    timesSeeingSamePlayer++;
                }
                else
                {
                    previousPlayerSeenWhenAiming = lastPlayerSeenMoving;
                    timesSeeingSamePlayer = 0;
                }
                longRangeAudio.PlayOneShot(aimSFX);
                speedWhileAiming = speedWhileMoving * 0.35f;
                inSpecialAnimation = true;
                serverPosition = aimEnemyPos;
                yield return new WaitForSeconds(0.9f);
                yield return new WaitForEndOfFrame();
                if (IsOwner && !isFiring && gun != null && gun.shotgunRayPoint != null)
                {
                    Vector3 gunPosition = gun.shotgunRayPoint.position;
                    Vector3 gunForward = gun.shotgunRayPoint.forward;
                    if (target != null)
                    {
                        gunForward = new Vector3(
                            gun.shotgunRayPoint.forward.x,
                            (target.position - gun.shotgunRayPoint.position).normalized.y,
                            gun.shotgunRayPoint.forward.z
                        );
                    }
                    FireGunServerRpc(gunPosition, gunForward);
                }
                timeSinceFiringGun = 0f;
                yield return new WaitForSeconds(0.35f);
                aimingGun = false;
                inSpecialAnimation = false;
                creatureVoice.Play();
                creatureVoice.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            }
        }

        [Rpc(SendTo.Server)]
        public void FireGunServerRpc(Vector3 gunPosition, Vector3 gunForward)
        {
            if (stunNormalizedTimer <= 0f)
            {
                FireGunClientRpc(gunPosition, gunForward);
            }
            else
            {
                StartCoroutine(WaitToFireGun(gunPosition, gunForward));
            }

            IEnumerator WaitToFireGun(Vector3 delayedGunPosition, Vector3 delayedGunForward)
            {
                yield return new WaitUntil(() => stunNormalizedTimer <= 0f);
                yield return new WaitForSeconds(0.5f);
                FireGunClientRpc(delayedGunPosition, delayedGunForward);
            }
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void FireGunClientRpc(Vector3 gunPosition, Vector3 gunForward)
        {
            if (gun == null)
            {
                Log.LogError("FireGunClientRpc failed: gun is null");
                return;
            }

            if (gun.shotgunRayPoint == null)
            {
                Log.LogError("FireGunClientRpc failed: gun.shotgunRayPoint is null");
                return;
            }

            isFiring = true;
            Fire(gunPosition, gunForward);
            StartCoroutine(FireAfterDelay(0.35f, gunPosition, gunForward));
            StartCoroutine(FireAfterDelay(0.7f, gunPosition, gunForward));
            isFiring = false;

            IEnumerator FireAfterDelay(float time, Vector3 delayedGunPosition, Vector3 delayedGunForward)
            {
                yield return new WaitForSeconds(time);
                gun.currentUseCooldown = -1.0f;
                Fire(delayedGunPosition, delayedGunForward);
            }

            void Fire(Vector3 firePosition, Vector3 fireForward)
            {
                creatureAnimator.ResetTrigger("ShootGun");
                creatureAnimator.SetTrigger("ShootGun");
                if (gun == null)
                {
                    LogEnemyError("No gun held on local client, unable to shoot");
                }
                else
                {
                    gun.ShootGun(firePosition, fireForward);
                }
            }
        }

        private void StopAimingGun()
        {
            inSpecialAnimation = false;
            aimingGun = false;
            if (gunCoroutine != null)
            {
                StopCoroutine(gunCoroutine);
            }
        }


        [Rpc(SendTo.Server, RequireOwnership = false)]
        public void SwitchTargetServerRpc(Vector3 position, int playerId = -1)
        {
            SwitchTargetClientRpc(position, playerId);
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void SwitchTargetClientRpc(Vector3 position, int playerId = -1)
        {
            SwitchTargetTo(position, playerId);
        }

        private void SwitchTargetTo(Vector3 position, int playerId = -1)
        {
            if (playerId != -1)
            {
                lastPlayerSeenMoving = playerId;
                timeSinceSeeingTarget = 0f;
                target = StartOfRound.Instance.allPlayerScripts[playerId].gameplayCamera.transform;
                lastSeenPlayerPos = StartOfRound.Instance.allPlayerScripts[playerId].transform.position;
            }
            else
            {
                lastPlayerSeenMoving = -1;
                timeSinceSeeingTarget = 0f;
                lastSeenPlayerPos = position;
            }
        }

        public bool CheckLineOfSightForTarget(float width = 45f, int range = 60, int proximityAwareness = -1)
        {
            Transform lineOfSightOrigin = eye ?? transform;
            Vector3 origin = lineOfSightOrigin.position;
            float rangeSqr = range * range;
            float proximityAwarenessSqr = proximityAwareness * proximityAwareness;
            Vector3 position = GameNetworkManager.Instance.localPlayerController.gameplayCamera.transform.position;
            Vector3 to = position - origin;
            if (to.sqrMagnitude < rangeSqr && !Physics.Linecast(origin, position, StartOfRound.Instance.collidersAndRoomMaskAndDefault))
            {
                if (Vector3.Angle(lineOfSightOrigin.forward, to) < width || proximityAwareness != -1 && to.sqrMagnitude < proximityAwarenessSqr)
                {
                    target = GameNetworkManager.Instance.localPlayerController.gameplayCamera.transform;
                    return true;
                }
            }
            foreach (EnemyAI ai in RoundManager.Instance.SpawnedEnemies)
            {
                if (ai == null || ai.transform == null || ai.isEnemyDead) continue; // Skip
                position = ai.transform.position;
                to = position - origin;
                if (to.sqrMagnitude < rangeSqr && !Physics.Linecast(origin, position, StartOfRound.Instance.collidersAndRoomMaskAndDefault))
                {
                    if (Vector3.Angle(lineOfSightOrigin.forward, to) < width || proximityAwareness != -1 && to.sqrMagnitude < proximityAwarenessSqr)
                    {
                        target = ai.transform;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsLocalPlayerMoving()
        {
            localPlayerTurnDistance += StartOfRound.Instance.playerLookMagnitudeThisFrame;
            if (localPlayerTurnDistance > 0.1f && Vector3.SqrMagnitude(GameNetworkManager.Instance.localPlayerController.transform.position - transform.position) < 100f)
            {
                return true;
            }
            if (GameNetworkManager.Instance.localPlayerController.performingEmote)
            {
                return true;
            }
            if (Time.realtimeSinceStartup - StartOfRound.Instance.timeAtMakingLastPersonalMovement < 0.25f)
            {
                return true;
            }
            if (GameNetworkManager.Instance.localPlayerController.timeSincePlayerMoving < 0.02f)
            {
                return true;
            }
            return false;
        }

        public override void OnCollideWithPlayer(Collider other)
        {
            base.OnCollideWithPlayer(other);
            if (!isEnemyDead && !(timeSinceHittingPlayer < 1f) && !(stunNormalizedTimer >= 0f))
            {
                PlayerControllerB playerControllerB = MeetsStandardPlayerCollisionConditions(other, aimingGun);
                if (playerControllerB != null)
                {
                    timeSinceHittingPlayer = 0f;
                    LegKickPlayerServerRpc((int)playerControllerB.playerClientId);
                }
            }
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        public void LegKickPlayerServerRpc(int playerId)
        {
            if (playerId < 0 || playerId >= StartOfRound.Instance.allPlayerScripts.Length)
            {
                return;
            }
            PlayerControllerB playerControllerB = StartOfRound.Instance.allPlayerScripts[playerId];
            if (playerControllerB == null || playerControllerB.isPlayerDead || Vector3.SqrMagnitude(playerControllerB.transform.position - transform.position) > 9f)
            {
                return;
            }
            // Server checks kick range so it cant kill across clients
            LegKickPlayerClientRpc(playerId);
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void LegKickPlayerClientRpc(int playerId)
        {
            timeSinceHittingPlayer = 0f;
            PlayerControllerB playerControllerB = StartOfRound.Instance.allPlayerScripts[playerId];
            RoundManager.Instance.tempTransform.position = transform.position;
            RoundManager.Instance.tempTransform.LookAt(playerControllerB.transform.position);
            transform.eulerAngles = new Vector3(0f, RoundManager.Instance.tempTransform.eulerAngles.y, 0f);
            serverRotation = new Vector3(0f, RoundManager.Instance.tempTransform.eulerAngles.y, 0f);
            Vector3 bodyVelocity = Vector3.Normalize((playerControllerB.transform.position + Vector3.up * 0.75f - transform.position) * 100f) * 25f;
            if (playerControllerB.IsOwner)
            {
                playerControllerB.KillPlayer(bodyVelocity, spawnBody: true, CauseOfDeath.Kicking);
            }
            creatureAnimator.SetTrigger("Kick");
            creatureSFX.Stop();
            torsoTurnAudio.volume = 0f;
            creatureSFX.PlayOneShot(kickSFX);
            if (currentBehaviourStateIndex != 2)
            {
                SwitchTargetTo(Vector3.zero, playerId);
                SwitchToBehaviourStateOnLocalClient(2);
            }
        }

        public override void HitEnemy(int force = 1, PlayerControllerB? playerWhoHit = null, bool playHitSFX = false, int hitID = -1)
        {
            if (Immortal) return;
            if (onlyPlayers)
            {
                if (playerWhoHit == null) return;
            }
            base.HitEnemy(force, playerWhoHit, playHitSFX, hitID);
            if (!isEnemyDead)
            {
                if (currentBehaviourStateIndex == 2)
                {
                    creatureSFX.PlayOneShot(enemyType.audioClips[0]);
                    enemyHP -= force;
                }
                else
                {
                    creatureSFX.PlayOneShot(enemyType.audioClips[1]);
                }
                if (playerWhoHit != null)
                {
                    SeeMovingThreatServerRpc(Vector3.zero, enterAttackFromPatrolMode: true, (int)playerWhoHit.playerClientId);
                }
                if (enemyHP <= 0 && IsOwner)
                {
                    KillEnemyOnOwnerClient();
                }
            }
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        private void SetLivesServerRpc(int value) => SetLivesClientRpc(value);

        [Rpc(SendTo.ClientsAndHost)]
        private void SetLivesClientRpc(int value)
        {
            Lives = value;
            enemyHP = setHp;
        }

        public override void KillEnemy(bool destroy = false)
        {
            if (Immortal) return;

            Lives--;
            enemyHP = setHp;
            if (NetworkManager.Singleton.IsServer) SetLivesServerRpc(Lives);

            if (Lives > 0)
            {
                return;
            }

            StopActiveSearches();
            base.KillEnemy(destroy);
            targetTorsoDegrees = 0;
            StopAimingGun();
            if (IsOwner && gun != null)
            {
                DropGunServerRpc(gunPoint.position);
            }
            creatureVoice.Stop();
            torsoTurnAudio.Stop();
        }

    }
}
