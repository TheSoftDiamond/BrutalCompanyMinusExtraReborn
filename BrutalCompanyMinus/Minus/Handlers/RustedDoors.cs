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
using static BrutalCompanyMinus.Minus.Events.RustedDoorsEv;
using System.IO;
using DunGen;


namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class DoorPatches
    {

        [HarmonyPostfix]
        [HarmonyPatch("FinishGeneratingNewLevelClientRpc")]
        private static void OverwriteDoorPower()
        {
            // Overwrite the door power and change the display text.
            if (RustedDoorsNet.Value == 0)
            {
                try
                {
                    foreach (var behaviour in UnityEngine.Object.FindObjectsByType<Door>(FindObjectsSortMode.None))
                    {
                        behaviour.
                        
                    }
                }
                catch { throw new MissingComponentException("Failed to call FinishGeneratingNewLevelClientRpc on ZombiesBehaviour"); }
            }
        }

        /*  [HarmonyPostfix]
          [HarmonyPatch("Update")]
          private static void OverwriteDoorState(HangarShipDoor __instance)
          {
            // Overwrite the door power and change the display text.
            if (DoorCircuitNet.Value == 0)
            {
                if (/*TimeOfDay.Instance.hour == 4 | TimeOfDay.Instance.hour == 5 | TimeOfDay.Instance.hour == 6 | TimeOfDay.Instance.hour == 7 
                    | TimeOfDay.Instance.hour == 8 |*/ /*TimeOfDay.Instance.hour == 9 | TimeOfDay.Instance.hour == 10 | TimeOfDay.Instance.hour == 11 
                    | TimeOfDay.Instance.hour == 12 | TimeOfDay.Instance.hour == 13 | TimeOfDay.Instance.hour == 14 | TimeOfDay.Instance.hour == 15)
                {  // Make sure the power is constantly maxed out.
                    __instance.doorPower = 1f;
                   // __instance.SetDoorClosed();
                    __instance.PlayDoorAnimation(true);
                    __instance.buttonsEnabled = false;
                    // Cycle between two text messages.
                    lockdownTime = (lockdownTime + Time.deltaTime) % 6;
                    __instance.doorPowerDisplay.text = $"{(lockdownTime > 3 ? "LOCKED" : "OPEN 10PM")}";
                }
               else if (TimeOfDay.Instance.hour == 16)
                {// Make sure the power is constantly maxed out.
                 // __instance.doorPower = 1f;
                    __instance.PlayDoorAnimation(false);
                  //  __instance.SetDoorOpen();
                    __instance.buttonsEnabled = true;*/
                    // Cycle between two text messages.
                  //  lockdownTime = (lockdownTime + Time.deltaTime) % 6;
                  //  __instance.doorPowerDisplay.text = $"{(lockdownTime > 3 ? "LOCKED" : "OPEN 10PM")}";
           /*     }

            }
          }*/


      /*  [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void OverwriteClockText(HUDManager __instance)
        {
            
            
            // Overwrite the door power and change the display text.
             if (ShipLightsNet.Value == 0)
             {
            __instance.DisplayStatusEffect("NULL");
             }
        }*/
    }
}
