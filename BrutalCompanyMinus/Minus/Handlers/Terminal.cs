using BrutalCompanyMinus.Minus.Events;
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
            if (Events.TerminalFailure.Instance.Active)
            {
                HUDManager.Instance.UIAudio.PlayOneShot(
                    HUDManager.Instance.radiationWarningAudio,
                    1f);
                return false;
            }
            return true;
        }
    }  
}
