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
using static BrutalCompanyMinus.Minus.Events.FlashLightsFailure;
using System.IO;


namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(FlashlightItem))]
    internal class FlashlightItemPatches
    {

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void PreventItemActivation(FlashlightItem __instance)
        {
            // Overwrite the door power and change the display text.
            if (FlashLightNet.Value == 0)
            {
                // Make sure the power is constantly maxed out.
                __instance.flashlightBulb.intensity = 0;
                __instance.insertedBattery.empty = true;
                __instance.flashlightInterferenceLevel = 10;
            

            }

        }
    }
}
