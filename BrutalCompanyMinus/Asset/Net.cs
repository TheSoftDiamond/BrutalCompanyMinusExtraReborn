using BrutalCompanyMinus.Minus;
using BrutalCompanyMinus.Minus.Events;
using BrutalCompanyMinus.Minus.Handlers;
using BrutalCompanyMinus.Minus.Handlers.Modded;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.HighDefinition;
using AllWeather = BrutalCompanyMinus.Minus.Handlers.AllWeather;

namespace BrutalCompanyMinus
{
    [HarmonyPatch]
    public class Net : NetworkBehaviour
    {
        public static Net Instance { get; private set; }
        public static GameObject netObject { get; private set; }

        public NetworkList<Weather> currentWeatherMultipliers;
        public NetworkList<OutsideObjectsToSpawnMethod> outsideObjectsToSpawn;
        public NetworkList<CurrentWeatherEffect> currentWeatherEffects;

        public NetworkVariable<FixedString4096Bytes> textUI = new NetworkVariable<FixedString4096Bytes>();

        public bool receivedSyncedValues = false;

        public List<GameObject> objectsToSpawn = new List<GameObject>();
        public List<int> objectsToSpawnAmount = new List<int>();
        public List<float> objectsToSpawnRadius = new List<float>();
        public List<Vector3> objectsToSpawnOffsets = new List<Vector3>();

        private float currentIntervalTime = 0.0f;

        public NetworkVariable<bool> LeverUnityNet = new NetworkVariable<bool>(false);
        public NetworkVariable<bool> WalkiesUnityNet = new NetworkVariable<bool>(false);
        public NetworkVariable<bool> JetpackUnityNet = new NetworkVariable<bool>(false);
        public NetworkVariable<bool> TerminalUnityNet = new NetworkVariable<bool>(false);
        public NetworkVariable<bool> TargetingUnityNet = new NetworkVariable<bool>(false);
        public NetworkVariable<bool> TeleporterUnityNet = new NetworkVariable<bool>(false);
        public NetworkVariable<bool> ItemChargerUnityNet = new NetworkVariable<bool>(false);
        public NetworkVariable<bool> CircuitUnityNet = new NetworkVariable<bool>(false);
        public NetworkVariable<bool> DoorOvUnityNet = new NetworkVariable<bool>(false);
        public NetworkVariable<bool> DoorUnityNet = new NetworkVariable<bool>(false);
        public NetworkVariable<bool> CameraUnityNet = new NetworkVariable<bool>(false);

#pragma warning disable IDE0051 // Remove unused private members
        private void Awake()
#pragma warning restore IDE0051 // Remove unused private members
        {
            // Initalize or it will break
            currentWeatherMultipliers = new NetworkList<Weather>();
            outsideObjectsToSpawn = new NetworkList<OutsideObjectsToSpawnMethod>();
            currentWeatherEffects = new NetworkList<CurrentWeatherEffect>();

            Log.LogWarning("Net object Awake called");
        }

#pragma warning disable IDE0051 // Remove unused private members
        private void Update()
#pragma warning restore IDE0051 // Remove unused private members
        {
            if (currentIntervalTime > 0.0f)
            {
                currentIntervalTime -= Time.deltaTime;
            }
            else
            {
                currentIntervalTime = 0.5f;
                if (currentWeatherEffects.Count > 0) // Set atmosphere
                {
                    foreach (CurrentWeatherEffect e in currentWeatherEffects)
                    {
                        PlayerControllerB localPlayer = GameNetworkManager.Instance.localPlayerController;
                        if (localPlayer == null) continue;

                        if (localPlayer.isPlayerDead)
                        {
                            UpdateAtmosphere(e.name, false);
                        }
                        else if (!localPlayer.isInsideFactory)
                        {
                            UpdateAtmosphere(e.name, e.state);
                        }
                        else
                        {
                            UpdateAtmosphere(e.name, false);
                        }
                    }
                }
            }
            if (objectsToSpawn.Count > 0)
            {
                Manager.Spawn.DoSpawnOutsideObjects(objectsToSpawnAmount[0], objectsToSpawnRadius[0], objectsToSpawnOffsets[0], objectsToSpawn[0]);
                objectsToSpawn.RemoveAt(0);
                objectsToSpawnAmount.RemoveAt(0);
                objectsToSpawnRadius.RemoveAt(0);
                objectsToSpawnOffsets.RemoveAt(0);
            }
        }

