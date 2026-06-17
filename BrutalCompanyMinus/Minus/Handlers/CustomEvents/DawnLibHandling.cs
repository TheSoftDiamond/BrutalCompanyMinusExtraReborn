using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BrutalCompanyMinus.Minus.CustomEvents;
using Dawn;
using UnityEngine;

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
            public bool FacingAway, FacingWall, BackToWall, BackFlush, ReqDist, DisallowNear, AllowInMineshaft;
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
                GameObject? mapObject = mapObjectInfo.GetMapObjectPrefab();
                if (mapObjectInfo.ShouldSkipIgnoreOverride() || mapObject == null || mapObject.name != hazardName)
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
                GameObject? mapObject = mapObjectInfo.GetMapObjectPrefab();
                if (mapObjectInfo.ShouldSkipIgnoreOverride() || mapObject == null || mapObject.name != hazard.hazardObject.name)
                    continue;

                string hazardName = mapObject.name;
                IndoorMapHazardType? indoorMapHazardType = mapObjectInfo.InsideInfo?.IndoorMapHazardType;

                if (!originalStates.ContainsKey(hazardName))
                {
                    originalStates[hazardName] = new DawnSnapshot
                    {
                        OutsideWeights = mapObjectInfo.OutsideInfo?.SpawnWeights,
                        InsideWeights = mapObjectInfo.InsideInfo?.SpawnWeights,
                        FacingAway = indoorMapHazardType?.spawnFacingAwayFromWall ?? false,
                        FacingWall = indoorMapHazardType?.spawnFacingWall ?? false,
                        BackToWall = indoorMapHazardType?.spawnWithBackToWall ?? false,
                        BackFlush = indoorMapHazardType?.spawnWithBackFlushAgainstWall ?? false,
                        ReqDist = indoorMapHazardType?.requireDistanceBetweenSpawns ?? false,
                        DisallowNear = indoorMapHazardType?.disallowSpawningNearEntrances ?? false,
                        AllowInMineshaft = indoorMapHazardType?.allowInMineshaft ?? false
                    };
                }

                DawnOutsideMapObjectInfo? outsideInfo = mapObjectInfo.OutsideInfo;
                DawnInsideMapObjectInfo? insideInfo = mapObjectInfo.InsideInfo;

                NamespacedKey<DawnMoonInfo> moonKey = RoundManager.Instance.currentLevel.GetDawnInfo().TypedKey;

                float computedWeight = Random.Range(
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
                    insideInfo.IndoorMapHazardType.spawnFacingAwayFromWall = hazard.facingAwayFromWall;
                    insideInfo.IndoorMapHazardType.spawnFacingWall = hazard.facingWall;
                    insideInfo.IndoorMapHazardType.spawnWithBackToWall = hazard.backToWall;
                    insideInfo.IndoorMapHazardType.spawnWithBackFlushAgainstWall = hazard.backFlushWithWall;
                    insideInfo.IndoorMapHazardType.requireDistanceBetweenSpawns = hazard.requireDistanceBetween;
                    insideInfo.IndoorMapHazardType.disallowSpawningNearEntrances = hazard.disallowNearEntrances;
                    insideInfo.IndoorMapHazardType.allowInMineshaft = hazard.allowInMineshaft;

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
                    GameObject? mapObject = mapObjectInfo.GetMapObjectPrefab();
                    if (mapObject != null && mapObject.name == entry.Key)
                    {
                        if (mapObjectInfo.OutsideInfo != null)
                            mapObjectInfo.OutsideInfo.SpawnWeights = (ProviderTable<AnimationCurve?, DawnMoonInfo, SpawnWeightContext>?)entry.Value.OutsideWeights;

                        if (mapObjectInfo.InsideInfo != null)
                        {
                            mapObjectInfo.InsideInfo.SpawnWeights = (ProviderTable<AnimationCurve?, DawnMoonInfo, SpawnWeightContext>?)entry.Value.InsideWeights;

                            IndoorMapHazardType indoorMapHazardType = mapObjectInfo.InsideInfo.IndoorMapHazardType;
                            indoorMapHazardType.spawnFacingAwayFromWall = entry.Value.FacingAway;
                            indoorMapHazardType.spawnFacingWall = entry.Value.FacingWall;
                            indoorMapHazardType.spawnWithBackToWall = entry.Value.BackToWall;
                            indoorMapHazardType.spawnWithBackFlushAgainstWall = entry.Value.BackFlush;
                            indoorMapHazardType.requireDistanceBetweenSpawns = entry.Value.ReqDist;
                            indoorMapHazardType.disallowSpawningNearEntrances = entry.Value.DisallowNear;
                            indoorMapHazardType.allowInMineshaft = entry.Value.AllowInMineshaft;
                        }
                        break;
                    }
                }
            }
            originalStates.Clear();
        }
    }
}
