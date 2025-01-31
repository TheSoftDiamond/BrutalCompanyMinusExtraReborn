using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BrutalCompanyMinus.Minus.Events;
using Unity.Netcode;
using static BrutalCompanyMinus.Minus.Events.ZombiesPlush;

namespace BrutalCompanyMinus.Minus.Handlers
{
   // [HarmonyPatch(typeof(RoundManager))]
    public class ZombiesShutupPatches
    {
  
        protected private static readonly Harmony harmony = new Harmony(/*GUID*/"BCME-ExternalPatcher");

      /*  [HarmonyPostfix]
        [HarmonyPatch("FinishGeneratingNewLevelClientRpc")]*/
        protected unsafe static void FinishGeneratingNewLevelClientRpc() 
        {
            if (!NetworkManager.Singleton.IsServer) return;

            if (Compatibility.zombiesPlushPresent)
            {
                if (IsZombiesPlushEvent == true)
                {
                    try
                    {
                        foreach (var behaviour in UnityEngine.Object.FindObjectsByType<com.github.zehsteam.ZombiesPlush.MonoBehaviours.ZombiesBehaviour>(FindObjectsSortMode.None))
                        {
                            behaviour.SetIsPlayingMusicOnServer(false);
                        }
                    }
                    catch { throw new MissingComponentException("Failed to call FinishGeneratingNewLevelClientRpc on ZombiesBehaviour"); }
                }
                harmony.Patch(AccessTools.Method("Assembly-CSharp.RoundManager:FinishGeneratingNewLevelClientRpc")/*, null,new HarmonyMethod(HarmonyPostfix)*/);
            }
        }


    } 
}