        public override void OnNetworkSpawn()
        {
            Instance = this;

            UI.SpawnObject(); // Spawn client side UI object

            if (IsServer) // Only call on server
            {
                InitalizeCurrentWeatherMultipliersServerRpc();
            }

            base.OnNetworkSpawn();

        }

        public override void OnNetworkDespawn()
        {
            UI.Instance.UnsubscribeFromKeyboardEvent();
            Destroy(GameObject.Find("EventGUI"));

            base.OnNetworkDespawn();
        }

        [ServerRpc(RequireOwnership = false)]
        public void GetSyncedTextServerRpc()
        {
            GetSyncedTextClientRpc(textUI.Value);
        }

        [ClientRpc]
        public void GetSyncedTextClientRpc(FixedString4096Bytes textUI)

        {
            Manager.textUI = textUI.Value;
        }

        [ServerRpc(RequireOwnership = false)]
        public void GiveSeedServerRpc()
        {
            GiveSeedClientRpc(_seed++);
        }

        [ClientRpc]
        public void GiveSeedClientRpc(int seed)
        {
            Manager.seed = seed;
        }

        [ServerRpc(RequireOwnership = false)]
        public void ClearGameObjectsServerRpc()
        {
            ClearGameObjectsClientRpc();
        }

        [ClientRpc]
        public void ClearGameObjectsClientRpc()
        {
            for (int i = 0; i < Manager.objectsToClear.Count; i++)
            {
                var obj = Manager.objectsToClear[i];
                if (obj == null)
                    continue;

                var netObject = obj.GetComponent<NetworkObject>();
                string objectName = obj.name;
                try
                {
                    if (netObject != null)
                    {
                        netObject.Despawn(true);
                        if (Configuration.ExtraLogging.Value)
                            Log.LogInfo($"Despawned network object: {objectName}");
                    }
                    else
                    {
                        GameObject.Destroy(obj);
                        if (Configuration.ExtraLogging.Value)
                            Log.LogInfo($"Destroyed non-network object: {objectName}");
                    }
                }
                catch (KeyNotFoundException ex)
                {
                    Log.LogError($"KeyNotFoundException while despawning/destroying '{objectName}': {ex.Message}");
                }
                catch (Exception ex)
                {
                    Log.LogError($"Unexpected exception while despawning/destroying '{objectName}': {ex.Message}");
                }
            }
            Manager.objectsToClear.Clear();
        }

        [ClientRpc]
        public void SyncValuesClientRpc(float factorySizeMultiplier, float scrapValueMultiplier, float scrapAmountMultiplier, int bonusMaxHp)
        {
            RoundManager.Instance.currentLevel.factorySizeMultiplier = factorySizeMultiplier;
            Manager.bonusEnemyHp = bonusMaxHp;
            receivedSyncedValues = true;
        }

        [ServerRpc]
        public void SyncScrapValueServerRpc(NetworkObjectReference obj, int value)
        {
            SyncScrapValueClientRpc(obj, value);
        }

        [ClientRpc]
        private void SyncScrapValueClientRpc(NetworkObjectReference obj, int value)
        {
            obj.TryGet(out NetworkObject netObj);
            netObj.GetComponent<GrabbableObject>().SetScrapValue(value);
        }

        [ServerRpc(RequireOwnership = false)]
        public void GenerateAndSyncTerminalCodeServerRpc(NetworkObjectReference netObject, int code) => GenerateAndSyncTerminalCodeClientRpc(netObject, code);

        [ClientRpc]
        public void GenerateAndSyncTerminalCodeClientRpc(NetworkObjectReference netObject, int code)
        {
            NetworkObject netObj = null;
            if (!netObject.TryGet(out netObj))
            {
                Log.LogError("Network Object is null in GenerateAndSyncTerminalCodeClientRpc()");
                return;
            }

            TerminalAccessibleObject terminalAccessibleObject = netObj.GetComponentInChildren<TerminalAccessibleObject>();
            if (terminalAccessibleObject == null)
            {
                Log.LogError("Terminal Accessible Object is null in GenerateAndSyncTerminalCodeClientRpc()");
                return;
            }

            terminalAccessibleObject.InitializeValues();
            terminalAccessibleObject.SetCodeTo(code);
        }

