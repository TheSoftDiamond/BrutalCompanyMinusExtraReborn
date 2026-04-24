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
using static BrutalCompanyMinus.Minus.Events.KidnapperFox;
using System.IO;


namespace BrutalCompanyMinus.Minus.Handlers
{
    public class kidnapperFoxNet : NetworkBehaviour
    {

        // I literally tried several methods. This one isn't my favorite but... here we are.
        // I really hate this spaghetti code. May I find a better solution one day.
        public static kidnapperFoxNet instance;
        public void Awake()
        {
            if (instance != null) DestroyKidnapperNet();
            instance = this;
            Net.Instance.SetKidnapperFoxNetServerRpc(true);
        }

        public static void DestroyKidnapperNet() // Delete   
        {
            Events.KidnapperFox.Active = false;
            GameObject KidnapperFoxNet = GameObject.Find("KidnapperFoxNet");
            if (KidnapperFoxNet != null)
            {
                GameObject.Destroy(KidnapperFoxNet);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.ShipLeave))]
        public static void OnShipLeave()
        {
            DestroyKidnapperNet();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndGameServerRpc))]
        public static void DestroyForGodsSake()
        {
            DestroyKidnapperNet();
        }
    }
}
