using System;
using System.Collections.Generic;
using BrutalCompanyMinus.Minus.Handlers;
using BrutalCompanyMinus.Minus.Handlers.Modded;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using GameNetcodeStuff;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    [HarmonyPatch]
    internal class TZPCrazy : MEvent
    {
        public override string Name() => nameof(TZPCrazy);

        public static TZPCrazy Instance;

        public static float[] DrunkValues;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "Drunk at work", "High...", "Woah... you gotta try this.", "Someone changed the oxygen canisters.."};
            ColorHex = "#FFA500";
            Type = EventType.Bad;
            isSpecialEvent = true;
            Aliases = new List<string>() { "TZP" };
            ScaleList.Add(ScaleType.DrunknessMin, new Scale(0.00f, 0.0f, 0.0f, 0.0f));
            ScaleList.Add(ScaleType.DrunknessMax, new Scale(1.00f, 0.0f, 1.0f, 1.0f));
        }

        public override void Execute()
        {
            // Declare the event active
            Net.Instance.SetEventActiveServerRPC(Name(), true);

            GameObject netObject = new GameObject("TZPCrazyNet");

            netObject.AddComponent<TZPCrazyNet>();

            float drunknessmin = Getf(ScaleType.DrunknessMin);
            float drunknessmax = Getf(ScaleType.DrunknessMax);

            DrunkValues = new float[StartOfRound.Instance.allPlayerScripts.Length];
            for (int i = 0; i < DrunkValues.Length; i++)
            {
                DrunkValues[i] = UnityEngine.Random.Range(drunknessmin, drunknessmax);
            }

            Net.Instance.SetDrunkServerRpc(DrunkValues);

            
        }

        public override void OnShipLeave() //Patch to reset the network int value
        {
            // Reset the Active state
            Active = false;

            foreach (var player in StartOfRound.Instance.allPlayerScripts)
            {
                if (player == null) continue;
                player.drunkness = 0f;
            }
        }

        public override void OnGameStart() //Patch to reset the network int value
        {
            // Reset the Active state
            Active = false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.Update))]
        public static void Update(PlayerControllerB __instance)
        {
            if (TZPCrazy.Instance.Active == false) return;

            if (__instance == null) return;

            if (!__instance.isPlayerDead && __instance.isPlayerControlled)
            {
                __instance.drunkness = Mathf.Max(DrunkValues[__instance.playerClientId], __instance.drunkness);
                __instance.increasingDrunknessThisFrame = true;
            }
        }
    }
}
