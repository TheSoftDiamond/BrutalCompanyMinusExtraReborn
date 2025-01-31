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
using com.github.zehsteam.ZombiesPlush;


namespace BrutalCompanyMinus.Minus.Handlers
{
    /*  [HarmonyPatch(typeof(Takey))]
      public class Patches : Takey
      {

          [HarmonyPrefix]
          [HarmonyPatch("Awake")]
          public static void VariantAwake()
          {


              if (!NetworkUtils.IsServer)
              {
                  com.github.zehsteam.TakeyPlush.MonoBehaviours.Takey takey = new com.github.zehsteam.TakeyPlush.MonoBehaviours.Takey();
                  // takey.VariantDataList.GetIndex(TakeyVariantType.DinkDonk);
                  //TakeyVariantsLibrary.TakeyLib();
                  takey.VariantIndex = TakeyDinkDonk;
              }


          }


        /*  public static void Test()
          {
              // Takey GrabbableObject instance.
              Takey takey = new Takey();


              int variantIndex = GetTakeyVariantIndex(takey, TakeyVariantType.DinkDonk);
          }

          public static int GetTakeyVariantIndex(Takey takey, TakeyVariantType variantType)
          {
              for (int i = 0; i < takey.VariantDataList.Variants.ToArray().Length; i++)
              {
                  if (takey.VariantDataList.Variants[i].VariantType == variantType)
                  {
                      return i;
                  }
              }

              return 0; // Default variant.
          }*/

    /* public static void Test()
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

    // }

    [HarmonyPatch(typeof(Takey))]
    public class PatchV2 : Takey
    {
      /*  [HarmonyPrefix]
        [HarmonyPatch("Awake")]*/
        public override void Awake()
        {
             base.Awake();

            if (NetworkUtils.IsServer) 
            { 
                

        
                
                    VariantIndex = VariantDataList.GetIndex(TakeyVariantType.DinkDonk);
               // VariantType.Equals(TakeyVariantType.DinkDonk);// = TakeyVariantType.DinkDonk;
            }
        }

    }

   /* public class TakeyVariantsLibrary
    {
       
       public static List<int> TakeyLib()
       {
            Takey takey = new com.github.zehsteam.TakeyPlush.MonoBehaviours.Takey();


            
            
            int DinkDonk = takey.VariantDataList.GetIndex(TakeyVariantType.DinkDonk);
            int Stabby = takey.VariantDataList.GetIndex(TakeyVariantType.Stabby);
            int Cake = takey.VariantDataList.GetIndex(TakeyVariantType.Cake);
            int Gamble = takey.VariantDataList.GetIndex(TakeyVariantType.Gamble);
            int Gazmi = takey.VariantDataList.GetIndex(TakeyVariantType.Gazmi);
            int Aloo = takey.VariantDataList.GetIndex(TakeyVariantType.ALOO);
            int Captain = takey.VariantDataList.GetIndex(TakeyVariantType.Captain);
            int Gift = takey.VariantDataList.GetIndex(TakeyVariantType.Gift);
            int Chicken = takey.VariantDataList.GetIndex(TakeyVariantType.ChickenDance);
            int Cute = takey.VariantDataList.GetIndex(TakeyVariantType.Cute);
            int Feels = takey.VariantDataList.GetIndex(TakeyVariantType.Feels);
            int FightClub = takey.VariantDataList.GetIndex(TakeyVariantType.FightClub);
            int Lubbers = takey.VariantDataList.GetIndex(TakeyVariantType.LUBBERS);
            int Shady = takey.VariantDataList.GetIndex(TakeyVariantType.Shady);
         var ints = new List<int>[DinkDonk,Stabby,Cake,Gamble,Gazmi,Aloo,Captain,Gift,Chicken,Cute,Feels,FightClub,Lubbers,Shady];
            return ints[DinkDonk, Stabby, Cake, Gamble, Gazmi, Aloo, Captain, Gift, Chicken, Cute, Feels, FightClub, Lubbers, Shady];
            
       }
    }*/

}
