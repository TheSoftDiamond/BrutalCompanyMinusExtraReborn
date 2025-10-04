using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using BrutalCompanyMinus.Minus.Events;
using ElevatorMod.Patches;
using HarmonyLib;
using Scoops;
using Scoops.misc;
using Scoops.patch;

namespace BrutalCompanyMinus.Minus.Handlers.Modded
{
    internal class EndlessElevatorPatching
    {
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void PatchAllElevator(Harmony harmony)
        {
            ApplyElevatorPatch(harmony);
        }

        [HarmonyPrefix]
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void ApplyElevatorPatch(Harmony harmony)
        {
            var originalMethod = AccessTools.Method(typeof(EndlessElevator), nameof(EndlessElevator.GenerateNewFloor));
            var prefixMethod = AccessTools.Method(typeof(EndlessElevatorPatching), nameof(GenerateBrutalEvents));

            if (originalMethod != null && prefixMethod != null)
            {
                harmony.Patch(originalMethod, prefix: new HarmonyMethod(prefixMethod));
                Log.LogInfo("Successfully patched EndlessElevator.GenerateNewFloor.");
            }
            else
            {
                Log.LogError("Failed to locate methods for conditional patching.");
            }
        }

        public static bool GenerateBrutalEvents()
        {  
            SelectableLevel newLevel = new SelectableLevel();
            EventManager.ModifyLevel(ref newLevel);
            return true;
        }
    }
}
