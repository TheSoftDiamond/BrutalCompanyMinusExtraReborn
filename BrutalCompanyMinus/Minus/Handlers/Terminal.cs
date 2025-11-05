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
}
