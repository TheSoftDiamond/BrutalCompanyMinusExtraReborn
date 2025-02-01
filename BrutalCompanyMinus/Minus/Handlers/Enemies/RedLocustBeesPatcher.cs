using HarmonyLib;
using UnityEngine;
using Unity.Netcode;

namespace BrutalCompanyMinus.Minus.Handlers.Enemies
{
    [HarmonyPatch(typeof(RedLocustBees))]
    internal static class RedLocustBeesPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(RedLocustBees.SpawnHiveClientRpc))]
        internal static void IncreaseHiveScrapValue(NetworkObjectReference hiveObject, ref int hiveScrapValue, Vector3 hivePosition)
        {
            hiveScrapValue = Mathf.RoundToInt(hiveScrapValue * Manager.scrapValueMultiplier);
        }
    }
}
