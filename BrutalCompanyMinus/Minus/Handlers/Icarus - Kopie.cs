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
using System.IO;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using com.github.zehsteam.TakeyPlush.Enums;
using com.github.zehsteam.TakeyPlush.MonoBehaviours;
using com.github.zehsteam.ZombiesPlush.MonoBehaviours;


namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(StartMatchLever))]
    internal class TakeyPatches
    {

        /* [MethodImpl(MethodImplOptions.InternalCall)]
         private static extern void PlayOneShotHelper(AudioSource source, AudioClip clip, float volumeScale);*/


        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        internal static void IcarusEvacuate(StartMatchLever __instance)
        {

        }

        public static void Test()
        {
            // Takey GrabbableObject instance.
            Takey takey = new Takey();
            takey.
            int variantIndex = GetTakeyVariantIndex(takey, TakeyVariantType.DinkDonk);
            takey.SpawnVariant();
        }

        public static int GetTakeyVariantIndex(Takey takey, TakeyVariantType variantType)
        {
            for (int i = 0; i < takey.Variants.Length; i++)
            {
                if (takey.Variants[i].Type == variantType)
                {
                    return i;
                }
            }

            return 0; // Default variant.
        }

      /*  public static void Test()
        {
            // Takey GrabbableObject instance.
            Takey takey = new Takey();
            takey.Variant.VariantType
            int variantIndex = GetTakeyVariantIndex(takey, TakeyVariantType.DinkDonk);
        }

        public static int GetTakeyVariantIndex(Takey takey, TakeyVariantType variantType)
        {
            for (int i = 0; i < takey.Variants.Length; i++)
            {
                if (takey.Variants[i].Type == variantType)
                {
                    return i;
                }
            }

            return 0; // Default variant.
        }*/

    }
}
    
