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
using static BrutalCompanyMinus.Minus.Events.SlimeInside;
using System.IO;


namespace BrutalCompanyMinus.Minus.Handlers
{
    public class SlimeInsideNet : NetworkBehaviour
    {

        // I literally tried several methods. This one isn't my favorite but... here we are.
        // I really hate this spaghetti code. May I find a better solution one day.
        public static SlimeInsideNet instance;
        public void Awake()
        {
            if (instance != null) DestroySlime();
            instance = this;
            Net.Instance.SetSlimeServerRpc(true);
        }

        public static void DestroySlime() // Delete   
        {
            Events.SlimeInside.Active = false;
            GameObject SlimeObj = GameObject.Find("SlimeInsideObj");
            if (SlimeObj != null)
            {
                GameObject.Destroy(SlimeObj);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.ShipLeave))]
        public static void OnShipLeave()
        {
            DestroySlime();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndGameServerRpc))]
        public static void DestroyForGodsSake()
        {
            DestroySlime();
        }
    }
}
