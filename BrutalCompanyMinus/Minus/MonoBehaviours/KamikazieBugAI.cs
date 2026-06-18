using GameNetcodeStuff;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System.Collections;
using UnityEngine.Animations.Rigging;

namespace BrutalCompanyMinus.Minus.MonoBehaviours
{
    internal class KamikazieBugAI : EnemyAI
    {
        public AISearchRoutine searchForItems = null!;

        public AISearchRoutine searchForPlayer = null!;

        [Header("Tracking/Memory")]
        [Space(3f)]
        public Vector3 nestPosition;

        private bool choseNestPosition;

        [Space(3f)]
        public static List<HoarderBugItem> HoarderBugItems = [];

        public static List<GameObject> grabbableObjectsInMap = [];

        public float angryTimer;

        public GrabbableObject targetItem = null!;

        // Remember last item path so lootbug dont repath every AI tick
        private GameObject targetDestinationObject = null!;

        private Vector3 targetDestinationPosition;

        public HoarderBugItem heldItem = null!;

        [Header("Animations")]
        [Space(5f)]
        private Vector3 agentLocalVelocity;

        private Vector3 previousPosition;

        private float velX;

        private float velZ;

        public Transform turnCompass = null!;

        private float armsHoldLayerWeight;

        [Space(5f)]
        public Transform animationContainer = null!;

        public Transform grabTarget = null!;

        public MultiAimConstraint headLookRig = null!;

        public Transform headLookTarget = null!;

        [Header("Special behaviour states")]
        private float annoyanceMeter;

        public bool watchingPlayerNearPosition;

        public PlayerControllerB watchingPlayer = null!;

        public Transform lookTarget = null!;

        public bool lookingAtPositionOfInterest;

        private Vector3 positionOfInterest;

        private bool isAngry;

        [Header("Misc logic")]
        private bool sendingGrabOrDropRPC;

        private float waitingAtNestTimer;

        private bool waitingAtNest;

        private float timeSinceSeeingAPlayer;

        [Header("Chase logic")]
        private bool lostPlayerInChase;

        public PlayerControllerB angryAtPlayer = null!;

        private bool inChase;

        [Header("Audios")]
        public AudioClip[] chitterSFX = null!;

        [Header("Audios")]
        public AudioClip[] angryScreechSFX = null!;

        public AudioClip angryVoiceSFX = null!;

        public AudioClip bugFlySFX = null!;

        public AudioClip hitPlayerSFX = null!;

        private float timeSinceHittingPlayer;

        private float timeSinceLookingTowardsNoise;

        private float detectPlayersInterval;

        private bool inReturnToNestMode;

        // Custom variables

        public AudioSource tickingAudio = null!;

        public Light bugLight = null!;

        public Transform mainTransform = null!;

        private bool onBlowUpSequence;

        private readonly List<Coroutine> blowupCoroutines = [];

        public override void Start()
        {
            base.Start();
            heldItem = null!;
            grabbableObjectsInMap.Clear();
            GrabbableObject[] grabbableObjects = FindObjectsOfType<GrabbableObject>();
            for (int i = 0; i < grabbableObjects.Length; i++)
            {
                if (grabbableObjects[i].grabbableToEnemies && !grabbableObjects[i].deactivated)
                {
                    grabbableObjectsInMap.Add(grabbableObjects[i].gameObject);
                }
            }

            if (!Compatibility.yippeeModCompatibilityMode || Compatibility.yippeeNewSFX == null) return;

            chitterSFX = Compatibility.yippeeNewSFX;
        }

        private bool GrabTargetItemIfClose()
        {
            if (targetItem != null && heldItem == null && Vector3.SqrMagnitude(transform.position - targetItem.transform.position) < 0.5625f)
            {
                if (!SetDestinationToPosition(nestPosition, checkForPath: true))
                {
                    nestPosition = ChooseClosestNodeToPosition(transform.position).position;
                    SetDestinationToPosition(nestPosition);
                }
                NetworkObject component = targetItem.GetComponent<NetworkObject>();
                SwitchToBehaviourStateOnLocalClient(1);
                GrabItem(component);
                sendingGrabOrDropRPC = true;
                GrabItemServerRpc(component);
                return true;
            }
            return false;
        }

