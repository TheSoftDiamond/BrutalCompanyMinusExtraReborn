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
using static BrutalCompanyMinus.Minus.Events.LockedEntrance;
using System.IO;


namespace BrutalCompanyMinus.Minus.Handlers
{
    public class DoorLockPatches : NetworkBehaviour
    {

        // I literally tried several methods. This one isn't my favorite but... here we are.
        // I really hate this spaghetti code. May I find a better solution one day.
        public static DoorLockPatches instance;
        public void Awake()
        {
            if (instance != null) DestroyDoorLockPatches();
            instance = this;
            Net.Instance.SetEntranceServerRpc(true);
        }

        public static void DestroyDoorLockPatches() // Delete   
        {
            Events.LockedEntrance.Active = false;
            GameObject LockEntranceObject = GameObject.Find("LockEntranceObject");
            if (LockEntranceObject != null)
            {
                GameObject.Destroy(LockEntranceObject);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.ShipLeave))]
        public static void OnShipLeave()
        {
            DestroyDoorLockPatches();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndGameServerRpc))]
        public static void DestroyForGodsSake()
        {
            DestroyDoorLockPatches();
        }
    }
}
