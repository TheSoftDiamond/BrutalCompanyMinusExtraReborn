using BrutalCompanyMinus.Minus.Events;
using GameNetcodeStuff;
using HarmonyLib;
using HotbarPlus.Networking;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace BrutalCompanyMinus.Minus.Handlers.Modded
{
    internal class KidnapperFoxPatches
    {
        public static int foxChance = 100; // Chance of the fox spawning.

        public static int minWeeds = 31; // Minimum number of weeds to spawn

        public static int originalSpawnChance;

        public static int originalMinWeeds;

        /*
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void PatchAll(Harmony harmony)
        {
            ApplyKidnapperFoxPatches(harmony);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void ApplyKidnapperFoxPatches(Harmony harmony)
        {
            var originalMethod = AccessTools.PropertyGetter(typeof(YesFox.HarmonyPatches), nameof(YesFox.HarmonyPatches.SpawnWeedEnemies));
            var prefixMethod = AccessTools.Method(typeof(KidnapperFoxPatches), nameof(FoxSpawnPatch));

            if (originalMethod != null && prefixMethod != null)
            {
                harmony.Patch(originalMethod, prefix: new HarmonyMethod(prefixMethod));
                Log.LogInfo("[KidnappeFoxPatches] Successfully patched YesFox.HarmonyPatches.SpawnWeedEnemies.");
            }
            else
            {
                Log.LogError("[KidnappeFoxPatches] Failed to locate methods for conditional patching.");
            }

            var originalLeaveMethod = AccessTools.Method(typeof(StartOfRound), nameof(StartOfRound.ShipLeave));
            var leavePrefixMethod = AccessTools.Method(typeof(KidnapperFoxPatches), nameof(FoxSpawnLeaveFix));

            if (originalLeaveMethod != null && leavePrefixMethod != null)
            {
                harmony.Patch(originalLeaveMethod, prefix: new HarmonyMethod(leavePrefixMethod));
                Log.LogInfo("[KidnappeFoxPatches] Successfully patched StartOfRound.ShipLeave");
            }
            else
            {
                Log.LogError("[KidnappeFoxPatches] Failed to locate methods for conditional patching.");
            }

            var originalGuaranteeMoldMethod = AccessTools.Method(typeof(StartOfRound), nameof(StartOfRound.StartGame));
            var guaranteeMoldPrefixMethod = AccessTools.Method(typeof(KidnapperFoxPatches), nameof(GuaranteeMold));
        
            if (originalGuaranteeMoldMethod != null && guaranteeMoldPrefixMethod != null)
            {
                harmony.Patch(originalGuaranteeMoldMethod, prefix: new HarmonyMethod(guaranteeMoldPrefixMethod));
                Log.LogInfo("[KidnappeFoxPatches] Successfully patched StartOfRound.StartGame");
            }
            else
            {
                Log.LogError("[KidnappeFoxPatches] Failed to locate methods for conditional patching.");
            }
        }

        public static bool FoxSpawnPatch(bool active)
        {
            if (KidnapperFox.Active)
            {
                originalSpawnChance = YesFox.Plugin.Fox_SpawnChance.Value;
                YesFox.Plugin.Fox_SpawnChance.Value = foxChance;
                //return true; // We still want to execute the original method but with our values
            }

            return true; 
        }

        public static bool GuaranteeMold(StartOfRound __instance)
        {
            if (KidnapperFox.Active)
            {
                if (__instance.currentLevel.moldSpreadIterations < 1)
                {
                    __instance.currentLevel.moldSpreadIterations = 1;
                    YesFox.HarmonyPatches.PlayerLoadedServerRpc(__instance);
                }
                //return true; // We still want to execute the original method
            }
            return true;
        }
        

        public static bool FoxSpawnLeaveFix()
        {
            if (KidnapperFox.Active)
            {
                YesFox.Plugin.Fox_SpawnChance.Value = originalSpawnChance;
                //return true;
            }
            return true; // We still want the ship to leave of course, but returning the original values to prevent issues down the road
        }
        */
    }
}
