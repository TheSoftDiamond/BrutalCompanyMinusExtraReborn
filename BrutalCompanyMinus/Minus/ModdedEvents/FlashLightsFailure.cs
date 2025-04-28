using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using LethalNetworkAPI;
using GameNetcodeStuff;
using BrutalCompanyMinus.Minus.Handlers.Modded;
using BrutalCompanyMinus;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class FlashLightsFailure : MEvent
    {
        public override string Name() => nameof(FlashLightsFailure);

        public static FlashLightsFailure Instance;

        public static int FlashLightActive = 1;

        public static LethalNetworkVariable<int> FlashLightNet = new LethalNetworkVariable<int>(identifier: "flashlightid") { Value = 1 };

        public override void Initalize()
        {
            Instance = this;

            Weight = 3; //7
            Descriptions = new List<string>() { "I think the batteries leaked", "Who broke the flashlights?" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

        }

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                FlashLightNet.Value = 0;
                FlashLightActive = 0;
            }
            FlashLightActive = 0;

            FlashlightsGoEmptyAtStart();
        }
        public override void OnShipLeave()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                FlashLightNet.Value = 1;
                FlashLightActive = 1;
            }
            FlashLightActive = 1;

            ChargeUpBatteries();

        }
        public override void OnGameStart()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                FlashLightNet.Value = 1;
                FlashLightActive = 1;

            }
            FlashLightActive = 1;
        }
        internal void FlashlightsGoEmptyAtStart()
        {
            GameObject hangarShip = Assets.hangarShip;
            if (hangarShip == null)
            {
                return;
            }

            GrabbableObject[] itemsInShip = hangarShip.GetComponentsInChildren<GrabbableObject>();

            foreach (GrabbableObject item in itemsInShip)
            {
                if (item == null || (item.itemProperties.itemName != "Flashlight" && item.itemProperties.itemName != "Pro-flashlight"))
                {
                    continue;
                }
                else
                {
                    // Charge the item to 0%
                    item.insertedBattery = new Battery(false, 0f);
                    item.SyncBatteryServerRpc(0);
                }
            }

            GameObject companyCruiser = Assets.cruiser;
            if (companyCruiser != null)
            {
                GrabbableObject[] itemsInCruiser = companyCruiser.GetComponentsInChildren<GrabbableObject>();

                foreach (GrabbableObject item in itemsInCruiser)
                {
                    if (item == null || (item.itemProperties.itemName != "Flashlight" && item.itemProperties.itemName != "Pro-flashlight"))
                    {
                        continue;
                    }
                    else
                    {
                        // Charge the item to 0%
                        item.insertedBattery = new Battery(false, 0f);
                        item.SyncBatteryServerRpc(0);
                    }
                }
            }
            //else Log.LogDebug("No cruiser found");

            PlayerControllerB playerScript = GameNetworkManager.Instance.localPlayerController;
            if (playerScript == null) return;

            foreach (var item in playerScript.ItemSlots)
            {
                if (item == null || (item.itemProperties.itemName != "Flashlight" && item.itemProperties.itemName != "Pro-flashlight"))
                {
                    continue;
                }
                else
                {
                    // Charge the item to 0%
                    item.insertedBattery = new Battery(false, 0f);
                    item.SyncBatteryServerRpc(0);
                }
            }
        }

        internal void ChargeUpBatteries()
        {
            GameObject hangarShip = Assets.hangarShip;
            if (hangarShip == null)
            {
                return;
            }

            GrabbableObject[] itemsInShip = hangarShip.GetComponentsInChildren<GrabbableObject>();

            foreach (GrabbableObject item in itemsInShip)
            {
                if (item == null || (item.itemProperties.itemName != "Flashlight" && item.itemProperties.itemName != "Pro-flashlight"))
                {
                    continue;
                }
                else
                {
                    // Charge the item back to 100%
                    item.insertedBattery = new Battery(true, 1f);
                    item.SyncBatteryServerRpc(100);
                }
            }

            GameObject companyCruiser = Assets.cruiser;
            if (companyCruiser != null)
            {
                GrabbableObject[] itemsInCruiser = companyCruiser.GetComponentsInChildren<GrabbableObject>();

                foreach (GrabbableObject item in itemsInCruiser)
                {
                    if (item == null || (item.itemProperties.itemName != "Flashlight" && item.itemProperties.itemName != "Pro-flashlight"))
                    {
                        continue;
                    }
                    else
                    {
                        // Charge the item back to 100%
                        item.insertedBattery = new Battery(true, 1f);
                        item.SyncBatteryServerRpc(100);
                    }
                }
            }
            //else Log.LogDebug("No cruiser found");

            PlayerControllerB playerScript = GameNetworkManager.Instance.localPlayerController;
            if (playerScript == null) return;

            foreach (var item in playerScript.ItemSlots)
            {
                if (item == null || (item.itemProperties.itemName != "Flashlight" && item.itemProperties.itemName != "Pro-flashlight"))
                {
                    continue;
                }
                else
                {
                    // Charge the item back to 100%
                    item.insertedBattery = new Battery(true, 1f);
                    item.SyncBatteryServerRpc(100);
                }
            }
        }

    }
}