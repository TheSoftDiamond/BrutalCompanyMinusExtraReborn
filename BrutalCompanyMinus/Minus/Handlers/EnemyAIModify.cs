using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BrutalCompanyMinus.Minus.Events;
using GameNetcodeStuff;
using System.Linq;
using Unity.Netcode;
using static BrutalCompanyMinus.Minus.Events.ButlerAiTechinalDifficulties;


namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(FlowermanAI))]
    internal class EnemyAIModifyPatches
    {
        private static int temp = 1;

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void OverwriteBrackenAi(FlowermanAI __instance)
        {
            // Overwrite the door power and change the display text.
            if (temp == 0)
            {
                // Make sure the power is constantly maxed out.

                __instance.syncMovementSpeed = 2.5f;
                //  __instance.SetDoorOpen();
                // __instance.buttonsEnabled = true;
                // Cycle between two text messages.
                // lockdownTime = (lockdownTime + Time.deltaTime) % 6;
                // __instance.doorPowerDisplay.text = $"{(lockdownTime > 3 ? "FAILURE" : "RETRYING")}";
            }
        }
    }

    [HarmonyPatch(typeof(ButlerEnemyAI))]
    internal class EnemyAIModify2Patches
    {
        [HarmonyPostfix]
        [HarmonyPatch("LookForChanceToMurder")]
        private static bool OverwriteButlerAi(ButlerEnemyAI __instance)
        {
            if (ButlerNet.Value == 0)
            {
                __instance.agent = null;
                __instance.AIIntervalTime = 0;
                __instance.madlySearchingForPlayers = false;
                __instance.moveTowardsDestination = false;
                __instance.syncMovementSpeed = 0.3f;
                __instance.animationContainer = null;

                return false;
            }
            return true;
        }

    }
}
