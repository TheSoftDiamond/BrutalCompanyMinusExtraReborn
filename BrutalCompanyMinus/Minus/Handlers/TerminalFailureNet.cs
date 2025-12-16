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
    internal class TerminalFailureNet : NetworkBehaviour
    {
        public static TerminalFailureNet instance;

        public void Awake()
        {
            if (instance != null) DestroyInstance();
            instance = this;
            Net.Instance.SetEventActiveServerRPC("TerminalFailure", true);
        }

        public static void DestroyInstance() // This handles the deletion of Time Chaosness
        {
            Events.TerminalFailure.Instance.Active = false;
            TimeOfDay.Instance.globalTimeSpeedMultiplier = 1.0f;
            GameObject netObject = GameObject.Find("TerminalFailureEvent");
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
