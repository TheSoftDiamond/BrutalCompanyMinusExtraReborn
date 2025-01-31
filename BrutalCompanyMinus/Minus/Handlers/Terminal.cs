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
    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatches
    {
        // Block the user from confirming a moon node.
        [HarmonyPrefix]
        [HarmonyPatch("OnSubmit")]
        private static bool BlockMoonConfirmNode(/*Terminal __instance, TerminalNode node*/)
        {
            // Make sure we disallow the result of the action if the navigation malfunction is active.
            if (TerminalNet.Value == 0)
            {

                // This checks if the current node has selected a moon. By default the value is < 0 if not.
                //  if (node.buyRerouteToMoon > -1)
                //  {
                // Update the display text and play a sound.
              /*  node.displayText =
                    "TERMINAL ERROR: ORDER BROADCAST FAILED\n\nFAILED TO BROADCAST ORDER COMMAND TO THE COMPANY STORE SERVICE DUE TO SHIP COMMS MALFUNCTION\n\n";

                node.terminalEvent = "ERROR";

                __instance.PlayTerminalAudioServerRpc(3);

                __instance.LoadNewNode(node);




                //     return false;
                //  }
                __instance.orderedItemsFromTerminal.Capacity = 0;
                //   node.buyUnlockable = false;
                __instance.useCreditsCooldown = true;
                __instance.numberOfItemsInDropship = 0;
                __instance.buyableItemsList.SetValue(null, 0);
                __instance.moonsCatalogueList.SetValue(null, 0);
                //   FacilityMeltdown.API.MeltdownAPI.MeltdownStarted
                //   FacilityMeltdown.API.MeltdownAPI.StartMeltdown("BrutalCompanyMinusExtra");*/

                return false;


            }

            return true;
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
