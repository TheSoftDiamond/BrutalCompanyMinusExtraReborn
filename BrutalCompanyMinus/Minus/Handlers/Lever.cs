using HarmonyLib;
using BrutalCompanyMinus.Minus.Events;
using static BrutalCompanyMinus.Minus.Events.LeverFailure;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(StartMatchLever))]
    internal class StartMatchLeverPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("PullLever")]
        private static bool InterruptPullLever(StartMatchLever __instance)
        {
            if (EventManager.sideEvents.Contains(LeverFailure.Instance))
            {
                HUDManager.Instance.globalNotificationText.text =
                    "SHIP CORE DEPLETION:\nAWAIT 12AM EMERGENCY AUTOPILOT";
                HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                HUDManager.Instance.UIAudio.PlayOneShot(
                    HUDManager.Instance.radiationWarningAudio,
                    1f
                );

                Net.Instance.SyncLeverTooltipServerRpc("[No power to hydraulics]");
                return false;

            }
            else if (Events.LeverFailure.Instance.Active)
            {
                HUDManager.Instance.globalNotificationText.text =
                    "SHIP LEVER HYDRAULICS JAM:\nAWAIT 12AM EMERGENCY AUTOPILOT";
                HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                HUDManager.Instance.UIAudio.PlayOneShot(
                    HUDManager.Instance.radiationWarningAudio,
                    1f
                );
                Net.Instance.SyncLeverTooltipServerRpc("[Hydraulics jammed]");

                return false;
            }

            return true;
        }
    }
}