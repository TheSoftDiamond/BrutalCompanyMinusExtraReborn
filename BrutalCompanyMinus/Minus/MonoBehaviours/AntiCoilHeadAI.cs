using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;

using System.Collections.Generic;

namespace BrutalCompanyMinus.Minus.MonoBehaviours
{
    public class AntiCoilHeadAI : EnemyAI
    {
        public AISearchRoutine searchForPlayers = null!;

        private readonly List<PlayerControllerB> lookingPlayerQueue = [];

        private bool stoppingMovement;

        private bool hasStopped;

        public AnimationStopPoints animStopPoints = null!;

        private float currentChaseSpeed = 18f;

        private float currentAnimSpeed = 1f;

        private PlayerControllerB? previousTarget;

        private bool wasOwnerLastFrame;

        private float stopAndGoMinimumInterval;

        private float hitPlayerTimer;

        private float stopMovementTimer;

        public AudioClip[] springNoises = null!;

        public Collider mainCollider = null!;

        private bool PlayerIsInAntiCoilArea(PlayerControllerB player)
        {
            // Keep outside and facility targeting separate
            if (isOutside)
            {
                return !player.isInsideFactory && !player.isInHangarShipRoom;
            }
            return player.isInsideFactory;
        }

        private bool PlayerIsLookingAtAntiCoil(PlayerControllerB player)
        {
            if (!PlayerIsInAntiCoilArea(player) || !PlayerIsTargetable(player))
            {
                return false;
            }

            Transform lineOfSightOrigin = eye ?? transform;
            return player.HasLineOfSightToPosition(transform.position + Vector3.up * 1.6f, 60f) && Vector3.SqrMagnitude(player.gameplayCamera.transform.position - lineOfSightOrigin.position) > 0.09f;
        }

        private bool ShouldRoamWithoutPlayers()
        {
            // Outside ones shouldnt camp entrances forever
            if (StartOfRound.Instance.allPlayersDead)
            {
                return false;
            }

            for (int i = 0; i < StartOfRound.Instance.allPlayerScripts.Length; i++)
            {
                PlayerControllerB player = StartOfRound.Instance.allPlayerScripts[i];
                if (!PlayerIsInAntiCoilArea(player) || !PlayerIsTargetable(player))
                {
                    continue;
                }
                if (!isOutside || Vector3.SqrMagnitude(transform.position - player.transform.position) < 2500f)
                {
                    return false;
                }
            }
            return true;
        }

        private bool TargetFirstLookingPlayer(out PlayerControllerB firstLookingPlayer)
        {
            for (int i = lookingPlayerQueue.Count - 1; i >= 0; i--)
            {
                if (!PlayerIsLookingAtAntiCoil(lookingPlayerQueue[i]))
                {
                    lookingPlayerQueue.RemoveAt(i);
                }
            }

            for (int i = 0; i < StartOfRound.Instance.allPlayerScripts.Length; i++)
            {
                PlayerControllerB player = StartOfRound.Instance.allPlayerScripts[i];
                if (!PlayerIsLookingAtAntiCoil(player) || lookingPlayerQueue.Contains(player))
                {
                    continue;
                }

                lookingPlayerQueue.Add(player);
            }

            if (lookingPlayerQueue.Count == 0)
            {
                firstLookingPlayer = null!;
                return false;
            }

            firstLookingPlayer = lookingPlayerQueue[0];
            targetPlayer = firstLookingPlayer;
            return true;
        }

        private void StopMovingOnOwner()
        {
            if (!IsOwner)
            {
                return;
            }

            // Owner has to kill the nav movement
            if (agent.speed != 0f || movingTowardsTargetPlayer)
            {
                agent.speed = 0f;
                movingTowardsTargetPlayer = false;
                SetDestinationToPosition(transform.position);
            }
        }

        private void StopActiveSearches()
        {
            if (searchForPlayers.inProgress)
            {
                StopSearch(searchForPlayers);
            }
        }

