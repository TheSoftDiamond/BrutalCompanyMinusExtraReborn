using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BrutalCompanyMinus.Minus.Events;
using GameNetcodeStuff;
using System.Linq;
using Unity.Netcode;
using static BrutalCompanyMinus.Minus.Events.DoorOverdriveEv;


namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(HangarShipDoor))]
    internal class HangarShipDoorOvPatches
    {
        private static float lockdownTime;

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void OverwriteDoorPower(HangarShipDoor __instance)
        {
            // Overwrite the door power and change the display text.
            if (DoorOvNet.Value == 0)
            { 
                // Make sure the power is constantly maxed out.
               
                __instance.doorPower = 1f;
              //  __instance.SetDoorOpen();
                __instance.buttonsEnabled = true;
                // Cycle between two text messages.
               // lockdownTime = (lockdownTime + Time.deltaTime) % 6;
               // __instance.doorPowerDisplay.text = $"{(lockdownTime > 3 ? "FAILURE" : "RETRYING")}";
            }
        }
    }
}
