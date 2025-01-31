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
using LethalLib.Modules;
//using static BCMECodeSecurityCheck.Methods.RemoteMethods;


namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class ShipLightsAdditionsPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("Update")]
        private static void ShutdownShipLights(StartOfRound __instance)
        {
            // Interrupt light switching if a power malfunction has triggered.
            if (ShipLightsNet.Value == 0)                     
            {

                __instance.shipRoomLights.SetShipLightsServerRpc(false);

            }

            
        }
    }
}
