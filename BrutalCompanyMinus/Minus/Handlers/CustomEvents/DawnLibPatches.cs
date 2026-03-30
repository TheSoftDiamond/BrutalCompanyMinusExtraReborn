using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using BrutalCompanyMinus.Minus.CustomEvents;
using Dawn;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrutalCompanyMinus.Minus.Handlers.CustomEvents
{
    [HarmonyPatch]
    internal class DawnLibPatches
    {
        //private static Dictionary<NamespacedKey<DawnMapObjectInfo>, DawnMapObjectInfo> brutalRegistry;
        //private static readonly Dictionary<NamespacedKey<DawnMapObjectInfo>, DawnMapObjectInfo> originalRegistry = CopyOriginalRegistry();
        internal static Queue<GeneralCustomEvent.HazardEvent> eventQueue = new Queue<GeneralCustomEvent.HazardEvent>();

        internal static Dictionary<string, DawnSnapshot> originalStates = new Dictionary<string, DawnSnapshot>();

        /// <summary>
        /// This struct helps provide a template for storing original map object data.
        /// </summary>
        internal struct DawnSnapshot
        {
            public ProviderTable<AnimationCurve?, DawnMoonInfo, SpawnWeightContext> OutsideWeights;
            public ProviderTable<AnimationCurve?, DawnMoonInfo, SpawnWeightContext> InsideWeights;
            public bool FacingAway, FacingWall, BackToWall, BackFlush, ReqDist, DisallowNear;
        }

        /// <summary>
        /// This method processes the hazard event data by iterating through the current active events and checking for any GeneralCustomEvents that contain hazard events.
        /// </summary>
        private static bool ProcessHazardEvents()
        {
            bool dataFound = false;
            foreach (MEvent _event in EventManager.currentEvents)
            {
                Log.LogInfo($"Event name: {nameof(_event)}");
                if (_event is GeneralCustomEvent customEvent)
                {
                    Log.LogInfo($"Event {customEvent.Name} is a GeneralCustomEvent, checking hazard events...");
                    foreach (GeneralCustomEvent.HazardEvent hazard in customEvent.hazardEvents)
                    {
                        Log.LogInfo("Processing...");
                        dataFound |= ProcessMapObject(hazard);
                    }
                }
            }

            return dataFound;
        }

        /// <summary>
        /// This helps ensure the map object data actually gets processed correctly.
        /// </summary>
        [HarmonyPatch(typeof(RoundManager), "FinishGeneratingLevel")]
        [HarmonyPrefix]
        static void DawnFixMethod()
        {
            Log.LogInfo($"Area gen: {Manager.terrainArea}");
            if (Manager.terrainArea == 0f)
            {
                Manager.SampleMap();
            }

            while (eventQueue.Count > 0)
            {
                // Process the queue built up at the lever pull
                Log.LogInfo("Items left: "+ eventQueue.Count);
                ProcessMapObject(eventQueue.Dequeue());
            }
        }

        /// <summary>
        /// This method checks if a given hazard name corresponds to a map object that is managed by the Dawn system.
        /// </summary>
        /// <param name="hazardName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method processes the map object information based on the provided hazard event data.
        /// </summary>
        /// <param name="hazard"></param>
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

                // Get the current moon name lowercase without numbers
                NamespacedKey<DawnMoonInfo> moonKey = RoundManager.Instance.currentLevel.GetDawnInfo().TypedKey;

                // Compute spawn counts based on current BCMER difficulty
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

        public static bool OnEventStart()
        {
            return ProcessHazardEvents();
        }

        /// <summary>
        /// This resets the map object data back to normal.
        /// </summary>
        public static void OnEventEnd()
        {
            foreach (var entry in originalStates)
            {
                foreach (DawnMapObjectInfo mapObjectInfo in LethalContent.MapObjects.Values)
                {
                    if (mapObjectInfo.MapObject.name == entry.Key)
                    {
                        if (mapObjectInfo.OutsideInfo != null)
                            mapObjectInfo.OutsideInfo.SpawnWeights = entry.Value.OutsideWeights;

                        if (mapObjectInfo.InsideInfo != null)
                        {
                            mapObjectInfo.InsideInfo.SpawnWeights = entry.Value.InsideWeights;
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
