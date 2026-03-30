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
using static BrutalCompanyMinus.Minus.Events.IsMetal;
using System.IO;


namespace BrutalCompanyMinus.Minus.Handlers
{
    public class IsMetalNet : NetworkBehaviour
    {

        // I literally tried several methods. This one isn't my favorite but... here we are.
        // I really hate this spaghetti code. May I find a better solution one day.
        public static IsMetalNet instance;
        public void Awake()
        {
            if (instance != null) DestroyMetalPatch();
            instance = this;
            Net.Instance.SetMetalOnNetServerRpc(true);
        }

        public static void DestroyMetalPatch() // Delete   
        {
            Events.IsMetal.Active = false;
            GameObject IsMetalObj = GameObject.Find("IsMetalObj");
            if (IsMetalObj != null)
            {
                GameObject.Destroy(IsMetalObj);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.ShipLeave))]
        public static void OnShipLeave()
        {
            DestroyMetalPatch();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndGameServerRpc))]
        public static void DestroyForGodsSake()
        {
            DestroyMetalPatch();
        }
    }
}
