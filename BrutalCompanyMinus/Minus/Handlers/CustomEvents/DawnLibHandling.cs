using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using BrutalCompanyMinus.Minus.CustomEvents;
using Dawn;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrutalCompanyMinus.Minus.Handlers.CustomEvents
{
    public class DawnLibHandling
    {
        public static Queue<GeneralCustomEvent.HazardEvent> eventQueue = new Queue<GeneralCustomEvent.HazardEvent>();

        public static Dictionary<string, DawnSnapshot> originalStates = new Dictionary<string, DawnSnapshot>();

        /// <summary>  
        /// This struct helps provide a template for storing original map object data.  
        /// </summary>  
        public struct DawnSnapshot
        {
            public object OutsideWeights;
            public object InsideWeights;
            public bool FacingAway, FacingWall, BackToWall, BackFlush, ReqDist, DisallowNear;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static bool ProcessHazardEvents()
        {
            bool dataFound = false;
            foreach (MEvent _event in EventManager.currentEvents)
            {
                Log.LogInfo($"Event name: {_event.GetType().Name}");
                if (_event is GeneralCustomEvent customEvent)
                {
                    Log.LogInfo($"Event {customEvent.Name()} is a GeneralCustomEvent, checking hazard events...");
                    foreach (GeneralCustomEvent.HazardEvent hazard in customEvent.hazardEvents)
                    {
                        Log.LogInfo("Processing...");
                        dataFound |= ProcessMapObject(hazard);
                    }
                }
            }

            return dataFound;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static bool IsDawnManaged(string hazardName)
        {
            bool managed = false;
            foreach (DawnMapObjectInfo mapObjectInfo in LethalContent.MapObjects.Values)
            {
                if (mapObjectInfo.ShouldSkipIgnoreOverride() || mapObjectInfo.MapObject.name != hazardName)
                    continue;

                DawnOutsideMapObjectInfo? outsideInfo = mapObjectInfo.OutsideInfo;
                DawnInsideMapObjectInfo? insideInfo = mapObjectInfo.InsideInfo;

                if (outsideInfo != null || insideInfo != null)
                {
                    managed = true;
                    break;
                }
            }
            Log.LogInfo($"{hazardName} managed: {managed}");

            return managed;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static bool ProcessMapObject(GeneralCustomEvent.HazardEvent hazard)
        {
            bool processed = false;
            foreach (DawnMapObjectInfo mapObjectInfo in LethalContent.MapObjects.Values)
            {
                if (mapObjectInfo.ShouldSkipIgnoreOverride() || mapObjectInfo.MapObject.name != hazard.hazardObject.name)
                    continue;

                string hazardName = mapObjectInfo.MapObject.name;

                if (!originalStates.ContainsKey(hazardName))
                {
                    originalStates[hazardName] = new DawnSnapshot
                    {
                        OutsideWeights = mapObjectInfo.OutsideInfo?.SpawnWeights,
                        InsideWeights = mapObjectInfo.InsideInfo?.SpawnWeights,
                        FacingAway = mapObjectInfo.InsideInfo?.SpawnFacingAwayFromWall ?? false,
                        FacingWall = mapObjectInfo.InsideInfo?.SpawnFacingWall ?? false,
                        BackToWall = mapObjectInfo.InsideInfo?.SpawnWithBackToWall ?? false,
                        BackFlush = mapObjectInfo.InsideInfo?.SpawnWithBackFlushAgainstWall ?? false,
                        ReqDist = mapObjectInfo.InsideInfo?.RequireDistanceBetweenSpawns ?? false,
                        DisallowNear = mapObjectInfo.InsideInfo?.DisallowSpawningNearEntrances ?? false
                    };
                }

                DawnOutsideMapObjectInfo? outsideInfo = mapObjectInfo.OutsideInfo;
                DawnInsideMapObjectInfo? insideInfo = mapObjectInfo.InsideInfo;

                NamespacedKey<DawnMoonInfo> moonKey = RoundManager.Instance.currentLevel.GetDawnInfo().TypedKey;

                float computedWeight = UnityEngine.Random.Range(
                    hazard.minDensity.Computef(hazard.Type),
                    hazard.maxDensity.Computef(hazard.Type)
                );
                Log.LogInfo($"Raw computed: {computedWeight}");

                if (outsideInfo != null)
                {
                    computedWeight = (int)Mathf.Clamp(computedWeight * Manager.terrainArea, 0f, 1000f);
                    Log.LogInfo($"Outside adjusted computed: {computedWeight} Area: {Manager.terrainArea}");

                    outsideInfo.SpawnWeights = new CurveTableBuilder<DawnMoonInfo, SpawnWeightContext>()
                        .AddCurve(moonKey, AnimationCurve.Constant(0, 1, computedWeight)).Build();

                    mapObjectInfo.OutsideInfo = outsideInfo;
                    processed = true;
                }

                if (insideInfo != null)
                {
                    int insideWeight = (int)computedWeight;
                    Log.LogInfo("Inside adjusted computed: " + insideWeight);
                    insideInfo.SpawnWeights = new CurveTableBuilder<DawnMoonInfo, SpawnWeightContext>()
                        .AddCurve(moonKey, AnimationCurve.Constant(0, 1, insideWeight))
                        .Build();
                    insideInfo.SpawnFacingAwayFromWall = hazard.facingAwayFromWall;
                    insideInfo.SpawnFacingWall = hazard.facingWall;
                    insideInfo.SpawnWithBackToWall = hazard.backToWall;
                    insideInfo.SpawnWithBackFlushAgainstWall = hazard.backFlushWithWall;
                    insideInfo.RequireDistanceBetweenSpawns = hazard.requireDistanceBetween;
                    insideInfo.DisallowSpawningNearEntrances = hazard.disallowNearEntrances;

                    mapObjectInfo.InsideInfo = insideInfo;
                    processed = true;
                }

                if (processed) break;
            }

            return processed;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static bool OnEventStart()
        {
            return ProcessHazardEvents();
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void OnEventEnd()
        {
            foreach (var entry in originalStates)
            {
                foreach (DawnMapObjectInfo mapObjectInfo in LethalContent.MapObjects.Values)
                {
                    if (mapObjectInfo.MapObject.name == entry.Key)
                    {
                        if (mapObjectInfo.OutsideInfo != null)
                            mapObjectInfo.OutsideInfo.SpawnWeights = (ProviderTable<AnimationCurve?, DawnMoonInfo, SpawnWeightContext>?)entry.Value.OutsideWeights;

                        if (mapObjectInfo.InsideInfo != null)
                        {
                            mapObjectInfo.InsideInfo.SpawnWeights = (ProviderTable<AnimationCurve?, DawnMoonInfo, SpawnWeightContext>?)entry.Value.InsideWeights;
                            mapObjectInfo.InsideInfo.SpawnFacingAwayFromWall = entry.Value.FacingAway;
                            mapObjectInfo.InsideInfo.SpawnFacingWall = entry.Value.FacingWall;
                            mapObjectInfo.InsideInfo.SpawnWithBackToWall = entry.Value.BackToWall;
                            mapObjectInfo.InsideInfo.SpawnWithBackFlushAgainstWall = entry.Value.BackFlush;
                            mapObjectInfo.InsideInfo.RequireDistanceBetweenSpawns = entry.Value.ReqDist;
                            mapObjectInfo.InsideInfo.DisallowSpawningNearEntrances = entry.Value.DisallowNear;
                        }
                        break;
                    }
                }
            }
            originalStates.Clear();
        }
    }
}
