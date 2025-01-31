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
using static BrutalCompanyMinus.Minus.Events.JetpackFailure;
using System.IO;
//using com.github.zehsteam.TakeyPlush;
//using com.github.zehsteam.TakeyPlush.MonoBehaviours;


namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(JetpackItem))]
    internal class JetpackItemPatches
    {

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void PreventItemActivation(JetpackItem __instance)
        {
            // Overwrite the door power and change the display text.
            if (JetpackNet.Value == 0)
            {
                // Make sure the power is constantly maxed out.
                __instance.insertedBattery.empty = true;
                

            

            }

        }
    }

  /*  [HarmonyPatch(typeof(TakeypackItemBehaviour))]
    internal class TakeypackItemPatches
    {

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void PreventItemActivation(TakeypackItemBehaviour __instance)
        {
            if (Compatibility.takeyPlushPresent)
            {   try
                {
                    // Overwrite the door power and change the display text.
                    if (JetpackNet.Value == 0)
                    {
                        // Make sure the power is constantly maxed out.
                         __instance.insertedBattery.empty = true;


                        

                    }
                }
                catch 
                {
                    Log.LogError("Failed to run TakeypackItemPatches. Aborting!");
                }

            }

        }
    }*/

}
