using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Handlers.Modded
{
    internal class HotBarPlusCompat
    {
        private static bool hotbarSet = false;

        public static void ResizeHotbarRandomly(ref int newSize)
        {
            if (hotbarSet) return; // Hotbar has already been set by another hotbar event instance, ignore this one

            // Generate a new size for the hotbar
            newSize = UnityEngine.Random.Range(HotbarPlus.Networking.SyncManager.currentHotbarSize, 30);
            Log.LogInfo("[HotBarMania] New big hotbar size: " + newSize);

            // Update the hotbar
            UpdateHotbar(newSize);
            newSize += HotbarPlus.Config.ConfigSettings.hotbarSizeConfig.Value; // Offset by base hotbar size for the reset operation on ship leave
            hotbarSet = true;
        }

        public static void ResizeHotbarRandomlySmall(ref int newSize)
        {
            if (hotbarSet) return; // Hotbar has already been set by another hotbar event instance, ignore this one

            // Generate a new size for the hotbar
            newSize = -(UnityEngine.Random.Range(1, HotbarPlus.Networking.SyncManager.currentHotbarSize));
            Log.LogInfo("[HotBarMania] New small hotbar size: " + newSize);

            // Update the hotbar
            UpdateHotbar(newSize);
            newSize += HotbarPlus.Config.ConfigSettings.hotbarSizeConfig.Value; // Offset by base hotbar size for the reset operation on ship leave
            hotbarSet = true;
        }
        
        public static void ResetHotbar(ref int newSize)
        {
            if (!hotbarSet) return; // Hotbar has already been reset, ignore this call

            newSize = HotbarPlus.Networking.SyncManager.currentHotbarSize - newSize;
            Log.LogInfo("[HotBarMania] New reset hotbar size: " + newSize);

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