        [ServerRpc(RequireOwnership = false)]
        public void MoveTimeServerRpc(float amount, float speedMultiplier = 1.0f) => MoveTimeClientRpc(amount, speedMultiplier);

        [ClientRpc]
        public void MoveTimeClientRpc(float amount, float speedMultiplier)
        {
            Manager.moveTime = true;
            Manager.moveTimeAmount += amount;
            Manager.timeSpeedMultiplier *= speedMultiplier;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetTimeMultiplierServerRpc(float speedMultiplier = 1.0f) => SetTimeMultiplierClientRpc(speedMultiplier);

        [ClientRpc]
        public void SetTimeMultiplierClientRpc(float speedMultiplier)
        {
            Manager.moveTime = true;
            Manager.timeSpeedMultiplier = speedMultiplier;
        }

        private void UpdateAtmosphere(FixedString128Bytes name, bool state)
        {
            for (int i = 0; i < TimeOfDay.Instance.effects.Length; i++)
            {
                if (TimeOfDay.Instance.effects[i].name == name)
                {
                    TimeOfDay.Instance.effects[i].effectEnabled = state;
                }
            }
        }

        [ClientRpc]
        public void ShowCaseEventsClientRpc()
        {
            // Showcase Events
            UI.Instance.curretShowCaseEventTime = UI.Instance.showCaseEventTime;
            UI.Instance.TogglePanel(true);
            UI.Instance.panelScrollBar.value = 1.0f; // Start from top
            UI.Instance.showCaseEvents = true;
        }

        [ServerRpc(RequireOwnership = false)]
        private void InitalizeCurrentWeatherMultipliersServerRpc()
        {
            currentWeatherMultipliers = Weather.InitalizeWeatherMultipliers(currentWeatherMultipliers);
            UpdateCurrentWeatherMultipliersServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void UpdateCurrentWeatherMultipliersServerRpc()
        {
            currentWeatherMultipliers = Weather.RandomizeWeatherMultipliers(currentWeatherMultipliers);
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetRecievedServerRpc(bool state) => SetRecievedClientRpc(state);

        [ClientRpc]
        public void SetRecievedClientRpc(bool state) => receivedSyncedValues = state;

        [ServerRpc(RequireOwnership = false)]
        public void SetRealityShiftActiveServerRpc(bool state) => SetRealityShiftActiveClientRpc(state);

        [ClientRpc]
        public void SetRealityShiftActiveClientRpc(bool state) => Minus.Events.RealityShift.Active = state;

        [ServerRpc(RequireOwnership = false)]
        public void SetAllWeatherActiveServerRpc(bool state) => SetAllWeatherActiveClientRpc(state);

        [ClientRpc]
        public void SetAllWeatherActiveClientRpc(bool state) => Minus.Events.AllWeather.Active = state;

        [ServerRpc(RequireOwnership = false)]
        public void SetEventActiveServerRPC(string eventName, bool state) => SetEventActiveClientRPC(eventName, state);

        [ClientRpc]
        public void SetEventActiveClientRPC(string eventName, bool state)
        {
            MEvent mEvent = MEvent.GetEvent(eventName);
            if (mEvent != null)
            {
                mEvent.Active = state;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void MessWithLightsServerRpc() => MessWithLightsClientRpc();

        [ClientRpc]
        public void MessWithLightsClientRpc() => RoundManager.Instance.FlickerLights(true, true);

        [ServerRpc(RequireOwnership = false)]
        public void MessWithBreakerServerRpc(bool state) => MessWithBreakerClientRpc(state);

        [ClientRpc]
        public void MessWithBreakerClientRpc(bool state)
        {
            BreakerBox breakerBox = GameObject.FindObjectOfType<BreakerBox>();
            if (breakerBox != null)
            {
                breakerBox.SetSwitchesOff();
                RoundManager.Instance.TurnOnAllLights(state);
            }
        }

        public int _seed = 49;
        [ServerRpc(RequireOwnership = false)]
        public void MessWithDoorsServerRpc(float openCloseChance, bool messWithLock = false, float messWithLockChance = 0.0f)
        {
            if (_seed == 0) _seed = StartOfRound.Instance.randomMapSeed;
            _seed++;
            MessWithDoorsClientRpc(openCloseChance, _seed, messWithLock, messWithLockChance);
        }

        [ClientRpc]
        public void MessWithDoorsClientRpc(float openCloseChance, int seed, bool messWithLock, float messWithLockChance)
        {
            DoorLock[] doors = GameObject.FindObjectsOfType<DoorLock>();
            System.Random rng = new System.Random(seed);
            foreach (DoorLock door in doors)
            {
                if (door == null) continue;
                if (rng.NextDouble() <= openCloseChance) continue;

                if (messWithLock && rng.NextDouble() <= messWithLockChance)
                {
                    if (rng.Next(0, 2) == 0)
                    {
                        door.LockDoor();
                    }
                    else
                    {
                        door.UnlockDoor();
                    }
                    return;
                }

                if (!door.isLocked)
                {
                    door.gameObject.GetComponent<AnimatedObjectTrigger>().TriggerAnimationNonPlayer(false, true);
                    door.SetDoorAsOpen(Convert.ToBoolean(rng.Next(0, 2)));
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void UnlockAndOpenAllDoorsServerRpc()
        {
            TerminalAccessibleObject[] doors = GameObject.FindObjectsOfType<TerminalAccessibleObject>();

            foreach (TerminalAccessibleObject door in doors) door.SetDoorOpenServerRpc(true);

            UnlockAndOpenAllDoorsClientRpc();
        }

        [ClientRpc]
        public void UnlockAndOpenAllDoorsClientRpc()
        {
            DoorLock[] doors = GameObject.FindObjectsOfType<DoorLock>();

            foreach (DoorLock door in doors)
            {
                if (door == null) continue;

                if (door.isLocked) door.UnlockDoor();

                door.gameObject.GetComponent<AnimatedObjectTrigger>().TriggerAnimationNonPlayer(false, true);
                door.SetDoorAsOpen(true);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnMudPilesOutsideServerRpc(int amount)
        {
            NavMeshHit hit = default(NavMeshHit);
            System.Random rng = new System.Random(_seed++);
            for (int i = 0; i < amount; i++)
            {
                Vector3 node = RoundManager.Instance.outsideAINodes[rng.Next(0, RoundManager.Instance.outsideAINodes.Length)].transform.position;
                Vector3 nodeMoved = RoundManager.Instance.GetRandomNavMeshPositionInBoxPredictable(node, 30f, hit, rng) + Vector3.up;

                SpawnMudPilesOutsideClientRpc(nodeMoved, _seed++);
            }
        }

        [ClientRpc]
        public void SpawnMudPilesOutsideClientRpc(Vector3 position, int seed) => GameObject.Instantiate(RoundManager.Instance.quicksandPrefab, position, Quaternion.identity, RoundManager.Instance.mapPropsContainer.transform);

        [ServerRpc(RequireOwnership = false)]
        public void TeleportEnemyServerRpc(NetworkObjectReference enemy, Vector3 position) => TeleportEnemyClientRpc(enemy, position);

        [ClientRpc]
        private void TeleportEnemyClientRpc(NetworkObjectReference enemy, Vector3 position)
        {
            if (!enemy.TryGet(out NetworkObject enemyAI)) return;

            GameObject.Instantiate(Assets.teleportAudio, enemyAI.transform.position, Quaternion.identity);
            GameObject.Instantiate(Assets.teleportAudio, position, Quaternion.identity);

            enemyAI.transform.position = position;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnAllWeatherServerRpc(int seed) => SpawnAllWeatherClientRpc(seed);

        [ClientRpc]
        public void SpawnAllWeatherClientRpc(int seed) // Messy as fuck
        {
            if (RoundManager.Instance.currentLevel.randomWeathers == null) return;

            System.Random rng = new System.Random();

            AllWeather.raining = false;
            foreach (RandomWeatherWithVariables randomWeather in RoundManager.Instance.currentLevel.randomWeathers)
            {
                if (randomWeather.weatherType == RoundManager.Instance.currentLevel.currentWeather) continue;
                switch (randomWeather.weatherType)
                {
                    case LevelWeatherType.Rainy:
                        AllWeather.raining = true;
                        break;
                    case LevelWeatherType.Foggy:
                        if (TimeOfDay.Instance.foggyWeather == null) break;
                        LocalVolumetricFog fog = GameObject.Instantiate(TimeOfDay.Instance.foggyWeather);
                        Manager.objectsToClear.Add(fog.gameObject);

                        fog.parameters.albedo = new Color(0.25f, 0.35f, 0.55f, 1f);
                        fog.parameters.meanFreePath = rng.Next((int)MathF.Max(4.0f, randomWeather.weatherVariable), randomWeather.weatherVariable2) * 5;
                        fog.parameters.size.y = 255f;

                        fog.gameObject.SetActive(true);
                        break;
                    case LevelWeatherType.Flooded:
                        FloodWeather flooded = GameObject.Instantiate(Assets.flooded);
                        Manager.objectsToClear.Add(flooded.gameObject);

                        AllWeather.floodVariable1 = randomWeather.weatherVariable;
                        AllWeather.floodVariable2 = randomWeather.weatherVariable2;

                        AllWeather.spawnedFloodedWeather = flooded;
                        flooded.gameObject.SetActive(true);
                        break;
                    case LevelWeatherType.Stormy:
                        StormyWeather stormy = GameObject.Instantiate(Assets.stormy);
                        Manager.objectsToClear.Add(stormy.gameObject);

                        AllWeather.lightningVariable1 = randomWeather.weatherVariable;
                        AllWeather.LightningVariable2 = randomWeather.weatherVariable2;

                        stormy.gameObject.SetActive(true);
                        break;
                    case LevelWeatherType.Eclipsed:
                        Manager.minEnemiesToSpawnInside += randomWeather.weatherVariable;
                        Manager.minEnemiestoSpawnOutside += randomWeather.weatherVariable2;
                        break;
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ShiftServerRpc(NetworkObjectReference networkObject)
        {
            if (Minus.Handlers.RealityShift.shiftList.Count == 0) return;
            if (Minus.Handlers.RealityShift.shiftList[0] == null) return;

            NetworkObject netObj = null;
            networkObject.TryGet(out netObj);
            if (netObj == null)
            {
                Log.LogError("NetworkObject in ShiftServerRpc() is null.");
                return;
            }
            GrabbableObject instance = netObj.GetComponent<GrabbableObject>();

            GameObject scrap = GameObject.Instantiate(Minus.Handlers.RealityShift.shiftList[0], instance.transform.position, Quaternion.identity);
            GrabbableObject grabbableObject = scrap.GetComponent<GrabbableObject>();
            if (grabbableObject == null)
            {
                Log.LogError("GrabbableObject is null in ShiftServerRpc()");
                return;
            }

            grabbableObject.targetFloorPosition = grabbableObject.GetItemFloorPosition(instance.transform.position);
            grabbableObject.SetScrapValue(Minus.Handlers.RealityShift.shiftListValues[0]);
            grabbableObject.NetworkObject.Spawn();
            SyncScrapValueClientRpc(grabbableObject.NetworkObject, Minus.Handlers.RealityShift.shiftListValues[0]);

            AddObjectToGrabToListClientRpc(grabbableObject.NetworkObject);

            NetworkObject oldNetObject = instance.GetComponent<NetworkObject>();
            if (oldNetObject != null)
            {
                oldNetObject.Despawn(true);
            }
            else
            {
                Log.LogError("NetworkObject is null in ShiftServerRpc(), destroying on client instead.");
                GameObject.Destroy(instance);
            }

            if (scrap != null)
            {
                Minus.Handlers.RealityShift.shiftListValues.RemoveAt(0);
                Minus.Handlers.RealityShift.shiftList.RemoveAt(0);
            }
        }

        [ClientRpc]
        public void AddObjectToGrabToListClientRpc(NetworkObjectReference obj) => Minus.Handlers.RealityShift.shiftedObjects.Add(obj);

        [ServerRpc(RequireOwnership = false)]
        public void GenerateShiftableObjectsListServerRpc(NetworkObjectReference[] spawnedScrap) => GenerateShiftableObjectsListClientRpc(spawnedScrap);

        [ClientRpc]
        public void GenerateShiftableObjectsListClientRpc(NetworkObjectReference[] spawnedScrap)
        {
            Minus.Handlers.RealityShift.ShiftableObjects.Clear();
            foreach (NetworkObjectReference netRef in spawnedScrap)
            {
                NetworkObject netObj = null;
                netRef.TryGet(out netObj);

                if (netObj != null)
                {
                    Minus.Handlers.RealityShift.ShiftableObjects.Add(netObj.gameObject.GetInstanceID());
                }
                else
                {
                    Log.LogError("Scrap spawn has null NetworkObject");
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void FireAtServerRpc(Vector3 at, Vector3 from) => FireAtClientRpc(at, from);

        [ClientRpc]
        public void FireAtClientRpc(Vector3 at, Vector3 from)
        {
            GameObject artilleryShell = GameObject.Instantiate(Assets.artilleryShell, from, Quaternion.identity);
            ArtilleryShell script = artilleryShell.GetComponent<ArtilleryShell>();
            script.target = at;

            Manager.objectsToClear.Add(artilleryShell);
        }

        [ServerRpc(RequireOwnership = false)]
        public void DisplayTipServerRpc(string headerText, string bodyText, bool isWarning = false) => DisplayTipClientRpc(headerText, bodyText, isWarning);

        [ClientRpc]
        public void DisplayTipClientRpc(string headerText, string bodyText, bool isWarning = false) => HUDManager.Instance.DisplayTip(headerText, bodyText, isWarning);

        [ServerRpc(RequireOwnership = false)]
        public void BlackFridayServerRpc(int minPercentageCut, int maxPercentageCut)
        {
            BlackFridayClientRpc(minPercentageCut, maxPercentageCut, _seed++);
        }

        [ClientRpc]
        public void BlackFridayClientRpc(int minPercentageCut, int maxPercentageCut, int seed)
        {
            System.Random rng = new System.Random(seed);
            for (int i = 0; i < Manager.currentTerminal.buyableItemsList.Length; i++)
            {
                int percentage = 100 - (int)Mathf.Clamp(rng.Next(minPercentageCut, maxPercentageCut), 0.0f, 90.0f);
                Manager.currentTerminal.itemSalesPercentages[i] = (int)Math.Round((double)percentage / 10) * 10;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetPhonesOutServerRpc(bool active)
        {
            SetPhonesOutClientRpc(active);
        }

        [ClientRpc]
        public void SetPhonesOutClientRpc(bool active)
        {
            PhonesOut.Active = active;
        }


        [ServerRpc(RequireOwnership = false)]
        public void SetFlashlightsServerRpc(bool active)
        {
            SetFlashlightsClientRpc(active);
        }

        [ClientRpc]
        public void SetFlashlightsClientRpc(bool active)
        {
            FlashLightsFailure.Active = active;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetEntranceServerRpc(bool active)
        {
            SetEntranceClientRpc(active);
        }

        [ClientRpc]
        public void SetEntranceClientRpc(bool active)
        {
            LockedEntrance.Active = active;
        }

        [ServerRpc(RequireOwnership = false)]
        public void LightsOffServerRpc()
        {
            LightsOffClientRpc();
        }

        [ClientRpc]
        public void LightsOffClientRpc()
        {
            GameObject hangarShip = Assets.hangarShip;
            if (hangarShip == null)
            {
                return;
            }

            // Find a gameobject called ShipElectricLights, which is a child of hangarShip
            GameObject shipElectricLights = hangarShip.transform.Find("ShipElectricLights").gameObject;
            if (shipElectricLights == null)
            {
                return;
            }

            GameObject areaLight3 = shipElectricLights.transform.Find("Area Light (3)")?.gameObject;
            if (areaLight3 != null)
            {
                areaLight3.SetActive(false);
            }

            GameObject areaLight4 = shipElectricLights.transform.Find("Area Light (4)")?.gameObject;
            if (areaLight4 != null)
            {
                areaLight4.SetActive(false);
            }

            GameObject areaLight5 = shipElectricLights.transform.Find("Area Light (5)")?.gameObject;
            if (areaLight5 != null)
            {
                areaLight5.SetActive(false);
            }

            GameObject areaLight7 = shipElectricLights.transform.Find("Area Light (7)")?.gameObject;
            if (areaLight7 != null)
            {
                areaLight7.SetActive(false);
            }

            GameObject areaLight8 = shipElectricLights.transform.Find("Area Light (8)")?.gameObject;
            if (areaLight8 != null)
            {
                areaLight8.SetActive(false);
            }

            GameObject areaLight9 = shipElectricLights.transform.Find("Area Light (9)")?.gameObject;
            if (areaLight9 != null)
            {
                areaLight9.SetActive(false);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void LightsOnServerRpc()
        {
            LightsOnClientRpc();
        }

        [ClientRpc]
        public void LightsOnClientRpc()
        {
            GameObject hangarShip = Assets.hangarShip;
            if (hangarShip == null)
            {
                return;
            }

            // Find a gameobject called ShipElectricLights, which is a child of hangarShip
            GameObject shipElectricLights = hangarShip.transform.Find("ShipElectricLights").gameObject;
            if (shipElectricLights == null)
            {
                return;
            }

            GameObject areaLight3 = shipElectricLights.transform.Find("Area Light (3)")?.gameObject;
            if (areaLight3 != null)
            {
                areaLight3.SetActive(true);
            }

            GameObject areaLight4 = shipElectricLights.transform.Find("Area Light (4)")?.gameObject;
            if (areaLight4 != null)
            {
                areaLight4.SetActive(true);
            }

            GameObject areaLight5 = shipElectricLights.transform.Find("Area Light (5)")?.gameObject;
            if (areaLight5 != null)
            {
                areaLight5.SetActive(true);
            }

            GameObject areaLight7 = shipElectricLights.transform.Find("Area Light (7)")?.gameObject;
            if (areaLight7 != null)
            {
                areaLight7.SetActive(true);
            }

            GameObject areaLight8 = shipElectricLights.transform.Find("Area Light (8)")?.gameObject;
            if (areaLight8 != null)
            {
                areaLight8.SetActive(true);
            }

            GameObject areaLight9 = shipElectricLights.transform.Find("Area Light (9)")?.gameObject;
            if (areaLight9 != null)
            {
                areaLight9.SetActive(true);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void TeleportPlayerServerRPC(NetworkObjectReference obj, Vector3 pos) => TeleportPlayerClientRPC(obj, pos);

        [ClientRpc]
        public void TeleportPlayerClientRPC(NetworkObjectReference obj, Vector3 pos)
        {
            NetworkObject netObj = null;
            obj.TryGet(out netObj);
            if (netObj == null)
            {
                Log.LogError("NetworkObject in TeleportPlayerClientRpc() is null.");
                return;
            }

            PlayerControllerB component = netObj.GetComponent<PlayerControllerB>();
            if (component != null)
            {
                component.isInElevator = false;
                component.isInHangarShipRoom = false;
                component.isInsideFactory = true;
                component.averageVelocity = 0f;
                component.velocityLastFrame = Vector3.zero;
                component.TeleportPlayer(pos);
                component.beamOutParticle.Play();

                var presets = UnityEngine.Object.FindObjectOfType<AudioReverbPresets>();
                if (presets != null)
                {
                    presets.audioPresets[2].ChangeAudioReverbForPlayer(component);
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPriority(Priority.VeryHigh)]
        [HarmonyPatch(typeof(GameNetworkManager), "Start")]
        private static void InitalizeServerObject()
        {
            if (netObject != null) return;


            netObject = (GameObject)Assets.bundle.LoadAsset("BrutalCompanyMinus");

            netObject.AddComponent<Net>();
            netObject.AddComponent<EnemySpawnCycle>();
            NetworkManager.Singleton.AddNetworkPrefab(netObject);
        }

        [HarmonyPrefix]
        [HarmonyPriority(Priority.VeryHigh)]
        [HarmonyPatch(typeof(Terminal), "Start")]
        private static void SpawnServerObject()
        {

            if (!FindObjectOfType<NetworkManager>().IsServer) return;

            GameObject net = Instantiate(netObject);
            net.GetComponent<NetworkObject>().Spawn(destroyWithScene: false);

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), "ShipLeave")]
        private static void OnGameEnd()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                // Randomize weather multipliers
                Instance.UpdateCurrentWeatherMultipliersServerRpc();
                Instance.SetRecievedServerRpc(false);

                Instance.currentWeatherEffects.Clear(); // Clear weather effects
                Instance.outsideObjectsToSpawn.Clear();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ResizeHotbarRandomlyServerRpc()
        {
            ResizeHotbarRandomlyClientRpc();
        }

        [ClientRpc]
        public void ResizeHotbarRandomlyClientRpc()
        {
            if (Compatibility.HotBarPlusPresent)
            {
                HotBarPlusCompat.ResizeHotbarRandomly();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ResizeHotbarRandomlySmallServerRpc()
        {
            ResizeHotbarRandomlySmallClientRpc();
        }

        [ClientRpc]
        public void ResizeHotbarRandomlySmallClientRpc()
        {
            if (Compatibility.HotBarPlusPresent)
            {
                HotBarPlusCompat.ResizeHotbarRandomlySmall();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ResetHotbarServerRpc()
        {
            if (Compatibility.HotBarPlusPresent)
            {
                ResetHotbarClientRpc();
            }
        }

        [ClientRpc]
        public void ResetHotbarClientRpc()
        {
            if (Compatibility.HotBarPlusPresent)
            {
                HotBarPlusCompat.ResetHotbar();
            }
        }

        public struct CurrentWeatherEffect : INetworkSerializable, IEquatable<CurrentWeatherEffect>
        {
            public FixedString128Bytes name;
            public bool state;

            public CurrentWeatherEffect(FixedString128Bytes name, bool state)
            {
                this.name = name;
                this.state = state;
            }

            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                if (serializer.IsReader)
                {
                    FastBufferReader reader = serializer.GetFastBufferReader();
                    reader.ReadValueSafe(out name);
                    reader.ReadValueSafe(out state);
                }
                else
                {
                    FastBufferWriter write = serializer.GetFastBufferWriter();
                    write.WriteValueSafe(name);
                    write.WriteValueSafe(state);
                }
            }

            public bool Equals(CurrentWeatherEffect other)
            {
                return name == other.name;
            }
        }

        public struct OutsideObjectsToSpawnMethod : INetworkSerializable, IEquatable<OutsideObjectsToSpawnMethod>
        {
            public float density;
            public int objectEnumID;

            public OutsideObjectsToSpawnMethod(float density, int objectEnumID)
            {
                this.density = density;
                this.objectEnumID = objectEnumID;
            }

            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                if (serializer.IsReader)
                {
                    FastBufferReader reader = serializer.GetFastBufferReader();
                    reader.ReadValueSafe(out density);
                    reader.ReadValueSafe(out objectEnumID);
                }
                else
                {
                    FastBufferWriter write = serializer.GetFastBufferWriter();
                    write.WriteValueSafe(density);
                    write.WriteValueSafe(objectEnumID);
                }
            }

            public bool Equals(OutsideObjectsToSpawnMethod other)
            {
                return (objectEnumID == other.objectEnumID) && (density == other.density);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetCruiserOfflineServerRpc()
        {
            SetCruiserOfflineClientRpc();
        }

        [ClientRpc]
        public void SetCruiserOfflineClientRpc()
        {
            try
            {
                GameObject companyCruiser = Assets.cruiser;
                if (companyCruiser != null)
                {
                    Log.LogInfo("Cruiser found, attempting to set ignition off.");
                    try
                    {
                        VehicleController controller = Assets.cruiser.GetComponent<VehicleController>();
                        controller.SetIgnition(false);
                    }
                    catch (Exception e)
                    {
                        Log.LogError("Error setting cruiser ignition " + e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Log.LogError("Error finding cruiser or setting ignition " + e.Message);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SyncLeverTooltipServerRpc(string text)
        {
            SyncLeverTooltipClientRpc(text);
        }

        [ClientRpc]
        public void SyncLeverTooltipClientRpc(string text)
        {
            try
            {
                StartMatchLever lever = GameObject.FindObjectOfType<StartMatchLever>();
                lever.triggerScript.disabledHoverTip = text;
            }
            catch (Exception e)
            { 
                Log.LogError(e);
            }
        }

    }
}
