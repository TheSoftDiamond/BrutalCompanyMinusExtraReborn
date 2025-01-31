using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BrutalCompanyMinus.Minus.Events;
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
            // Interrupt the teleporter if the teleporter or power malfunction has triggered.
            if (TeleportNet.Value == 0)
            {
                // Still do the animation and play the sound.
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
            // When the distortion malfunction is active, set the teleporting player to a random one.
            if (TargetingNet.Value == 0)
            {
                PlayerControllerB[] players = GameObject
                    .FindObjectsByType<PlayerControllerB>(FindObjectsSortMode.None)
                    .Where(player => player.isPlayerControlled)
                    .ToArray();

                if (players.Length > 0)
                {
                    // Select a random player.
                    StartOfRound.Instance.mapScreen.targetedPlayer = players[
                        Random.Range(0, players.Length)
                    ];
                }
            }
        }
    }
}
