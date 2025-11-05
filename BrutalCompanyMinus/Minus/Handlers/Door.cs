using HarmonyLib;
using UnityEngine;
using static BrutalCompanyMinus.Minus.Events.DoorFailure;
using static BrutalCompanyMinus.Minus.Events.DoorCircuitFailure;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(HangarShipDoor))]
    internal class HangarShipDoorPatches
    {
        private static float lockdownTime;

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void OverwriteDoorPower(HangarShipDoor __instance)
        {
            if (DoorUnityNet.Value == true)
            { 
                __instance.doorPower = 0f;
                __instance.SetDoorOpen();
                __instance.buttonsEnabled = false;
                lockdownTime = (lockdownTime + Time.deltaTime) % 6;
                __instance.doorPowerDisplay.text = $"{(lockdownTime > 3 ? "FAILURE" : "RETRYING")}";
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void OverwriteDoorState(HangarShipDoor __instance)
        {
            if (CircuitUnityNet.Value == true)
            {
                if (/*TimeOfDay.Instance.hour == 4 | TimeOfDay.Instance.hour == 5 | TimeOfDay.Instance.hour == 6 | TimeOfDay.Instance.hour == 7 
                    | TimeOfDay.Instance.hour == 8 |*/ TimeOfDay.Instance.hour == 9 | TimeOfDay.Instance.hour == 10 | TimeOfDay.Instance.hour == 11 
                    | TimeOfDay.Instance.hour == 12 | TimeOfDay.Instance.hour == 13 | TimeOfDay.Instance.hour == 14 | TimeOfDay.Instance.hour == 15)
                {
                    __instance.doorPower = 1f;
                    __instance.PlayDoorAnimation(true);
                    __instance.buttonsEnabled = false;
                    lockdownTime = (lockdownTime + Time.deltaTime) % 6;
                    __instance.doorPowerDisplay.text = $"{(lockdownTime > 3 ? "LOCKED" : "OPEN 10PM")}";
                }
                else if (TimeOfDay.Instance.hour == 16)
                {
                    __instance.PlayDoorAnimation(false);
                    __instance.buttonsEnabled = true;
                }
            }
        }
    }
}
