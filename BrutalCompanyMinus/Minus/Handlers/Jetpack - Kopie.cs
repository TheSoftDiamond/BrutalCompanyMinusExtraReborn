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
using static BrutalCompanyMinus.Minus.Events.JetpackFailure;
using System.IO;


namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(StartMatchLever))]
    internal class TestingPatches
    {

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void PreventItemActivation(StartMatchLever __instance)
        {
            // Overwrite the door power and change the display text.
            if (JetpackNet.Value == 1)
            {
                // Make sure the power is constantly maxed out.
                __instance.


            

            }

        }
    }
}
