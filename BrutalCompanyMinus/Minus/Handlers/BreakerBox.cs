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
using static BrutalCompanyMinus.Minus.Events.TerminalFailure;


namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(BreakerBox))]
    internal class BreakerBoxPatches
    {

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void OverwriteSwitchBreaker(BreakerBox __instance)
        {
            // Overwrite the door power and change the display text.
            if (TerminalNet.Value == 0)
            {
                // Make sure the power is constantly maxed out.

                __instance.leversSwitchedOff = 5;
                //   bbx.isPowerOn = false;
            }
        }
    }
}
