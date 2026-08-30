using BrutalCompanyMinus.Minus.Events;
using HarmonyLib;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(ManualCameraRenderer))]
    internal class ManualCameraRendererPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("SwitchScreenButton")]
        private static bool InterruptSwitchScreenButton(ManualCameraRenderer __instance)
        {
            if (Events.ManualCameraFailure.Instance.Active)
            {
                __instance.SwitchScreenOn(false);
                __instance.syncingSwitchScreen = true;
                __instance.SwitchScreenOnServerRpc(false);
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch("SwitchRadarTargetClientRpc")]
        private static bool InterruptSwitchCameraView(ManualCameraRenderer __instance)
        {
            if (Events.ManualCameraFailure.Instance.Active)
            {
                if (__instance.isScreenOn)
                {
                    __instance.SwitchScreenOn(false);
                    __instance.syncingSwitchScreen = true;
                    __instance.SwitchScreenOnServerRpc(false);
                }
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch("SwitchScreenButton")]
        [HarmonyPatch("SwitchRadarTargetForward")]
        private static bool BlockSwitchScreen()
        {
            if (Events.ManualCameraFailure.Instance.Active)
            {
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch("SwitchScreenOn")]
        private static bool BlockSwitchScreenOn(bool on)
        {
            if (Events.ManualCameraFailure.Instance.Active && on)
            {
                return false;
            }
            return true;
        }
    }
}