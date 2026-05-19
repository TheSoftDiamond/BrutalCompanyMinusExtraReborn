using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;
using System.Collections;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch]
    internal class LockedDoorsHandler
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(RoundManager), "RefreshEnemiesList")]
        private static void OnRefreshEnemiesList()
        {
            if (Events.LockedDoors.Active && NetworkManager.Singleton.IsServer)
            {
                RoundManager.Instance.StartCoroutine(CloseAll());
                Events.LockedDoors.Active = false;
            }
        }

        private static IEnumerator CloseAll()
        {
            yield return new WaitForSeconds(8.0f);
            Net.Instance.LockAndCloseAllDoorsServerRpc();
        }
    }
}
