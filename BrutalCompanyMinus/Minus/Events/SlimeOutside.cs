using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BrutalCompanyMinus.Minus.Handlers;
using GameNetcodeStuff;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;
using static BrutalCompanyMinus.Net;

namespace BrutalCompanyMinus.Minus.Events
{
    [HarmonyPatch]
    internal class SlimeOutside : MEvent
    {
        public override string Name() => nameof(SlimeOutside);

        public static SlimeOutside Instance;

        public static float SlippinessValue;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "I don't know how.. but its very slippery outside.", "Be careful moving around", "You will not like this..." };
            ColorHex = "#8B008B";
            Type = EventType.Insane;
            isBetaEvent = true;
            isSpecialEvent = true;

            ScaleList.Add(ScaleType.Slipperyness, new Scale(2.0f, 0.0f, 2.0f, 2.0f));
        }

        public override void Execute()
        {
            Net.Instance.SetEventActiveServerRPC(Name(), true);

            GameObject SlimeInsideObj = new GameObject("SlimeOutsideObj");

            SlimeInsideObj.AddComponent<SlimeOutsideNet>();

            float Slipperyness = Getf(ScaleType.Slipperyness);

            Net.Instance.SetSlimeSlipperyOutServerRpc(Slipperyness);
        }

        public override void OnShipLeave()
        {
            foreach (var player in StartOfRound.Instance.allPlayerScripts)
            {
                if (player == null) continue;
                player.slipperyFloor = 0f;
            }

            // Reset the Active state
            Active = false;
        }
        public override void OnGameStart()
        {
            // Reset the Active state
            Active = false;
        }

        public override void OnLocalDisconnect()
        {
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.Update))]
        public static void Update(PlayerControllerB __instance)
        {
            if (SlimeOutside.Instance.Active == false) return;

            if (__instance == null) return;

            if (!__instance.isPlayerDead && !__instance.isInHangarShipRoom && !__instance.isInsideFactory && __instance.thisController.isGrounded)
            {
                //Set slippery value
                __instance.slipperyFloor = SlippinessValue;
            }
        }

    }
}
