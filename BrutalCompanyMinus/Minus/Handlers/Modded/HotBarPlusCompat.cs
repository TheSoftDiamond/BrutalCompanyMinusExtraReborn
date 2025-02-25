using GameNetcodeStuff;
using HarmonyLib;
using HotbarPlus.Networking;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Handlers.Modded
{
    internal class HotBarPlusCompat
    {
        private static int _defaultHotbarSize => SyncManager.hotbarSize + SyncManager.purchasedHotbarSlots;
        private static int _currentHotbarSize => SyncManager.currentHotbarSize;

        private static int _overrideSlots = 0;

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void PatchAll(Harmony harmony)
        {
            ApplyCurrentHotbarSizePatch(harmony);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void ResizeHotbarRandomly()
        {
            if (_overrideSlots > 0) return;

            int newSize = (_defaultHotbarSize >= 20) ? _defaultHotbarSize : Random.Range(_defaultHotbarSize, 20);

            Log.LogInfo("[HotBarMania] New big hotbar size: " + newSize);

            OverrideHotbarSlots(newSize);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void ResizeHotbarRandomlySmall()
        {
            if (_overrideSlots > 0) return;

            int newSize = Random.Range(1, _defaultHotbarSize);

            Log.LogInfo("[HotBarMania] New small hotbar size: " + newSize);

            OverrideHotbarSlots(newSize);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void OverrideHotbarSlots(int size)
        {
            if (size <= 0)
            {
                if (_defaultHotbarSize < _currentHotbarSize)
                {
                    DropExtraItems(newSize: _defaultHotbarSize);
                }
            }
            else if (size < _currentHotbarSize)
            {
                DropExtraItems(newSize: size);
            }
            
            _overrideSlots = Mathf.Max(size, 0);
            SyncManager.OnUpdateHotbarSize();
        }

        private static void DropExtraItems(int newSize)
        {
            PlayerControllerB playerScript = GameNetworkManager.Instance.localPlayerController;
            if (playerScript == null) return;

            int slots = playerScript.ItemSlots.Length;
            if (newSize >= slots) return;

            for (int i = slots - 1; i >= newSize; i--)
            {
                if (i > playerScript.ItemSlots.Length - 1)
                {
                    Log.LogWarning($"DropExtraItems(); Index {i} is out of range. Slots: {slots}");
                    continue;
                }

                if (playerScript.ItemSlots[i] != null)
                {
                    playerScript.SwitchToItemSlot(i, null);
                    playerScript.DiscardHeldObject();
                }
            }
        }

        public static void ResetHotbar()
        {
            OverrideHotbarSlots(0);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void ApplyCurrentHotbarSizePatch(Harmony harmony)
        {
            var originalMethod = AccessTools.PropertyGetter(typeof(SyncManager), nameof(SyncManager.currentHotbarSize));
            var prefixMethod = AccessTools.Method(typeof(HotBarPlusCompat), nameof(CurrentHotbarSizePatch));

            if (originalMethod != null && prefixMethod != null)
            {
                harmony.Patch(originalMethod, prefix: new HarmonyMethod(prefixMethod));
                Log.LogInfo("Successfully patched SyncManager.currentHotbarSize.");
            }
            else
            {
                Log.LogError("Failed to locate methods for conditional patching.");
            }
        }

        private static bool CurrentHotbarSizePatch(ref short __result)
        {
            if (_overrideSlots <= 0)
            {
                return true;
            }

            __result = (short)_overrideSlots;
            return false;
        }
    }
}
