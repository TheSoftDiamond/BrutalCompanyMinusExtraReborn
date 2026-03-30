using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BrutalCompanyMinus.Minus.Events;
using static BrutalCompanyMinus.Minus.Events.NotMetal;
using Object = UnityEngine.Object;
using Steamworks.Ugc;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch]
    public class AllMetalPatches
    {
        public static Dictionary<GrabbableObject, bool> OriginalStates = new Dictionary<GrabbableObject, bool>();

        /// <summary>
        /// This patch is for the Metal Off Event.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GrabbableObject), "Start")]
        public static void MetalOffFix(GrabbableObject __instance)
        {
            if (!Events.NotMetal.Active) return;

            ApplyMetalStates(__instance, false);
        }

        /// <summary>
        /// This patch is for the Metal On Event.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GrabbableObject), "Start")]
        public static void MetalOnFix(GrabbableObject __instance)
        {
            if (!Events.IsMetal.Active) return;

            ApplyMetalStates(__instance, true);
        }

        /// <summary>
        /// This patch is for the Metal Switch Event. Where it takes the opposite state
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GrabbableObject), "Start")]
        public static void MetalSwitch(GrabbableObject __instance)
        {
            if (!Events.MetalSwitch.Active) return;

            if (__instance != null && __instance.itemProperties != null)
            {
                ApplyMetalStates(__instance, !__instance.itemProperties.isConductiveMetal);
            }
        }

        /// <summary>
        /// This patch is for the Metal Off Event.
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), "OpenShipDoors")]
        public static void MetalOffSweep()
        {
            if (!Events.NotMetal.Active) return;

            GrabbableObject[] allItems = Object.FindObjectsOfType<GrabbableObject>();

            foreach (var item in allItems)
            {
                ApplyMetalStates(item, false);
            }
        }

        /// <summary>
        /// This patch is for the Metal On Event.
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), "OpenShipDoors")]
        public static void MetalOnSweep()
        {
            if (!Events.IsMetal.Active) return;

            GrabbableObject[] allItems = Object.FindObjectsOfType<GrabbableObject>();

            foreach (var item in allItems)
            {
                ApplyMetalStates(item, true);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), "OpenShipDoors")]
        public static void MetalSwitchSweep()
        {
            if (!Events.MetalSwitch.Active) return;
            GrabbableObject[] allItems = Object.FindObjectsOfType<GrabbableObject>();
            foreach (var item in allItems)
            {
                if (item != null && item.itemProperties != null)
                {
                    ApplyMetalStates(item, !item.itemProperties.isConductiveMetal);
                }
            }
        }

        /// <summary>
        /// This method applies the metal state to a given item and stores the original state if it hasn't been stored already.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="state"></param>
        public static void ApplyMetalStates(GrabbableObject item, bool state)
        {
            if (item == null || item.itemProperties == null) return;

            // This is here to prevent the original item properties from being overwritten
            item.itemProperties = Object.Instantiate(item.itemProperties);

            if (!OriginalStates.ContainsKey(item))
            {
                OriginalStates.Add(item, item.itemProperties.isConductiveMetal);
                if (Configuration.ExtraLogging.Value)
                {
                    Log.LogDebug($"Stored original state for item: {item.name}, isConductiveMetal: {item.itemProperties.isConductiveMetal}");
                }
            }

            // Set the item to not be conductive metal
            item.itemProperties.isConductiveMetal = state;
        }


        /// <summary>
        /// This method restores the original states of all items that were affected by the Metal Events.
        /// </summary>
        public static void RestoreOriginalStates()
        {
            foreach (var itemEntry in OriginalStates)
            {
                if (itemEntry.Key != null && itemEntry.Key.itemProperties != null)
                {
                    itemEntry.Key.itemProperties.isConductiveMetal = itemEntry.Value;
                    if (Configuration.ExtraLogging.Value)
                    {
                        Log.LogMessage($"Restored item: {itemEntry.Key.name} to original isConductiveMetal: {itemEntry.Value}");
                    }
                }
            }

            OriginalStates.Clear();
        }

    }
}
