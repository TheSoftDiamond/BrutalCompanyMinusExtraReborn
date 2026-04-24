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
using static BrutalCompanyMinus.Minus.Events.CruiserFailure;
using System.IO;


namespace BrutalCompanyMinus.Minus.Handlers
{
    public class ScanVanNet : NetworkBehaviour
    {

        // I literally tried several methods. This one isn't my favorite but... here we are.
        // I really hate this spaghetti code. May I find a better solution one day.
        public static ScanVanNet instance;
        public void Awake()
        {
            if (instance != null) DestroyCruiserFailure();
            instance = this;
            Net.Instance.CruiserFailureServerRpc(true);
        }

        public static void DestroyCruiserFailure() // Delete   
        {
            Events.CruiserFailure.Active = false;
            GameObject netObject = GameObject.Find("ScanVanFailure");
            if (netObject != null)
            {
                GameObject.Destroy(netObject);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.ShipLeave))]
        public static void OnShipLeave()
        {
            DestroyCruiserFailure();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndGameServerRpc))]
        public static void DestroyForGodsSake()
        {
            DestroyCruiserFailure();
        }
    }
}
