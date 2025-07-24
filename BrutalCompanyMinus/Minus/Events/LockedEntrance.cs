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
using BrutalCompanyMinus.Minus.Handlers;
using System.ComponentModel.Design;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using BepInEx;

namespace BrutalCompanyMinus.Minus.Events
{
    [HarmonyPatch]
    internal class LockedEntrance : MEvent
    {
        public override string Name() => nameof(LockedEntrance);

        public static LockedEntrance Instance;

        public static bool Active = false;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "The entrance is locked", "The entrance is blocked", "Time for the fire exit" };
            ColorHex = "#000000";
            Type = EventType.VeryBad;
        }

        public override void Execute()
        {

            // Declare the Active state to true globally
            Net.Instance.SetEntranceServerRpc(true);

            // Bind the FlashLightFailure to an GameObject
            GameObject LockEntranceObject = new GameObject("LockEntranceObject");

            // Add the FlashlightItemChargerPatches component to the GameObject
            LockEntranceObject.AddComponent<DoorLockPatches>();
        }
        public override void OnShipLeave()
        {

            // Reset the Active state
            Active = false;
        }
        public override void OnGameStart()
        {
            // Reset the Active state
            Active = false;
        }

        public override void OnLocalDisconnect()
        {
        }

        public override bool AddEventIfOnly() => Assets.ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Enable Special Events?");

        [HarmonyPrefix]
        [HarmonyPatch(typeof(InteractTrigger), nameof(InteractTrigger.Interact))]
        public static bool InterruptEntranceTeleport(Transform playerTransform, InteractTrigger __instance)
        {
            // Interrupt the charger method  
            if (Events.LockedEntrance.Active)
            {

                PlayerControllerB localPlayer = GameNetworkManager.Instance.localPlayerController;
                if (localPlayer != null)
                {
                    Transform interactObject = __instance.transform;
                    if (interactObject.name.Contains("EntranceTeleportA"))
                    {
                        __instance.interactable = false;
                        //Give a random response using random
                        int randomResponse = UnityEngine.Random.RandomRangeInt(0, 3);
                        switch (randomResponse)
                        {
                            case 0:
                                HUDManager.Instance.globalNotificationText.text = "The entrance is locked!";
                                break;
                            case 1:
                                HUDManager.Instance.globalNotificationText.text = "You can't go through here!";
                                break;
                            case 2:
                                HUDManager.Instance.globalNotificationText.text = "The door is blocked!";
                                break;
                            default:
                                HUDManager.Instance.globalNotificationText.text = "DOOR IS LOCKED!!!!";
                                break;
                        }

                        HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                        HUDManager.Instance.UIAudio.PlayOneShot(
                            HUDManager.Instance.radiationWarningAudio,
                            1f
                        );
                        return false;
                    }
                }
            }
            return true;
        }
    }
}