        public override void DoAIInterval()
        {
            base.DoAIInterval();
            if (StartOfRound.Instance.allPlayersDead || isEnemyDead)
            {
                StopActiveSearches();
                return;
            }
            switch (currentBehaviourStateIndex)
            {
                case 0:
                    {
                        if (!IsServer)
                        {
                            ChangeOwnershipOfEnemy(StartOfRound.Instance.allPlayerScripts[0].actualClientId);
                            break;
                        }

                        // only wake up when someone is actually looking
                        if (TargetFirstLookingPlayer(out PlayerControllerB wakeTargetPlayer))
                        {
                            previousTarget = wakeTargetPlayer;
                            ChangeOwnershipOfEnemy(wakeTargetPlayer.actualClientId);
                            SwitchToBehaviourState(1);
                            return;
                        }

                        if (ShouldRoamWithoutPlayers())
                        {
                            previousTarget = null;
                            addPlayerVelocityToDestination = 0f;
                            agent.speed = 6f;
                            movingTowardsTargetPlayer = false;
                            if (!searchForPlayers.inProgress)
                            {
                                StartSearch(transform.position, searchForPlayers);
                            }
                            break;
                        }

                        if (searchForPlayers.inProgress)
                        {
                            StopSearch(searchForPlayers);
                        }
                        previousTarget = null;
                        addPlayerVelocityToDestination = 0f;
                        StopMovingOnOwner();
                        creatureAnimator.SetFloat("walkSpeed", 0f);
                        currentAnimSpeed = 0f;
                        break;
                    }
                case 1:
                    if (searchForPlayers.inProgress)
                    {
                        StopSearch(searchForPlayers);
                    }
                    if (TargetFirstLookingPlayer(out PlayerControllerB chaseTargetPlayer))
                    {
                        if (previousTarget != chaseTargetPlayer)
                        {
                            previousTarget = chaseTargetPlayer;
                            ChangeOwnershipOfEnemy(chaseTargetPlayer.actualClientId);
                        }
                        agent.stoppingDistance = 0f;
                        agent.acceleration = 50f;
                        addPlayerVelocityToDestination = 0f;
                        movingTowardsTargetPlayer = true;
                    }
                    else
                    {
                        // Stop chasing when no one looking
                        addPlayerVelocityToDestination = 0f;
                        movingTowardsTargetPlayer = false;
                        StopMovingOnOwner();
                        SwitchToBehaviourState(0);
                        ChangeOwnershipOfEnemy(StartOfRound.Instance.allPlayerScripts[0].actualClientId);
                    }
                    break;
            }
        }

