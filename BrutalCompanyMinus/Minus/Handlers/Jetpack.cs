using HarmonyLib;
using static BrutalCompanyMinus.Minus.Events.JetpackFailure;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(JetpackItem))]
    internal class JetpackItemPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void PreventItemActivation(JetpackItem __instance)
        {
            if (JetpackUnityNet.Value == true)
            {
                __instance.insertedBattery.empty = true;
            }
        }
    }
}
