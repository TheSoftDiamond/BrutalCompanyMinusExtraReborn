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
using static BrutalCompanyMinus.Minus.Events.NotMetal;
using System.IO;


namespace BrutalCompanyMinus.Minus.Handlers
{
    public class NotMetalNet : NetworkBehaviour
    {

        // I literally tried several methods. This one isn't my favorite but... here we are.
        // I really hate this spaghetti code. May I find a better solution one day.
        public static NotMetalNet instance;
        public void Awake()
        {
            if (instance != null) DestroyNotMetalPatch();
            instance = this;
            Net.Instance.SetMetalOffNetServerRpc(true);
        }

        public static void DestroyNotMetalPatch() // Delete   
        {
            Events.NotMetal.Active = false;
            GameObject NotMetalObj = GameObject.Find("NotMetal");
            if (NotMetalObj != null)
            {
                GameObject.Destroy(NotMetalObj);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.ShipLeave))]
        public static void OnShipLeave()
        {
            DestroyNotMetalPatch();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndGameServerRpc))]
        public static void DestroyForGodsSake()
        {
            DestroyNotMetalPatch();
        }
    }
}
