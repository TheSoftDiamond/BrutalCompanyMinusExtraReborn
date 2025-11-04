using HarmonyLib;
using UnityEngine;
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
            if (CameraUnityNet.Value == true)
            {
                __instance.offScreenMat.color = Color.black;
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch("SwitchRadarTargetClientRpc")]
        private static bool InterruptSwitchCameraView(ManualCameraRenderer __instance)
        {
            if (CameraUnityNet.Value == true)
            {   __instance.offScreenMat.color = Color.black;
                __instance.currentCameraDisabled = true;
                return false;
            }
            return true;
        }
    }
}