        private void ChooseNestPosition()
        {
            HoarderBugAI[] array = FindObjectsOfType<HoarderBugAI>();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != this && !PathIsIntersectedByLineOfSight(array[i].nestPosition, calculatePathDistance: false, avoidLineOfSight: false))
                {
                    nestPosition = array[i].nestPosition;
                    SyncNestPositionServerRpc(nestPosition);
                    return;
                }
            }
            nestPosition = ChooseClosestNodeToPosition(transform.position).position;
            SyncNestPositionServerRpc(nestPosition);
        }

        [Rpc(SendTo.Server)]
        private void SyncNestPositionServerRpc(Vector3 newNestPosition)
        {
            SyncNestPositionClientRpc(newNestPosition);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void SyncNestPositionClientRpc(Vector3 newNestPosition)
        {
            nestPosition = newNestPosition;
        }

        public override void DoAIInterval()
        {

            base.DoAIInterval();
            if (isEnemyDead || StartOfRound.Instance.allPlayersDead)
            {
                StopActiveSearches();
                return;
            }
            if (!choseNestPosition)
            {
                choseNestPosition = true;
                ChooseNestPosition();
                return;
            }
            Transform lineOfSightOrigin = eye ?? transform;
            Vector3 toNest = nestPosition - lineOfSightOrigin.position;
            bool canSeeNest = toNest.sqrMagnitude < 1600f && !Physics.Linecast(lineOfSightOrigin.position, nestPosition, StartOfRound.Instance.collidersAndRoomMaskAndDefault);
            if (canSeeNest && (Vector3.Angle(lineOfSightOrigin.forward, toNest) < 60f || Vector3.SqrMagnitude(transform.position - nestPosition) < 0.25f))
            {
                for (int i = 0; i < HoarderBugItems.Count; i++)
                {
                    if (HoarderBugItems[i].itemGrabbableObject.isHeld && HoarderBugItems[i].itemNestPosition == nestPosition)
                    {
                        HoarderBugItems[i].status = HoarderBugItemStatus.Stolen;
                    }
                }
            }
            HoarderBugItem? hoarderBugItem = CheckLineOfSightForItem(HoarderBugItemStatus.Stolen, 60f, 30, 3f);
            if (hoarderBugItem != null && !hoarderBugItem.itemGrabbableObject.isHeld)
            {
                hoarderBugItem.status = HoarderBugItemStatus.Returned;
                if (!hoarderBugItem.itemGrabbableObject.deactivated && !grabbableObjectsInMap.Contains(hoarderBugItem.itemGrabbableObject.gameObject))
                {
                    grabbableObjectsInMap.Add(hoarderBugItem.itemGrabbableObject.gameObject);
                }
            }
            switch (currentBehaviourStateIndex)
            {
                case 0:
                    {
                        inReturnToNestMode = false;
                        ExitChaseMode();
                        if (GrabTargetItemIfClose())
                        {
                            break;
                        }
                        if (targetItem == null && !searchForItems.inProgress)
                        {
                            StartSearch(nestPosition, searchForItems);
                            break;
                        }
                        if (targetItem != null)
                        {
                            SetGoTowardsTargetObject(targetItem.gameObject);
                            break;
                        }
                        GameObject gameObject2 = CheckLineOfSight(grabbableObjectsInMap, 60f, 40, 5f);
                        if ((bool)gameObject2)
                        {
                            GrabbableObject component = gameObject2.GetComponent<GrabbableObject>();
                            if ((bool)component && !component.deactivated && (!component.isHeld || UnityEngine.Random.Range(0, 100) < 4 && !component.isPocketed))
                            {
                                SetGoTowardsTargetObject(gameObject2);
                            }
                        }
                        break;
                    }
                case 1:
                    ExitChaseMode();
                    if (!inReturnToNestMode)
                    {
                        inReturnToNestMode = true;
                        if (SetDestinationToPosition(nestPosition, checkForPath: true))
                        {
                            targetItem = null!;
                            StopSearch(searchForItems, clear: false);
                        }
                        else
                        {
                            ChooseNestPosition();
                        }
                    }
                    GrabTargetItemIfClose();
                    if (waitingAtNest)
                    {
                        if (heldItem != null)
                        {
                            DropItemAndCallDropRPC(heldItem.itemGrabbableObject.GetComponent<NetworkObject>());
                        }
                        else
                        {
                            GameObject gameObject = CheckLineOfSight(grabbableObjectsInMap, 60f, 40, 5f);

                            if (eye == null)
                                return;

                            if ((bool)gameObject && Vector3.SqrMagnitude(eye.position - gameObject.transform.position) < 36f)
                            {
                                targetItem = gameObject.GetComponent<GrabbableObject>();
                                if (targetItem != null && !targetItem.isHeld && !targetItem.deactivated)
                                {
                                    waitingAtNest = false;
                                    SwitchToBehaviourState(0);
                                    break;
                                }
                            }
                        }
                        if (waitingAtNestTimer <= 0f && !watchingPlayerNearPosition)
                        {
                            waitingAtNest = false;
                            SwitchToBehaviourStateOnLocalClient(0);
                        }
                    }
                    else if (Vector3.SqrMagnitude(transform.position - nestPosition) < 0.5625f)
                    {
                        waitingAtNest = true;
                        waitingAtNestTimer = 15f;
                    }
                    break;
                case 2:
                    inReturnToNestMode = false;
                    if (heldItem != null)
                    {
                        DropItemAndCallDropRPC(heldItem.itemGrabbableObject.GetComponent<NetworkObject>(), droppedInNest: false);
                    }
                    if (lostPlayerInChase)
                    {
                        if (!searchForPlayer.inProgress)
                        {
                            searchForPlayer.searchWidth = 30f;
                            StartSearch(targetPlayer.transform.position, searchForPlayer);
                        }
                        break;
                    }
                    if (targetPlayer == null)
                    {
                        if (watchingPlayer != null)
                        {
                            targetPlayer = watchingPlayer;
                        }
                    }
                    if (searchForPlayer.inProgress)
                    {
                        StopSearch(searchForPlayer);
                    }
                    movingTowardsTargetPlayer = true;
                    break;
                case 3:
                    break;
            }
        }

        private void SetGoTowardsTargetObject(GameObject foundObject)
        {
            if (foundObject == null)
            {
                targetItem = null!;
                targetDestinationObject = null!;
                return;
            }

            GrabbableObject component = foundObject.GetComponent<GrabbableObject>();
            if (!grabbableObjectsInMap.Contains(foundObject) || !(bool)component || component.deactivated)
            {
                targetItem = null!;
                targetDestinationObject = null!;
                return;
            }

            Vector3 foundObjectPosition = foundObject.transform.position;
            // Item barely moved, keep the current path
            if (targetItem == component && targetDestinationObject == foundObject && Vector3.SqrMagnitude(targetDestinationPosition - foundObjectPosition) < 0.25f)
            {
                return;
            }

            if (SetDestinationToPosition(foundObjectPosition, checkForPath: true))
            {
                targetDestinationObject = foundObject;
                targetDestinationPosition = foundObjectPosition;
                targetItem = component;
                StopSearch(searchForItems, clear: false);
            }
            else
            {
                targetItem = null!;
                targetDestinationObject = null!;
            }
        }

        // Vanilla cleanup, this was missing
        private void ExitChaseMode()
        {
            if (inChase)
            {
                inChase = false;
                if (searchForPlayer.inProgress)
                {
                    StopSearch(searchForPlayer);
                }
                movingTowardsTargetPlayer = false;
                creatureAnimator.SetBool("Chase", value: false);
                creatureSFX.Stop();
            }
        }

        private void LateUpdate()
        {
            if (!inSpecialAnimation && !isEnemyDead && !StartOfRound.Instance.allPlayersDead)
            {
                if (detectPlayersInterval <= 0f)
                {
                    detectPlayersInterval = 0.2f;
                    do
                    {
                        Vector3 b = currentBehaviourStateIndex != 1 ? transform.position : nestPosition;
                        PlayerControllerB[] allPlayersInLineOfSight = GetAllPlayersInLineOfSight(70f, 30, eye, 1.2f);
                        if (allPlayersInLineOfSight != null)
                        {
                            PlayerControllerB playerControllerB = watchingPlayer;
                            timeSinceSeeingAPlayer = 0f;
                            float num = 250000f;
                            bool flag = false;
                            if (stunnedByPlayer != null)
                            {
                                flag = true;
                                angryAtPlayer = stunnedByPlayer;
                            }
                            for (int i = 0; i < allPlayersInLineOfSight.Length; i++)
                            {
                                if (!flag && allPlayersInLineOfSight[i].currentlyHeldObjectServer != null)
                                {
                                    for (int j = 0; j < HoarderBugItems.Count; j++)
                                    {
                                        if (HoarderBugItems[j].itemGrabbableObject == allPlayersInLineOfSight[i].currentlyHeldObjectServer)
                                        {
                                            HoarderBugItems[j].status = HoarderBugItemStatus.Stolen;
                                            angryAtPlayer = allPlayersInLineOfSight[i];
                                            flag = true;
                                        }
                                    }
                                }
                                if (IsHoarderBugAngry() && allPlayersInLineOfSight[i] == angryAtPlayer)
                                {
                                    watchingPlayer = angryAtPlayer;
                                }
                                else
                                {
                                    float num2 = Vector3.SqrMagnitude(allPlayersInLineOfSight[i].transform.position - b);
                                    if (num2 < num)
                                    {
                                        num = num2;
                                        watchingPlayer = allPlayersInLineOfSight[i];
                                    }
                                }
                                float num3 = Vector3.SqrMagnitude(allPlayersInLineOfSight[i].transform.position - nestPosition);
                                if (HoarderBugItems.Count > 0)
                                {
                                    if ((num3 < 16f || inChase && num3 < 64f) && angryTimer < 3.25f)
                                    {
                                        angryAtPlayer = allPlayersInLineOfSight[i];
                                        watchingPlayer = allPlayersInLineOfSight[i];
                                        angryTimer = 3.25f;
                                        break;
                                    }
                                    if (!isAngry && currentBehaviourStateIndex == 0 && num3 < 64f && (targetItem == null || Vector3.SqrMagnitude(targetItem.transform.position - transform.position) > 56.25f) && IsOwner)
                                    {
                                        SwitchToBehaviourState(1);
                                    }
                                }
                                if (currentBehaviourStateIndex != 2 && Vector3.SqrMagnitude(transform.position - allPlayersInLineOfSight[i].transform.position) < 6.25f)
                                {
                                    annoyanceMeter += 0.2f;
                                    if (annoyanceMeter > 2.5f)
                                    {
                                        angryAtPlayer = allPlayersInLineOfSight[i];
                                        watchingPlayer = allPlayersInLineOfSight[i];
                                        angryTimer = 3.25f;
                                    }
                                }
                            }
                            watchingPlayerNearPosition = num < 36f;
                            if (watchingPlayer != playerControllerB)
                            {
                                RoundManager.PlayRandomClip(creatureVoice, chitterSFX);
                            }
                            if (!IsOwner)
                            {
                                break;
                            }
                            if (currentBehaviourStateIndex != 2)
                            {
                                if (IsHoarderBugAngry())
                                {
                                    lostPlayerInChase = false;
                                    targetPlayer = watchingPlayer;
                                    SwitchToBehaviourState(2);
                                }
                            }
                            else
                            {
                                targetPlayer = watchingPlayer;
                                if (lostPlayerInChase)
                                {
                                    lostPlayerInChase = false;
                                }
                            }
                            break;
                        }
                        timeSinceSeeingAPlayer += 0.2f;
                        watchingPlayerNearPosition = false;
                        if (currentBehaviourStateIndex != 2)
                        {
                            if (timeSinceSeeingAPlayer > 1.5f)
                            {
                                watchingPlayer = null!;
                            }
                            break;
                        }
                        if (timeSinceSeeingAPlayer > 1.25f)
                        {
                            watchingPlayer = null!;
                        }
                        if (IsOwner)
                        {
                            if (timeSinceSeeingAPlayer > 15f)
                            {
                            }
                            else if (timeSinceSeeingAPlayer > 2.5f)
                            {
                                lostPlayerInChase = true;
                            }
                        }
                    }
                    while (false);
                }
                else
                {
                    detectPlayersInterval -= Time.deltaTime;
                }

                bool animateLook = true;
                if (watchingPlayer != null)
                {
                    lookTarget.position = watchingPlayer.gameplayCamera.transform.position;
                }
                else
                {
                    if (!lookingAtPositionOfInterest)
                    {
                        agent.angularSpeed = 220f;
                        headLookRig.weight = Mathf.Lerp(headLookRig.weight, 0f, 10f);
                        animateLook = false;
                    }
                    else
                    {
                        lookTarget.position = positionOfInterest;
                    }
                }
                if (animateLook)
                {
                    if (IsOwner)
                    {
                        agent.angularSpeed = 0f;
                        turnCompass.LookAt(lookTarget);
                        transform.rotation = Quaternion.Lerp(transform.rotation, turnCompass.rotation, 6f * Time.deltaTime);
                        transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
                    }
                    float num = Vector3.Angle(transform.forward, lookTarget.position - transform.position);
                    if (num > 22f)
                    {
                        headLookRig.weight = Mathf.Lerp(headLookRig.weight, 1f * (Mathf.Abs(num - 180f) / 180f), 7f);
                    }
                    else
                    {
                        headLookRig.weight = Mathf.Lerp(headLookRig.weight, 1f, 7f);
                    }
                    headLookTarget.position = Vector3.Lerp(headLookTarget.position, lookTarget.position, 8f * Time.deltaTime);
                }

                agentLocalVelocity = animationContainer.InverseTransformDirection(Vector3.ClampMagnitude(transform.position - previousPosition, 1f) / (Time.deltaTime * 2f));
                velX = Mathf.Lerp(velX, agentLocalVelocity.x, 10f * Time.deltaTime);
                creatureAnimator.SetFloat("VelocityX", Mathf.Clamp(velX, -1f, 1f));
                velZ = Mathf.Lerp(velZ, agentLocalVelocity.z, 10f * Time.deltaTime);
                creatureAnimator.SetFloat("VelocityZ", Mathf.Clamp(velZ, -1f, 1f));
                previousPosition = transform.position;

                if (heldItem != null)
                {
                    armsHoldLayerWeight = Mathf.Lerp(armsHoldLayerWeight, 0.85f, 8f * Time.deltaTime);
                }
                else
                {
                    armsHoldLayerWeight = Mathf.Lerp(armsHoldLayerWeight, 0f, 8f * Time.deltaTime);
                }
                creatureAnimator.SetLayerWeight(1, armsHoldLayerWeight);
            }
        }

        private bool IsHoarderBugAngry()
        {
            if (stunNormalizedTimer > 0f)
            {
                angryTimer = 4f;
                if ((bool)stunnedByPlayer)
                {
                    angryAtPlayer = stunnedByPlayer;
                }
                return true;
            }
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < HoarderBugItems.Count; i++)
            {
                if (HoarderBugItems[i].status == HoarderBugItemStatus.Stolen)
                {
                    num2++;
                }
                else if (HoarderBugItems[i].status == HoarderBugItemStatus.Returned)
                {
                    num++;
                }
            }
            if (!(angryTimer > 0f))
            {
                return num2 > 0;
            }
            return true;
        }

        public override void Update()
        {
            base.Update();
            timeSinceHittingPlayer += Time.deltaTime;
            timeSinceLookingTowardsNoise += Time.deltaTime;
            if (timeSinceLookingTowardsNoise > 0.6f)
            {
                lookingAtPositionOfInterest = false;
            }
            if (inSpecialAnimation || isEnemyDead || StartOfRound.Instance.allPlayersDead)
            {
                return;
            }
            if (angryTimer >= 0f)
            {
                angryTimer -= Time.deltaTime;
            }
            creatureAnimator.SetBool("stunned", stunNormalizedTimer > 0f);
            bool flag = IsHoarderBugAngry();
            if (!isAngry && flag)
            {
                isAngry = true;
                creatureVoice.clip = angryVoiceSFX;
                creatureVoice.Play();
            }
            else if (isAngry && !flag)
            {
                isAngry = false;
                angryAtPlayer = null!;
                creatureVoice.Stop();
            }
            switch (currentBehaviourStateIndex)
            {
                case 0:
                    ExitChaseMode();
                    addPlayerVelocityToDestination = 0f;
                    if (stunNormalizedTimer > 0f)
                    {
                        agent.speed = 0f;
                    }
                    else
                    {
                        agent.speed = 6f;
                    }
                    waitingAtNest = false;
                    break;
                case 1:
                    ExitChaseMode();
                    addPlayerVelocityToDestination = 0f;
                    if (stunNormalizedTimer > 0f)
                    {
                        agent.speed = 0f;
                    }
                    else
                    {
                        agent.speed = 6f;
                    }
                    agent.acceleration = 30f;
                    if (waitingAtNest && waitingAtNestTimer > 0f)
                    {
                        waitingAtNestTimer -= Time.deltaTime;
                    }
                    break;
                case 2:
                    if (!inChase)
                    {
                        inChase = true;
                        creatureSFX.clip = bugFlySFX;
                        creatureSFX.Play();
                        RoundManager.PlayRandomClip(creatureVoice, angryScreechSFX);
                        creatureAnimator.SetBool("Chase", value: true);
                        waitingAtNest = false;
                        if (IsOwner)
                        {
                            // Only start the bomb once when chase starts
                            doBlowupServerRpc();
                        }
                        if (Vector3.SqrMagnitude(transform.position - GameNetworkManager.Instance.localPlayerController.transform.position) < 100f)
                        {
                            GameNetworkManager.Instance.localPlayerController.JumpToFearLevel(0.5f);
                        }
                    }
                    addPlayerVelocityToDestination = 2f;
                    if (!IsOwner)
                    {
                        break;
                    }
                    if (!IsHoarderBugAngry())
                    {
                        HoarderBugItem? hoarderBugItem = CheckLineOfSightForItem(HoarderBugItemStatus.Returned, 60f, 12, 3f);
                        if (hoarderBugItem != null && !hoarderBugItem.itemGrabbableObject.isHeld)
                        {
                            SetGoTowardsTargetObject(hoarderBugItem.itemGrabbableObject.gameObject);
                        }
                        else
                        {
                        }
                        break;
                    }
                    if (stunNormalizedTimer > 0f)
                    {
                        agent.speed = 0f;
                    }
                    else
                    {
                        agent.speed = 18f;
                    }
                    agent.acceleration = 16f;
                    if (GameNetworkManager.Instance.localPlayerController.HasLineOfSightToPosition(transform.position + Vector3.up * 0.75f, 60f, 15))
                    {
                        GameNetworkManager.Instance.localPlayerController.IncreaseFearLevelOverTime(0.4f);
                    }
                    break;
            }
        }

        public override void DetectNoise(Vector3 noisePosition, float noiseLoudness, int timesPlayedInOneSpot = 0, int noiseID = 0)
        {
            base.DetectNoise(noisePosition, noiseLoudness, timesPlayedInOneSpot, noiseID);
            if (timesPlayedInOneSpot <= 10 && !(timeSinceLookingTowardsNoise < 0.6f))
            {
                timeSinceLookingTowardsNoise = 0f;
                float num = Vector3.SqrMagnitude(noisePosition - nestPosition);
                if (IsOwner && HoarderBugItems.Count > 0 && !isAngry && currentBehaviourStateIndex == 0 && num < 225f && (targetItem == null || Vector3.SqrMagnitude(targetItem.transform.position - transform.position) > 20.25f))
                {
                    SwitchToBehaviourState(1);
                }
                positionOfInterest = noisePosition;
                lookingAtPositionOfInterest = true;
            }
        }

        private void DropItemAndCallDropRPC(NetworkObject dropItemNetworkObject, bool droppedInNest = true)
        {
            Vector3 targetFloorPosition = RoundManager.Instance.RandomlyOffsetPosition(heldItem.itemGrabbableObject.GetItemFloorPosition(), 1.2f, 0.4f);
            DropItem(dropItemNetworkObject, targetFloorPosition);
            sendingGrabOrDropRPC = true;
            DropItemServerRpc(dropItemNetworkObject, targetFloorPosition, droppedInNest);
        }

        [Rpc(SendTo.Server)]
        public void DropItemServerRpc(NetworkObjectReference objectRef, Vector3 targetFloorPosition, bool droppedInNest)
        {
            DropItemClientRpc(objectRef, targetFloorPosition, droppedInNest);
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void DropItemClientRpc(NetworkObjectReference objectRef, Vector3 targetFloorPosition, bool droppedInNest)
        {
            if (objectRef.TryGet(out var networkObject))
            {
                try
                {
                    DropItem(networkObject, targetFloorPosition, droppedInNest);
                }
                catch
                {
                }
            }
        }

        [Rpc(SendTo.Server)]
        public void GrabItemServerRpc(NetworkObjectReference objectRef)
        {
            GrabItemClientRpc(objectRef);
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void GrabItemClientRpc(NetworkObjectReference objectRef)
        {
            SwitchToBehaviourStateOnLocalClient(1);
            if (objectRef.TryGet(out var networkObject))
            {
                GrabItem(networkObject);
            }
        }

        private void DropItem(NetworkObject item, Vector3 targetFloorPosition, bool droppingInNest = true)
        {
            if (sendingGrabOrDropRPC)
            {
                sendingGrabOrDropRPC = false;
                return;
            }
            if (heldItem == null)
            {
                return;
            }
            GrabbableObject itemGrabbableObject = heldItem.itemGrabbableObject;
            itemGrabbableObject.parentObject = null;
            itemGrabbableObject.transform.SetParent(StartOfRound.Instance.propsContainer, worldPositionStays: true);
            itemGrabbableObject.EnablePhysics(enable: true);
            itemGrabbableObject.fallTime = 0f;
            itemGrabbableObject.startFallingPosition = itemGrabbableObject.transform.parent.InverseTransformPoint(itemGrabbableObject.transform.position);
            itemGrabbableObject.targetFloorPosition = itemGrabbableObject.transform.parent.InverseTransformPoint(targetFloorPosition);
            itemGrabbableObject.floorYRot = -1;
            itemGrabbableObject.DiscardItemFromEnemy();
            heldItem = null!;
            if (!droppingInNest && !grabbableObjectsInMap.Contains(itemGrabbableObject.gameObject))
            {
                grabbableObjectsInMap.Add(itemGrabbableObject.gameObject);
            }
        }

        private void GrabItem(NetworkObject item)
        {
            if (sendingGrabOrDropRPC)
            {
                sendingGrabOrDropRPC = false;
                return;
            }
            if (heldItem != null)
            {
                DropItem(heldItem.itemGrabbableObject.GetComponent<NetworkObject>(), heldItem.itemGrabbableObject.GetItemFloorPosition());
            }
            targetItem = null!;
            targetDestinationObject = null!;
            GrabbableObject component = item.gameObject.GetComponent<GrabbableObject>();
            HoarderBugItems.Add(new HoarderBugItem(component, HoarderBugItemStatus.Owned, nestPosition));
            heldItem = HoarderBugItems[HoarderBugItems.Count - 1];
            component.parentObject = grabTarget;
            component.hasHitGround = false;
            component.GrabItemFromEnemy(this);
            component.EnablePhysics(enable: false);
            grabbableObjectsInMap.Remove(component.gameObject);
        }

        public override void OnCollideWithPlayer(Collider other)
        {
            base.OnCollideWithPlayer(other);
            if (!inChase)
            {
                return;
            }
            if (!(timeSinceHittingPlayer < 0.5f))
            {
                PlayerControllerB playerControllerB = MeetsStandardPlayerCollisionConditions(other);
                if (playerControllerB != null)
                {
                    timeSinceHittingPlayer = 0f;
                    playerControllerB.DamagePlayer(30, hasDamageSFX: true, callRPC: true, CauseOfDeath.Mauling);
                    HitPlayerServerRpc();
                }
            }
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        public void HitPlayerServerRpc()
        {
            HitPlayerClientRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void HitPlayerClientRpc()
        {
            if (!isEnemyDead)
            {
                creatureAnimator.SetTrigger("HitPlayer");
                creatureSFX.PlayOneShot(hitPlayerSFX);
                WalkieTalkie.TransmitOneShotAudio(creatureSFX, hitPlayerSFX);
            }
        }

        public override void HitEnemy(int force = 1, PlayerControllerB? playerWhoHit = null, bool playHitSFX = false, int hitID = -1)
        {
            base.HitEnemy(force, playerWhoHit, playHitSFX, hitID);
            if (!isEnemyDead)
            {
                creatureAnimator.SetTrigger("damage");

                if (playerWhoHit != null)
                    angryAtPlayer = playerWhoHit;

                angryTimer += 18f;
                enemyHP -= force;
                if (enemyHP <= 0 && IsOwner)
                {
                    KillEnemyOnOwnerClient();
                }
            }
        }

        public override void KillEnemy(bool destroy = false)
        {
            StopActiveSearches();
            base.KillEnemy();
            DisableBlowupServerRpc();
            agent.speed = 0f;
            creatureVoice.Stop();
            creatureSFX.Stop();
        }

        private void StopActiveSearches()
        {
            if (searchForItems.inProgress)
            {
                StopSearch(searchForItems);
            }
            if (searchForPlayer.inProgress)
            {
                StopSearch(searchForPlayer);
            }
        }

        public HoarderBugItem? CheckLineOfSightForItem(HoarderBugItemStatus searchForItemsOfStatus = HoarderBugItemStatus.Any, float width = 45f, int range = 60, float proximityAwareness = -1f)
        {
            for (int i = 0; i < HoarderBugItems.Count; i++)
            {
                if (HoarderBugItems[i] == null || HoarderBugItems[i].itemGrabbableObject == null)
                {
                    HoarderBugItems.RemoveAt(i);
                    i--;
                    continue;
                }
                GrabbableObject itemGrabbableObject = HoarderBugItems[i].itemGrabbableObject;
                if (!itemGrabbableObject.grabbableToEnemies || itemGrabbableObject.deactivated || itemGrabbableObject.isHeld || searchForItemsOfStatus != HoarderBugItemStatus.Any && HoarderBugItems[i].status != searchForItemsOfStatus)
                {
                    continue;
                }
                Vector3 position = itemGrabbableObject.transform.position;
                if (!Physics.Linecast(eye.position, position, StartOfRound.Instance.collidersAndRoomMaskAndDefault))
                {
                    Vector3 to = position - eye.position;
                    if (Vector3.Angle(eye.forward, to) < width || proximityAwareness != -1f && Vector3.SqrMagnitude(transform.position - position) < proximityAwareness * proximityAwareness)
                    {
                        return HoarderBugItems[i];
                    }
                }
            }
            return null;
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        public void doBlowupServerRpc()
        {
            if (RoundManager.Instance.IsHost) doBlowupClientRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void doBlowupClientRpc()
        {
            if (onBlowUpSequence) return;
            onBlowUpSequence = true;

            tickingAudio.Play();

            // Slow
            StartBlowupCoroutine(FlicketLights(0.2f, 0.15f));
            StartBlowupCoroutine(FlicketLights(1.2f, 0.15f));
            StartBlowupCoroutine(FlicketLights(2.2f, 0.15f));
            StartBlowupCoroutine(FlicketLights(3.2f, 0.15f));
            StartBlowupCoroutine(FlicketLights(4.2f, 0.15f));

            // Fast
            StartBlowupCoroutine(FlicketLights(4.36f, 0.5f));
            StartBlowupCoroutine(FlicketLights(4.46f, 0.5f));
            StartBlowupCoroutine(FlicketLights(4.59f, 0.5f));
            StartBlowupCoroutine(FlicketLights(4.71f, 0.5f));
            StartBlowupCoroutine(FlicketLights(4.84f, 0.5f));
            StartBlowupCoroutine(FlicketLights(4.96f, 0.5f));
            StartBlowupCoroutine(FlicketLights(5.09f, 0.5f));
            StartBlowupCoroutine(FlicketLights(5.21f, 0.5f));
            StartBlowupCoroutine(FlicketLights(5.34f, 0.5f));
            StartBlowupCoroutine(FlicketLights(5.45f, 0.5f));
            StartBlowupCoroutine(FlicketLights(5.56f, 0.5f));
            StartBlowupCoroutine(FlicketLights(5.70f, 0.5f));
            StartBlowupCoroutine(FlicketLights(5.81f, 0.5f));

            // Blowup
            StartBlowupCoroutine(BlowUpAt(6.0f));

            IEnumerator BlowUpAt(float timeStamp)
            {
                yield return new WaitForSeconds(timeStamp);
                bugLight.gameObject.SetActive(false);

                if (!isEnemyDead) Landmine.SpawnExplosion(mainTransform.position + Vector3.up, spawnExplosionEffect: true, 3.0f, 6.0f);
            }
        }

        // Track these so death cleanup doesnt kill AI search coroutines
        private void StartBlowupCoroutine(IEnumerator routine)
        {
            blowupCoroutines.Add(StartCoroutine(routine));
        }

        public IEnumerator FlicketLights(float timeStamp, float flickerLightTime)
        {
            yield return new WaitForSeconds(timeStamp);
            bugLight.gameObject.SetActive(true);

            yield return new WaitForSeconds(flickerLightTime);
            bugLight.gameObject.SetActive(false);
        }

        [Rpc(SendTo.Server, RequireOwnership = false)]
        public void DisableBlowupServerRpc()
        {
            DisableBlowupClientRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void DisableBlowupClientRpc()
        {
            bugLight.gameObject.SetActive(false);
            tickingAudio.Stop();
            for (int i = 0; i < blowupCoroutines.Count; i++)
            {
                if (blowupCoroutines[i] != null)
                {
                    StopCoroutine(blowupCoroutines[i]);
                }
            }
            blowupCoroutines.Clear();
        }
    }
}
