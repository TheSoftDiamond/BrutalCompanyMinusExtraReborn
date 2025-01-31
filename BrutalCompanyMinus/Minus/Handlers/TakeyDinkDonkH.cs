using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BrutalCompanyMinus.Minus.Events;
using Unity.Netcode;
using static BrutalCompanyMinus.Minus.Events.TakeyPlushDinkDonk;
using com.github.zehsteam.ZombiesPlush.MonoBehaviours;
using com.github.zehsteam.TakeyPlush.Enums;
using com.github.zehsteam.TakeyPlush.MonoBehaviours;
using com.github.zehsteam.TakeyPlush.Data;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class TakeyDinkDonkHPatches :Takey
    {

        [HarmonyPrefix]
        [HarmonyPatch("FinishGeneratingNewLevelClientRpc")]
        public static void SetIndex()
        {
            
            if (!NetworkManager.Singleton.IsServer)
            {
                if (!isTakeyPlushDinkDonk)
                {
                   
              //  int TakeyDinkDonk =  TakeyVariantDataList.GetIndex(TakeyVariantType.DinkDonk);
                }
            }


        }


      /*  public static void Test()
        {
            // Takey GrabbableObject instance.
            Takey takey = new Takey();
            com.github.zehsteam.TakeyPlush.Data.TakeyVariantDataList takeyVariantDataList = new com.github.zehsteam.TakeyPlush.Data.TakeyVariantDataList();
            
            int variantIndex = GetTakeyVariantIndex(takeyVariantDataList, TakeyVariantType.DinkDonk);
        }

        public static int GetTakeyVariantIndex(TakeyVariantDataList takeyVariantDataList, TakeyVariantType variantType)
        {
            for (int i = 0; i < takeyVariantDataList.Variants.ToArray().Length; i++)
            {
                if (takeyVariantDataList.Variants[i].VariantType == variantType)
                {
                    return i;
                }
            }

            return 0; // Default variant.
        }*/
    } 
}
