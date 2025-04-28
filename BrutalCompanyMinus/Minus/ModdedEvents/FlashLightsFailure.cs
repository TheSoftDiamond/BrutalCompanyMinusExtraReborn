using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using HarmonyLib;
using LethalNetworkAPI;
using GameNetcodeStuff;
using BrutalCompanyMinus.Minus.Handlers.Modded;
using BrutalCompanyMinus;
using Steamworks.Ugc;

namespace BrutalCompanyMinus.Minus.Events
{
    [HarmonyPatch]
    internal class FlashLightsFailure : MEvent
    {
        public override string Name() => nameof(FlashLightsFailure);

        public static FlashLightsFailure Instance;

        public static int FlashLightActive = 1;

        public static bool Active = false;

        public static LethalNetworkVariable<int> FlashLightNet = new LethalNetworkVariable<int>(identifier: "flashlightid") { Value = 1 };

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "I think the batteries leaked", "Who broke the flashlights?" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

        }

        public override void Execute()
        {
            FlashlightsGoEmptyAtStart();
            Active = true;
        }
        public override void OnShipLeave()
        {
            ChargeUpBatteries();
            Active = false;
        }
        public override void OnGameStart()
        {
            Active = false;
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
                        item.insertedBattery = new Battery(true, 0f);
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
                    item.insertedBattery = new Battery(true, 0f);
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
                    item.insertedBattery = new Battery(false, 1f);
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
                        item.insertedBattery = new Battery(false, 1f);
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
                    item.insertedBattery = new Battery(false, 1f);
                    item.SyncBatteryServerRpc(100);
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.GrabItem))]
        public static void FlashlightFailureItemGrab(GrabbableObject __instance)
        {
            if (Events.FlashLightsFailure.Active)
            {
                if (__instance.itemProperties.itemName == "Flashlight" || __instance.itemProperties.itemName == "Pro-flashlight")
                {
                    __instance.insertedBattery = new Battery(false, 0f);
                    __instance.SyncBatteryServerRpc(0);
                }
            }
        }
        
        // We borrow from the ItemChargerPatches to allow it to patch flashlights.
        [HarmonyPatch(typeof(ItemCharger))]
        internal class ItemChargerPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("ChargeItem")]
            private static bool InterruptChargeFlashlightItem()
            {
                // Interrupt the charger if certain criterion met
                if (Events.FlashLightsFailure.Active)
                {
                    GrabbableObject currentItem = GameNetworkManager.Instance.localPlayerController.currentlyHeldObjectServer;
                    if (currentItem.itemProperties.itemName == "Flashlight" || currentItem.itemProperties.itemName == "Pro-flashlight")
                    {
                        HUDManager.Instance.globalNotificationText.text =
                        "FLASHLIGHT CANNOT BE CHARGED!!!!";

                        HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                        HUDManager.Instance.UIAudio.PlayOneShot(
                            HUDManager.Instance.radiationWarningAudio,
                            1f
                        );
                        return false;
                    }
                }

                return true;
            }
        }
    }
}