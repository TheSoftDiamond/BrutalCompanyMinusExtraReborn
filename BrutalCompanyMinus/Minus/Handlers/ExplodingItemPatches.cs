using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BrutalCompanyMinus.Minus.Events;
using Object = UnityEngine.Object;
using Steamworks.Ugc;
using Unity.Netcode;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch]
    public class ExplodingItemPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(NetworkObject), "Awake")]
        private static void NetworkObjectAwakePrefix(NetworkObject __instance)
        {
            if (__instance.TryGetComponent(out GrabbableObject item))
                ApplyExplosiveStates(item);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.Start))]
        private static void GrabbableObjectStartPostfix(GrabbableObject __instance)
        {
            int seed = StartOfRound.Instance.randomMapSeed + (int)__instance.NetworkObject.NetworkObjectId;
            System.Random seeded = new System.Random(seed);

            int roll = seeded.Next(1, 101);

            if (roll > ExplodingItems.AmountValue)
                return;

            ApplyExplosiveStates(__instance);
        }

        /// <summary>
        /// This method applies the explosive state to a given item.
        /// </summary>
        public static void ApplyExplosiveStates(GrabbableObject item)
        {
            if (!ExplodingItems.Active || item == null)
                return;

            if (item.isInShipRoom || item.isInElevator || !item.itemProperties.isScrap || item is GrabbableLandmine || item.GetComponent<ExplodingItemsNetScript>() != null)
                return;

            item.gameObject.AddComponent<ExplodingItemsNetScript>();
        }

        // Patch from GrabbableLandmine, to explode nearby items - could be combined with existing patch to reduce duplicate code later
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Landmine), nameof(Landmine.SpawnExplosion))]
        private static void ChainExplodingItems(Vector3 explosionPosition, float damageRange)
        {
            if (!ExplodingItems.Active || !RoundManager.Instance.IsServer)
                return;

            ExplodingItemsNetScript[] items = Object.FindObjectsByType<ExplodingItemsNetScript>(FindObjectsSortMode.None);

            for (int i = 0; i < items.Length; i++)
            {
                ExplodingItemsNetScript item = items[i];
                float distance = Vector3.Distance(explosionPosition, item.transform.position);

                if (item.HasExploded || distance >= damageRange || distance >= 6f)
                    continue;

                if (Physics.Linecast(explosionPosition, item.transform.position + (Vector3.up * 0.3f), out RaycastHit hitInfo, 1073742080, QueryTriggerInteraction.Ignore) && (hitInfo.collider.gameObject.layer == 30 || distance > 4f))
                    continue;

                item.StartCoroutine(item.TriggerOtherMineDelayed());
            }
        }
    }
}
