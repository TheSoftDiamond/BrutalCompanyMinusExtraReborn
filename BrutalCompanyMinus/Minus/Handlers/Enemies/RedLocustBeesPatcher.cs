using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Handlers.Enemies
{
    [HarmonyPatch(typeof(RedLocustBees))]
    internal static class RedLocustBeesPatcher
    {
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(RedLocustBees.SpawnHiveClientRpc))]
        internal static IEnumerable<CodeInstruction> IncreaseHiveScrapValue(IEnumerable<CodeInstruction> instructions)
        {
            var scrapValueMultiplier = typeof(Manager).GetField(nameof(Manager.scrapValueMultiplier), BindingFlags.Static | BindingFlags.NonPublic);
            var roundMethod = typeof(Mathf).GetMethod(nameof(Mathf.RoundToInt));

            var found = false;
            foreach (var instruction in instructions)
            {
                if (instruction.IsLdarg(2) && !found)
                {
                    found = true;
                    yield return new CodeInstruction(OpCodes.Ldarg_2);                          // push 'hiveScrapValue' to stack
                    yield return new CodeInstruction(OpCodes.Conv_R4);                          // converting 'hiveScrapValue' to float32
                    yield return new CodeInstruction(OpCodes.Ldsfld, scrapValueMultiplier);     // push bcmer multiplier to stack
                    yield return new CodeInstruction(OpCodes.Mul);                              // multiply bcmer multiplier and 'hiveScrapValue'
                    yield return new CodeInstruction(OpCodes.Call, roundMethod);                // round result to Int32, since 'hiveScrapValue' is an integer
                    yield return new CodeInstruction(OpCodes.Starg, 2);                         // save result to 'hiveScrapValue'
                }
                yield return instruction;
            }
        }
    }
}