        public override void Update()
        {
            base.Update();
            if (isEnemyDead)
            {
                return;
            }
            if (hitPlayerTimer >= 0f)
            {
                hitPlayerTimer -= Time.deltaTime;
            }
            if (currentBehaviourStateIndex != 1)
            {
                if (ShouldRoamWithoutPlayers())
                {
                    stoppingMovement = false;
                    stopMovementTimer = 0f;
                    if (hasStopped)
                    {
                        hasStopped = false;
                        mainCollider.isTrigger = true;
                    }
                    currentAnimSpeed = Mathf.Lerp(currentAnimSpeed, 2f, 5f * Time.deltaTime);
                    creatureAnimator.SetFloat("walkSpeed", currentAnimSpeed);
                    if (IsOwner)
                    {
                        addPlayerVelocityToDestination = 0f;
                        agent.speed = 6f;
                        movingTowardsTargetPlayer = false;
                    }
                    return;
                }
                stoppingMovement = true;
                stopMovementTimer = 0f;
                creatureAnimator.SetFloat("walkSpeed", 0f);
                currentAnimSpeed = 0f;
                StopMovingOnOwner();
                return;
            }
            if (!IsOwner)
            {
                wasOwnerLastFrame = false;
            }
            if (IsOwner)
            {
                if (stopAndGoMinimumInterval > 0f)
                {
                    stopAndGoMinimumInterval -= Time.deltaTime;
                }
                if (!wasOwnerLastFrame)
                {
                    wasOwnerLastFrame = true;
                    if (!stoppingMovement)
                    {
                        agent.speed = currentChaseSpeed;
                    }
                    else
                    {
                        agent.speed = 0f;
                    }
                }
                bool flag = true;
                for (int i = 0; i < StartOfRound.Instance.allPlayerScripts.Length; i++)
                {
                    if (PlayerIsLookingAtAntiCoil(StartOfRound.Instance.allPlayerScripts[i]))
                    {
                        flag = false;
                        break;
                    }
                }
                if (stunNormalizedTimer > 0f)
                {
                    flag = true;
                }
                if (flag != stoppingMovement && stopAndGoMinimumInterval <= 0f)
                {
                    stopAndGoMinimumInterval = 0.15f;
                    if (flag)
                    {
                        SetAnimationStopServerRpc();
                    }
                    else
                    {
                        SetAnimationGoServerRpc();
                    }
                    stoppingMovement = flag;
                }
            }
            if (stoppingMovement)
            {
                if (!animStopPoints.canAnimationStop)
                {
                    // dont let anticoilhead slide forever
                    // might be overkill but gets the job done
                    stopMovementTimer += Time.deltaTime;
                    if (stopMovementTimer <= 0.27f)
                    {
                        return;
                    }
                }
                else
                {
                    stopMovementTimer = 0f;
                }
                if (!hasStopped)
                {
                    hasStopped = true;
                    if (GameNetworkManager.Instance.localPlayerController.HasLineOfSightToPosition(transform.position, 60f, 25))
                    {
                        float num2 = Vector3.SqrMagnitude(transform.position - GameNetworkManager.Instance.localPlayerController.transform.position);
                        if (num2 < 16f)
                        {
                            GameNetworkManager.Instance.localPlayerController.JumpToFearLevel(0.9f);
                        }
                        else if (num2 < 81f)
                        {
                            GameNetworkManager.Instance.localPlayerController.JumpToFearLevel(0.4f);
                        }
                    }
                    if (currentAnimSpeed > 2f)
                    {
                        RoundManager.PlayRandomClip(creatureVoice, springNoises, randomize: false);
                        if (animStopPoints.animationPosition == 1)
                        {
                            creatureAnimator.SetTrigger("springBoing");
                        }
                        else
                        {
                            creatureAnimator.SetTrigger("springBoingPosition2");
                        }
                    }
                }
                if (mainCollider.isTrigger && Vector3.SqrMagnitude(GameNetworkManager.Instance.localPlayerController.transform.position - transform.position) > 0.0625f)
                {
                    mainCollider.isTrigger = false;
                }
                creatureAnimator.SetFloat("walkSpeed", 0f);
                currentAnimSpeed = 0f;
                if (IsOwner)
                {
                    addPlayerVelocityToDestination = 0f;
                    StopMovingOnOwner();
                }
            }
            else
            {
                stopMovementTimer = 0f;
                if (hasStopped)
                {
                    hasStopped = false;
                    mainCollider.isTrigger = true;
                }
                currentAnimSpeed = Mathf.Lerp(currentAnimSpeed, 6f, 5f * Time.deltaTime);
                creatureAnimator.SetFloat("walkSpeed", currentAnimSpeed);
                if (IsOwner)
                {
                    agent.stoppingDistance = 0f;
                    agent.acceleration = 50f;
                    addPlayerVelocityToDestination = 0f;
                    agent.speed = Mathf.Lerp(agent.speed, currentChaseSpeed, 4.5f * Time.deltaTime);
                    movingTowardsTargetPlayer = true;
                }
            }
        }

        public override void NavigateTowardsTargetPlayer()
        {
            if (targetPlayer == null)
            {
                return;
            }
            if (setDestinationToPlayerInterval <= 0f)
            {
                setDestinationToPlayerInterval = 0.05f;
                destination = targetPlayer.transform.position;
                moveTowardsDestination = true;
                if (agent.enabled && agent.isOnNavMesh)
                {
                    agent.SetDestination(destination);
                }
            }
            else
            {
                setDestinationToPlayerInterval -= Time.deltaTime;
            }
        }

        [Rpc(SendTo.Server)]
        public void SetAnimationStopServerRpc()
        {
            SetAnimationStopClientRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void SetAnimationStopClientRpc()
        {
            stoppingMovement = true;
        }

        [Rpc(SendTo.Server)]
        public void SetAnimationGoServerRpc()
        {
            SetAnimationGoClientRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void SetAnimationGoClientRpc()
        {
            stoppingMovement = false;
        }

        public override void OnCollideWithPlayer(Collider other)
        {
            base.OnCollideWithPlayer(other);
            if (!stoppingMovement && currentBehaviourStateIndex == 1 && !(hitPlayerTimer >= 0f))
            {
                PlayerControllerB playerControllerB = MeetsStandardPlayerCollisionConditions(other);
                if (playerControllerB != null)
                {
                    if (playerControllerB.IsOwner)
                    {
                        hitPlayerTimer = 0.2f;
                        Vector3 bodyVelocity = Vector3.Normalize(playerControllerB.gameplayCamera.transform.position - transform.position) * 80f;
                        playerControllerB.KillPlayer(bodyVelocity, spawnBody: true, CauseOfDeath.Mauling);
                        playerControllerB.JumpToFearLevel(1f);
                    }
                }
            }
        }

        public override void KillEnemy(bool destroy = false)
        {
            StopActiveSearches();
            base.KillEnemy(destroy);
        }
    }
}
