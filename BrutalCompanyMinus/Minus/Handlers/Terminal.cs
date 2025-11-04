using HarmonyLib;
using static BrutalCompanyMinus.Minus.Events.TerminalFailure;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("OnSubmit")]
        private static bool BlockMoonConfirmNode()
        {
            if (TerminalUnityNet.Value == true)
            {
                return false;
            }
            return true;
        }
    }  

   /* [HarmonyPatch(typeof(StartOfRound))]
    internal class TerminalPatchesNew
    {


        [HarmonyPrefix]
        [HarmonyPatch("StartGame")]
        private static void KillTerminal(Terminal __instance)
        {
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

}
