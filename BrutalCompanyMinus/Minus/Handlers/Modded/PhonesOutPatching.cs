using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using BrutalCompanyMinus.Minus.Events;
using HarmonyLib;
using Scoops;
using Scoops.misc;
using Scoops.patch;

namespace BrutalCompanyMinus.Minus.Handlers.Modded
{
    internal class PhonesOutPatching
    {
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void PatchAllPhone(Harmony harmony)
        {
            ApplyInterruptPhoneUsage(harmony);
        }

        [HarmonyPrefix]
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void ApplyInterruptPhoneUsage(Harmony harmony)
        {
            var originalMethod = AccessTools.Method(typeof(PlayerPhone), nameof(PlayerPhone.ToggleActive));
            var prefixMethod = AccessTools.Method(typeof(PhonesOutPatching), nameof(InterruptPhoneUsage));

            if (originalMethod != null && prefixMethod != null)
            {
                harmony.Patch(originalMethod, prefix: new HarmonyMethod(prefixMethod));
                Log.LogInfo("Successfully patched PlayerPhone.ToggleActive.");
            }
            else
            {
                Log.LogError("Failed to locate methods for conditional patching.");
            }
        }

        public static bool InterruptPhoneUsage(bool active)
        {
            // Interrupt the phone pickup  
            if (PhonesOut.Active)
            {
                HUDManager.Instance.globalNotificationText.text =
                    "BAD PHONE RECEPTION!!!!";

                HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                HUDManager.Instance.UIAudio.PlayOneShot(
                    HUDManager.Instance.radiationWarningAudio,
                    1f
                );
                return false;
            }

            return true;
        }
    }
}
