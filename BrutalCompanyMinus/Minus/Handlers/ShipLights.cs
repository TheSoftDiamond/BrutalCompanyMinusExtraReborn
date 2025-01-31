using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BrutalCompanyMinus.Minus.Events;
using Unity.Netcode;
using GameNetcodeStuff;
using static BrutalCompanyMinus.Minus.Events.ShipLightsFailure;
using System.Data;
//using static BCMECodeSecurityCheck.Methods.RemoteMethods;


namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(ShipLights))]
    internal class ShipLightsPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("ToggleShipLights")]
        private static bool InterruptToggleShipLights()
        {
            // Interrupt light switching if a power malfunction has triggered.
            if (ShipLightsNet.Value == 0)                     //if (Events.ShipLightsFailure.LightsActive == 0)
            {

             //   BCMECodeSecurityCheck.Modules.EventRelatedStuff.KillLightsEvent();
             //   __instance.SetShipLightsBoolean(false);
             //   __instance.areLightsOn = false;
                    return false;
//
//
//                }
//                return true;
            }

            return true;
        }
        [HarmonyPatch(typeof(StartOfRound))]
        internal class StartOfRoundPatchesV2
        {
            [HarmonyPrefix]
            [HarmonyPatch("Update")]
            internal static void PreventLightsOn(ShipLights __instance)
            {
                // Interrupt light switching if a power malfunction has triggered.
                if (ShipLightsNet.Value == 0)                     //if (Events.ShipLightsFailure.LightsActive == 0)
                {
                    try
                    {
                        BCMECodeSecurityCheck.Modules.EventRelatedStuff.ToggleLightsEvent(false);
                        __instance.areLightsOn = false;
                       // StartOfRound.Instance.PowerSurgeShip();
                    }
                    catch
                    {
                        Log.LogError(" Failed to run PreventLightsOn()... Aborting!");
                    }
                }

                
            }
        }
    }
}
