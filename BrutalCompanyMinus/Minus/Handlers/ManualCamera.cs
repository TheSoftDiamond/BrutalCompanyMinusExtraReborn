using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BrutalCompanyMinus.Minus.Events;
using static BrutalCompanyMinus.Minus.Events.ManualCameraFailure;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(ManualCameraRenderer))]
    internal class ManualCameraRendererPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("SwitchScreenButton")]
        private static bool InterruptSwitchScreenButton(ManualCameraRenderer __instance)
        {
            // Interrupt the screen button if a power or distortion malfunction has triggered.
            if (CameraNet.Value == 0)
            {
                __instance.offScreenMat.color = Color.black;
              //  __instance.currentCameraDisabled = true;
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch("SwitchRadarTargetClientRpc")]
        private static bool InterruptSwitchCameraView(ManualCameraRenderer __instance)
        {
            // Interrupt the screen button if a power or distortion malfunction has triggered.
            if (CameraNet.Value == 0)
            {   __instance.offScreenMat.color = Color.black;
                __instance.currentCameraDisabled = true;
              //  __instance.SwitchScreenOn();
            //  bridgeTrigger.bridgeDurability = 0;
                return false;
            }

            return true;
        }
    }
}