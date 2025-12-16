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
    internal class TimeChaosNet : NetworkBehaviour 
    {
        public static TimeChaosNet instance;

        public void Awake()
        {
            if (instance != null) DestroyTimeInstance();
            instance = this;
        }
        public void Update()
        {
            if (!TimeChaos.Instance.Active) return;

            // Set the time scale
            try
            {
                // Constantly multiplies time by the time multiplier
                Net.Instance.MoveTimeServerRpc(0, TimeChaos.timeMultiplier);
            }
            catch
            {
                // Fallback if something were to go wrong
                Net.Instance.MoveTimeServerRpc(0, 1.0001f);
            }
        }

        public static void DestroyTimeInstance() // This handles the deletion of Time Chaosness
        {
            Events.TimeChaos.Instance.Active = false;
            TimeOfDay.Instance.globalTimeSpeedMultiplier = 1.0f;
            GameObject netObject = GameObject.Find("TimeChaosEvent");
            if (netObject != null)
            {
                GameObject.Destroy(netObject);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.ShipLeave))]
        public static void OnShipLeave()
        {
            DestroyTimeInstance();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndGameServerRpc))]
        public static void DestroyForGodsSake()
        {
            DestroyTimeInstance();
        }
    }
}
