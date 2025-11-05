using HarmonyLib;
using static BrutalCompanyMinus.Minus.Events.DoorOverdriveEv;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(HangarShipDoor))]
    internal class HangarShipDoorOvPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void OverwriteDoorPower(HangarShipDoor __instance)
        {
            if (DoorOvUnityNet.Value == true)
            { 
                __instance.doorPower = 1f;
                __instance.buttonsEnabled = true;
            }
        }
    }
}
