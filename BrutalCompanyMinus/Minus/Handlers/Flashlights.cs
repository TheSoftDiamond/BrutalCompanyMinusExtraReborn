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
using static BrutalCompanyMinus.Minus.Events.FlashLightsFailure;
using System.IO;


namespace BrutalCompanyMinus.Minus.Handlers
{
    public class FlashlightItemChargerPatches : NetworkBehaviour
    {

        // I literally tried several methods. This one isn't my favorite but... here we are.
        // I really hate this spaghetti code. May I find a better solution one day.
        public static FlashlightItemChargerPatches instance;
        public void Awake()
        {
            if (instance != null) DestroyFlashlightFailure();
            instance = this;
            Net.Instance.SetFlashlightsServerRpc(true);
        }

        public static void DestroyFlashlightFailure() // Delete   
        {
            Events.FlashLightsFailure.Active = false;
            GameObject flashlightObject = GameObject.Find("FlashlightsFailureObject");
            if (flashlightObject != null)
            {
                GameObject.Destroy(flashlightObject);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.ShipLeave))]
        public static void OnShipLeave()
        {
            DestroyFlashlightFailure();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndGameServerRpc))]
        public static void DestroyForGodsSake()
        {
            DestroyFlashlightFailure();
        }
    }
}
