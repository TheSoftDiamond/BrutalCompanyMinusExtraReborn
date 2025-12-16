using BrutalCompanyMinus.Minus.Events;
using HarmonyLib;
using static BrutalCompanyMinus.Minus.Events.WalkieFailure;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(WalkieTalkie))]
    internal class WalkieTalkiePatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("ItemActivate")]
        private static bool InterruptItemActivate(WalkieTalkie __instance)
        {
            if (Events.WalkieFailure.Instance.Active)
            {
                __instance.thisAudio.PlayOneShot(__instance.playerDieOnWalkieTalkieSFX);
                __instance.UseUpBatteries();
                return false;
            }
            return true;
        }
    }
}
