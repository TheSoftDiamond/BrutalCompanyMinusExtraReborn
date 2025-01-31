using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BrutalCompanyMinus.Minus.Events;
using GameNetcodeStuff;
using System.Linq;
using static BrutalCompanyMinus.Minus.Events.LeverFailure;
using UnityEngine.UI;
using UnityEngine.Assertions.Must;
//using static BrutalCompanyMinus.Modules.MeltdownEffects;
using OpenMonitors.Monitors;


namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(CreditsMonitor))]
    internal class CreditsMonitorPatches
    {
        // Block the user from confirming a moon node.
        [HarmonyPrefix]
        [HarmonyPatch("Start")]
        private static bool InterruptCreditsMonitor(CreditsMonitor __instance)
        {
            // Make sure we disallow the result of the action if the navigation malfunction is active.
            if (LeverNet.Value == 0)
            {
                
                __instance.UpdateMonitor();
                __instance.gameObject.GetInstanceID();
                return false;
            
            
            }

            return true;
        }
    }
}
