using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
//using UnityEngine;
using BrutalCompanyMinus.Minus.Events;
using static BrutalCompanyMinus.Minus.Events.ISP;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;
using GameNetcodeStuff;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("Update")]
        private static IEnumerator FlickerISPH(RoundManager __instance)
        {
            // Interrupt the walkie if a distortion malfunction has triggered.
            while (ISPNet.Value == 0) 
            {
                yield return new WaitForSeconds(2f);
                __instance.FlickerLights(true);
                break;
            }
            
           
        }
    }
}
