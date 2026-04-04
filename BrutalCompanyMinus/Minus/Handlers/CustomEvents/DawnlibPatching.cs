using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using HarmonyLib;

namespace BrutalCompanyMinus.Minus.Handlers.CustomEvents
{
    [HarmonyPatch]
    internal class DawnlibPatching
    {
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        [HarmonyPatch(typeof(RoundManager), "FinishGeneratingLevel")]
        [HarmonyPrefix]
        static void DawnFixMethod()
        {
            if (Compatibility.DawnLibPresent)
            {
                Log.LogInfo($"Area gen: {Manager.terrainArea}");
                if (Manager.terrainArea == 0f)
                {
                    Manager.SampleMap();
                }

                while (DawnLibHandling.eventQueue.Count > 0)
                {
                    Log.LogInfo("Items left: " + DawnLibHandling.eventQueue.Count);
                    DawnLibHandling.ProcessMapObject(DawnLibHandling.eventQueue.Dequeue());
                }
            }
        }
    }
}
