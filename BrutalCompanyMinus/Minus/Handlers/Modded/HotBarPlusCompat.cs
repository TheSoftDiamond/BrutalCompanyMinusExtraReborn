using System;
using System.Collections.Generic;
using System.Text;
using GameNetcodeStuff;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Handlers.Modded
{
    internal class HotBarPlusCompat
    {
        private static bool hotbarSet = false;
        private static readonly int baseSize = HotbarPlus.Config.ConfigSettings.hotbarSizeConfig.Value;

        public static void ResizeHotbarRandomly(ref int newSize)
        {
            if (hotbarSet) return; // Hotbar has already been set by another hotbar event instance, ignore this one

            // Generate a new size for the hotbar
            newSize = UnityEngine.Random.Range(HotbarPlus.Networking.SyncManager.currentHotbarSize, 30);
            Log.LogInfo("[HotBarMania] New big hotbar size: " + newSize);

            // Update the hotbar
            UpdateHotbar(newSize);
            newSize += baseSize; // Offset by base hotbar size for the reset operation on ship leave
            hotbarSet = true;
        }

        public static void ResizeHotbarRandomlySmall(ref int newSize)
        {
            if (hotbarSet) return; // Hotbar has already been set by another hotbar event instance, ignore this one

            // Generate a new size for the hotbar
            newSize = -(UnityEngine.Random.Range(1, HotbarPlus.Networking.SyncManager.currentHotbarSize));
            Log.LogInfo("[HotBarMania] New small hotbar size: " + newSize);
            
            // Drop any items
            foreach (PlayerControllerB player in StartOfRound.Instance.allPlayerScripts)
            {
                if (!player.isPlayerControlled) continue; // Ignore unused player controllers

                for (int i = HotbarPlus.Networking.SyncManager.currentHotbarSize + newSize; i < HotbarPlus.Networking.SyncManager.currentHotbarSize; i++)
                {
                    HotbarPlus.Patches.PlayerPatcher.CallSwitchToItemSlot(player, i);
                    player.DiscardHeldObject();
                }
            }

            // Update the hotbar
            UpdateHotbar(newSize);
            newSize += baseSize; // Offset by base hotbar size for the reset operation on ship leave
            hotbarSet = true;
        }
        
        public static void ResetHotbar(ref int newSize)
        {
            if (!hotbarSet) return; // Hotbar has already been reset, ignore this call

            int prevSize = newSize;
            newSize = HotbarPlus.Networking.SyncManager.currentHotbarSize - newSize;
            Log.LogInfo("[HotBarMania] New reset hotbar size: " + newSize);

            //We need to drop items that are in the extra slots if any to prevent items from being stuck in the inventory
            if (HotbarPlus.Networking.SyncManager.currentHotbarSize - baseSize < prevSize)
            {
                foreach (PlayerControllerB player in StartOfRound.Instance.allPlayerScripts)
                {
                    if (!player.isPlayerControlled) continue; // Ignore unused player controllers

                    for (int i = newSize; i < prevSize; i++)
                    {
                        HotbarPlus.Patches.PlayerPatcher.CallSwitchToItemSlot(player, i);
                        player.DiscardHeldObject();
                    }
                }
            }

            //Finally Reset the hotbar
            UpdateHotbar(newSize);
            hotbarSet = false;
        }

        private static void UpdateHotbar(int newSize)
        {
            try
            {
                Log.LogInfo("[HotBarMania] Attempting to set hotbar remotely");
                HotbarPlus.Networking.SyncManager.SendPurchaseHotbarSlotToServer(newSize);
            }
            catch (Exception e)
            {
                Log.LogError("[HotBarMania] Unable to set hotbar remotely: " + e.Message);
            }
        }
    }
}
