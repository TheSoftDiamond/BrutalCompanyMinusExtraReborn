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
            {
                    Log.LogDebug("Exploding Item called for " + __instance.name + " with ID " + __instance.NetworkObjectId);

                ApplyExplosiveStates(item);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.Start))]
        private static void GrabbableObjectStartPostfix(GrabbableObject __instance)
        {

                Log.LogDebug("[START] Exploding Item called for " + __instance.name + " with ID " + __instance.NetworkObject.NetworkObjectId);

            ApplyExplosiveStates(__instance);
        }

        /// <summary>
        /// This method applies the explosive state to a given item.
        /// </summary>
        public static void ApplyExplosiveStates(GrabbableObject item)
        {
            if (!ExplodingItems.Instance.Active || item == null)
            {

                    Log.LogDebug("Exploding Item skipped for " + (item != null ? item.name : "null") + " with ID " + (item != null ? item.NetworkObject.NetworkObjectId.ToString() : "null") + " because ExplodingItems is not active or item is null.");
                
                return;
            }

            if (item.isInShipRoom || !item.itemProperties.isScrap || item is GrabbableLandmine || item.GetComponent<ExplodingItemsNetScript>() != null)
            {

                
                    if (item.isInShipRoom)
                        Log.LogDebug("Exploding Item skipped for " + item.name + " with ID " + item.NetworkObject.NetworkObjectId + " because it is in the ship room.");
                    else if (!item.itemProperties.isScrap)
                        Log.LogDebug("Exploding Item skipped for " + item.name + " with ID " + item.NetworkObject.NetworkObjectId + " because it is not scrap.");
                    else if (item is GrabbableLandmine)
                        Log.LogDebug("Exploding Item skipped for " + item.name + " with ID " + item.NetworkObject.NetworkObjectId + " because it is a landmine.");
                    else if (item.GetComponent<ExplodingItemsNetScript>() != null)
                        Log.LogDebug("Exploding Item skipped for " + item.name + " with ID " + item.NetworkObject.NetworkObjectId + " because it already has ExplodingItemsNetScript.");

                
                return;
            }

            int seed = StartOfRound.Instance.randomMapSeed + (int)item.NetworkObject.NetworkObjectId;
            System.Random seeded = new System.Random(seed);

            int roll = seeded.Next(1, 101);

            Log.LogDebug(roll + " rolled for " + item.name + " with ID " + item.NetworkObject.NetworkObjectId + " against threshold of " + ExplodingItems.AmountValue);

            if (roll > ExplodingItems.AmountValue)
                return;

            item.gameObject.AddComponent<ExplodingItemsNetScript>();
        }

        // Patch from GrabbableLandmine, to explode nearby items - could be combined with existing patch to reduce duplicate code later
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Landmine), nameof(Landmine.SpawnExplosion))]
        private static void ChainExplodingItems(Vector3 explosionPosition, float damageRange)
        {
            if (!ExplodingItems.Instance.Active || !RoundManager.Instance.IsServer)
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
