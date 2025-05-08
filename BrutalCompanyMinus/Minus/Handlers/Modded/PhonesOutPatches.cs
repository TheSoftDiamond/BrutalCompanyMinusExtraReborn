using HarmonyLib;
using UnityEngine;
using Unity.Netcode;


namespace BrutalCompanyMinus.Minus.Handlers.Modded
{
    public class PhonesOutPatches : NetworkBehaviour
    {

        public static PhonesOutPatches instance;

        public void Awake()
        {
            if (instance != null) DestroyPhonesOut();
            instance = this;
            Net.Instance.SetPhonesOutServerRpc(true);
        }

        public static void DestroyPhonesOut() // Delete   
        {
            Events.PhonesOut.Active = false;
            GameObject PhonesOutObject = GameObject.Find("PhonesOutFailureObject");
            if (PhonesOutObject != null)
            {
                GameObject.Destroy(PhonesOutObject);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.ShipLeave))]
        public static void OnShipLeave()
        {
            DestroyPhonesOut();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndGameServerRpc))]
        public static void DestroyPhonesOutForGodsSake()
        {
            DestroyPhonesOut();
        }
    }
}
