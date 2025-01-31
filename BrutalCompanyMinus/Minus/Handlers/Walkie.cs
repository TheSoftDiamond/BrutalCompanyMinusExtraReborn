using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BrutalCompanyMinus.Minus.Events;
using static BrutalCompanyMinus.Minus.Events.WalkieFailure;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(WalkieTalkie))]
    internal class WalkieTalkiePatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("ItemActivate")]
        private static bool InterruptItemActivate(WalkieTalkie __instance)
        {
            // Interrupt the walkie if a distortion malfunction has triggered.
            if (WalkiesNet.Value == 0)
            {
                // Play a sound and dicharge the batteries.
                __instance.thisAudio.PlayOneShot(__instance.playerDieOnWalkieTalkieSFX);
                __instance.UseUpBatteries();

                return false;
            }

            return true;
        }
    }
}
