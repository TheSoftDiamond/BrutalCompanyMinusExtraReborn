﻿using BrutalCompanyMinus.Minus.Handlers.Modded;
using GameNetcodeStuff;
using HarmonyLib;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Handlers
{
    internal class _EnemyAI
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(EnemyAI.MeetsStandardPlayerCollisionConditions))]
        private static void OnMeetsStandardPlayerCollisionConditions(ref PlayerControllerB __result, ref Collider other, ref EnemyType ___enemyType, ref bool ___isEnemyDead, ref bool inKillAnimation, ref float ___stunNormalizedTimer) // This fix works, maybe theres a better way
        {
            // Why am i doing this again? just gona leave it here since nothing breaks.
            PlayerControllerB controller = other.gameObject.GetComponent<PlayerControllerB>();
            if (controller != null)
            {
                if (!___isEnemyDead && ___stunNormalizedTimer < 0.0f && !inKillAnimation && __result == null) // (This may have some unintended consequences)
                {
                    if (controller.actualClientId == GameNetworkManager.Instance.localPlayerController.actualClientId) __result = controller;
                }
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void PatchEnemyStart(Harmony harmony)
        {
            ApplyEnemyStart(harmony);
        }

        [HarmonyPostfix]
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void ApplyEnemyStart(Harmony harmony)
        {
            var originalMethod = AccessTools.Method(typeof(EnemyAI), nameof(EnemyAI.Start));
            var prefixMethod = AccessTools.Method(typeof(_EnemyAI), nameof(OnStart));

            if (originalMethod != null && prefixMethod != null)
            {
                harmony.Patch(originalMethod, prefix: new HarmonyMethod(prefixMethod));
                Log.LogInfo("Successfully patched EnemyAI");
            }
            else
            {
                Log.LogError("Failed to locate methods for conditional patching.");
            }
        }

        //[HarmonyPostfix]
        //[HarmonyPatch("Start")]
        public static void OnStart(ref EnemyAI __instance) // Set isOutside and scale hp
        {
            __instance.StartCoroutine(UpdateHP(__instance));
            
            try
            {
                GameObject terrainMap = Manager.terrainObject;

                float y = -100.0f;
                if (terrainMap != null) y = terrainMap.transform.position.y - 100.0f;

                if (__instance.transform.position.y > y)
                {
                    __instance.isOutside = true;
                    __instance.allAINodes = GameObject.FindGameObjectsWithTag("OutsideAINode"); // Otherwise AI would be fucked
                    if (GameNetworkManager.Instance.localPlayerController != null)
                    {
                        __instance.EnableEnemyMesh(!StartOfRound.Instance.hangarDoorsClosed || !GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom);
                    }
                    __instance.SyncPositionToClients();
                }
                else
                {
                    __instance.isOutside = false;
                    __instance.allAINodes = GameObject.FindGameObjectsWithTag("AINode");
                    __instance.SyncPositionToClients();
                }
            } catch
            {
                Log.LogError("Failed to set isOutside on EnemyAI.Start");
            }


        }

        private static IEnumerator UpdateHP(EnemyAI __instance)
        {
            yield return new WaitUntil(() => Net.Instance.receivedSyncedValues);
            __instance.enemyHP = (int)Mathf.Clamp(__instance.enemyHP + Manager.bonusEnemyHp, 1.1f, 99999999.0f);
        }

    }
}
