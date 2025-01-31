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
using static BrutalCompanyMinus.Minus.Events.SafeBridges;


namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(BridgeTrigger))]
    internal class SafeBridgesHPatches
    {
        private static float lockdownTime;

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void OverwriteBridgeData(BridgeTrigger __instance)
        {
            // Overwrite the door power and change the display text.
            if (SafeBridgesNet.Value == 0)
            {
                // Make sure the power is constantly maxed out.

                __instance.bridgeDurability = 100f;
                //  __instance.SetDoorOpen();
                __instance.playerCapacityAmount = 50f;
                __instance.weightCapacityAmount = 10000f;
                // Cycle between two text messages.
                // lockdownTime = (lockdownTime + Time.deltaTime) % 6;
                // __instance.doorPowerDisplay.text = $"{(lockdownTime > 3 ? "FAILURE" : "RETRYING")}";
            }
        }
    }
}
