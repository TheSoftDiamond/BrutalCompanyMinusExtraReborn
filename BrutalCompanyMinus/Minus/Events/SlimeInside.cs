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
    internal class SlimeInside : MEvent
    {
        public override string Name() => nameof(SlimeInside);

        public static SlimeInside Instance;

        public static bool Active = false;

        public static float SlippinessValue;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "The facility is very slippery", "Be careful moving around" };
            ColorHex = "#8B008B";
            Type = EventType.Insane;
            isBetaEvent = true;
            isSpecialEvent = true;

            ScaleList.Add(ScaleType.Slipperyness, new Scale(2.0f, 0.0f, 2.0f, 2.0f));
        }

        public override void Execute()
        {
            Net.Instance.SetSlimeServerRpc(true);
            
            GameObject SlimeInsideObj = new GameObject("SlimeInsideObj");

            SlimeInsideObj.AddComponent<SlimeInsideNet>();

            float Slipperyness = Getf(ScaleType.Slipperyness);

            Net.Instance.SetSlimeSlipperyServerRpc(Slipperyness);
        }

        public override void OnShipLeave()
        {
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
            if (SlimeInside.Active == false) return;

            if (__instance == null) return;

            if (!__instance.isPlayerDead && __instance.isInsideFactory && __instance.thisController.isGrounded)
            {
                //Set slippery value
                __instance.slipperyFloor = SlippinessValue;
            }
        }

    }
}
