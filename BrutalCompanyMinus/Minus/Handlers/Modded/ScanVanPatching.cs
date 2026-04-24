using System.Runtime.CompilerServices;
using BrutalCompanyMinus.Minus.Events;
using HarmonyLib;


namespace BrutalCompanyMinus.Minus.Handlers.Modded
{
    internal class ScanVanPatching
    {
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void PatchAllCruiserXL(Harmony harmony)
        {
            ApplyCruiserXLBlock(harmony);
        }

        [HarmonyPrefix]
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void ApplyCruiserXLBlock(Harmony harmony)
        {
            var originalMethod = AccessTools.Method(typeof(CruiserXLController), nameof(CruiserXLController.StartTryCarIgnition));
            var prefixMethod = AccessTools.Method(typeof(ScanVanPatching), nameof(InterruptCruiserXL));

            if (originalMethod != null && prefixMethod != null)
            {
                harmony.Patch(originalMethod, prefix: new HarmonyMethod(prefixMethod));
                Log.LogInfo("Successfully patched Scanvan");
            }
            else
            {
                Log.LogError("Failed to locate methods for conditional patching.");
            }
        }

        public static bool InterruptCruiserXL()
        {
            if (Events.CruiserFailure.Active)
            {
                int randomResponse = UnityEngine.Random.Range(0, 3);
                switch (randomResponse)
                {
                    case 0:
                        HUDManager.Instance.globalNotificationText.text = "This is more complicated than the Cruiser!";
                        break;
                    case 1:
                        HUDManager.Instance.globalNotificationText.text = "It wont work";
                        break;
                    case 2:
                        HUDManager.Instance.globalNotificationText.text = "Did you pass the drivers test first?";
                        break;
                    default:
                        HUDManager.Instance.globalNotificationText.text = "Did you know that this vehicle doesn't work?";
                        break;
                }

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
