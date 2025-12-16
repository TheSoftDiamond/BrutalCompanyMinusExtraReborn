using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using HarmonyLib;
using BepInEx.Configuration;
using BrutalCompanyMinus.Minus.Events;
using static BrutalCompanyMinus.Minus.MEvent;
using System.Collections;

namespace BrutalCompanyMinus.Minus.MonoBehaviours
{
    [HarmonyPatch]
    internal class ShipLightsFailureNet : NetworkBehaviour
    {
        public static ShipLightsFailureNet instance;

        public void Awake()
        {
            if (instance != null) DestroyInstance();
            instance = this;
            Net.Instance.SetEventActiveServerRPC("ShipLightsFailure", true);
        }

        public static void DestroyInstance() // This handles the deletion of Time Chaosness
        {
            Events.ShipLightsFailure.Instance.Active = false;
            GameObject netObject = GameObject.Find("ShipLightsFailureEvent");
            if (netObject != null)
            {
                GameObject.Destroy(netObject);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.ShipLeave))]
        public static void OnShipLeave()
        {
            DestroyInstance();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndGameServerRpc))]
        public static void DestroyForGodsSake()
        {
            DestroyInstance();
        }
    }
}
