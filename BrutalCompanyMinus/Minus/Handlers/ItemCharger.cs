using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BrutalCompanyMinus.Minus.Events;
using static BrutalCompanyMinus.Minus.Events.ItemChargerFailure;
using System.Threading.Tasks;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(ItemCharger))]
    internal class ItemChargerPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("ChargeItem")]
        private static bool InterruptChargeItem()
        {
            // Interrupt the charger if a power malfunction has triggered.
            if (/*ItemChargerNet.Value == 0*/ItemChargerUnityNet.Value == true)
            {
                HUDManager.Instance.globalNotificationText.text =
                "CHARGING STATION FAILURE:\nINSUFFICIENT POWER";

                HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                HUDManager.Instance.UIAudio.PlayOneShot(
                    HUDManager.Instance.radiationWarningAudio,
                    1f
                );
                return false;
            }

            return true;
        }
    }
}
