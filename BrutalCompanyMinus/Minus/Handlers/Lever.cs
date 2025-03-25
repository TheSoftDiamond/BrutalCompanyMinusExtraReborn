using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BrutalCompanyMinus.Minus.Events;
using Unity.Netcode;
using static BrutalCompanyMinus.Minus.Events.LeverFailure;
using static BrutalCompanyMinus.Minus.Events.ShipLightsFailure;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(StartMatchLever))]
    internal class StartMatchLeverPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("PullLever")]
        private static bool InterruptPullLever(StartMatchLever __instance)
        {
            // Interrupt pulling the lever if a power malfunction has triggered.
            if (LeverNet.Value == 0/* && ShipLightsNet.Value == 0*/)
            {


                HUDManager.Instance.globalNotificationText.text =
                    "SHIP CORE DEPLETION:\nAWAIT 12PM EMERGENCY AUTOPILOT";

                HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                HUDManager.Instance.UIAudio.PlayOneShot(
                    HUDManager.Instance.radiationWarningAudio,
                    1f
                );

                // Make sure that the tooltip specifies this.
                __instance.triggerScript.disabledHoverTip = "[No power to hydraulics]";

                return false;

            }
            else if (LeverNet.Value == 0)
            {
                HUDManager.Instance.globalNotificationText.text =
                    "SHIP LEVER HYDRAULICS JAM:\nAWAIT 12PM EMERGENCY AUTOPILOT";

                HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                HUDManager.Instance.UIAudio.PlayOneShot(
                    HUDManager.Instance.radiationWarningAudio,
                    1f
                );
                __instance.triggerScript.disabledHoverTip = "[Hydraulics jammed]";

                return false;
            }

            return true;
        }
    }
}