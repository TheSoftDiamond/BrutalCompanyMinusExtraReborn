using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BrutalCompanyMinus.Minus.Events;
using static BrutalCompanyMinus.Minus.Events.CruiserFailure;
using System.Threading.Tasks;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(VehicleController))]
    internal class CruiserFailures
    {
        [HarmonyPrefix]
        [HarmonyPatch("StartTryCarIgnition")]
        private static bool InterruptCruiser()
        {
            // Interrupt the cruiser if a power malfunction has triggered.
            // If there are other vehicles from other mods, I will be happy to add compat here.
            if (CruiserFailure.Instance.Active)
            {
                int randomResponse = UnityEngine.Random.RandomRangeInt(0, 3);
                switch (randomResponse)
                {
                    case 0:
                        HUDManager.Instance.globalNotificationText.text = "Do you even have a license?!";
                        break;
                    case 1:
                        HUDManager.Instance.globalNotificationText.text = "Hope you have insurance!";
                        break;
                    case 2:
                        HUDManager.Instance.globalNotificationText.text = "Do you know how to operate the vehicle?";
                        break;
                    default:
                        HUDManager.Instance.globalNotificationText.text = "Your car is out of gas!";
                        break;
                }

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
