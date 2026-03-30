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
using static BrutalCompanyMinus.Minus.Events.MetalSwitch;
using System.IO;


namespace BrutalCompanyMinus.Minus.Handlers
{
    public class MetalSwitchNet : NetworkBehaviour
    {

        // I literally tried several methods. This one isn't my favorite but... here we are.
        // I really hate this spaghetti code. May I find a better solution one day.
        public static MetalSwitchNet instance;
        public void Awake()
        {
            if (instance != null) DestroyMetalSwitchPatch();
            instance = this;
            Net.Instance.SetMetalSwitchNetServerRpc(true);
        }

        public static void DestroyMetalSwitchPatch() // Delete   
        {
            Events.MetalSwitch.Active = false;
            GameObject MetalSwitchNet = GameObject.Find("MetalSwitch");
            if (MetalSwitchNet != null)
            {
                GameObject.Destroy(MetalSwitchNet);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.ShipLeave))]
        public static void OnShipLeave()
        {
            DestroyMetalSwitchPatch();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndGameServerRpc))]
        public static void DestroyForGodsSake()
        {
            DestroyMetalSwitchPatch();
        }
    }
}
