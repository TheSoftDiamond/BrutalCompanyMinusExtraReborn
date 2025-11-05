using HarmonyLib;
using UnityEngine;
using GameNetcodeStuff;
using System.Linq;
using static BrutalCompanyMinus.Minus.Events.TeleporterFailure;
using static BrutalCompanyMinus.Minus.Events.TargetingFailure;


namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(ShipTeleporter))]
    internal class ShipTeleporterPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("PressTeleportButtonOnLocalClient")]
        private static bool InterruptPressTeleportButton(ShipTeleporter __instance)
        {
            if (TeleporterUnityNet.Value == true)
            {
                __instance.buttonAnimator.SetTrigger("press");
                __instance.buttonAudio.PlayOneShot(__instance.buttonPressSFX);

                return false;
            }

            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch("PressTeleportButtonOnLocalClient")]
        private static void RandomizePressTeleportButton()
        {
            if (TargetingUnityNet.Value == true)
            {
                PlayerControllerB[] players = GameObject
                    .FindObjectsByType<PlayerControllerB>(FindObjectsSortMode.None)
                    .Where(player => player.isPlayerControlled)
                    .ToArray();

                if (players.Length > 0)
                {
                    StartOfRound.Instance.mapScreen.targetedPlayer = players[
                        Random.Range(0, players.Length)
                    ];
                }
            }
        }
    }
}
