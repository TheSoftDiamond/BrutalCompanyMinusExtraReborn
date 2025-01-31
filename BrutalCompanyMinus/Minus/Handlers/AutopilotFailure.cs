using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BrutalCompanyMinus.Minus.Events;
using GameNetcodeStuff;
using System.Linq;
using static BrutalCompanyMinus.Minus.Events.TerminalFailure;
using UnityEngine.UI;
using UnityEngine.Assertions.Must;
//using static BrutalCompanyMinus.Modules.MeltdownEffects;
//using BrutalCompanyMinus.Modules;


namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(TerminalAccessibleObject))]
    internal class StartOfRoundPatches
    {
        // Block the user from confirming a moon node.
        [HarmonyPrefix]
        [HarmonyPatch("Update")]
        private static void PreventShipLeave(TerminalAccessibleObject __instance)
        {
            // Make sure we disallow the result of the action if the navigation malfunction is active.
            if (TerminalNet.Value == 0)
            {
                if (__instance.isBigDoor == true)
                { 
                    __instance.SetDoorOpen(false);
                }
                

              //  return false;


            }

           // return true;
        }
    }   // Block the user from confirming a moon node.

   /* [HarmonyPatch(typeof(StartOfRound))]
    internal class TerminalPatchesNew
    {


        [HarmonyPrefix]
        [HarmonyPatch("StartGame")]
        private static void KillTerminal(Terminal __instance)
        {
            // Make sure we disallow the result of the action if the navigation malfunction is active.
            if (TerminalNet.Value == 0)
            {
                //  node.displayText =
                //      "TERMINAL ERROR: ORDER BROADCAST FAILED\n\nFAILED TO BROADCAST ORDER COMMAND TO THE COMPANY STORE SERVICE DUE TO SHIP COMMS MALFUNCTION\n\n";

                // node.terminalEvent = "ERROR";
                BCMECodeSecurityCheck.Modules.EventRelatedStuff.KillTerminalEvent();
                
              //  __instance.PlayTerminalAudioServerRpc(3);

             //   __instance.LoadNewNode(node);
             //   __instance.QuitTerminal(true);
             //   __instance.
             /*   __instance.useCreditsCooldown = true;
                __instance.numberOfItemsInDropship = 0;
                __instance.buyableItemsList.SetValue(null, 0);
                __instance.moonsCatalogueList.SetValue(null, 0);*/
           /* }


        }
    }*/
}
