using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class Landmines : MEvent
    {
        public override string Name() => nameof(Landmines);

        public static Landmines Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "Watch your step", "This facility is rigged", "Landmines, yes", "+Landmines" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;
            Aliases = new List<string>() { "Landmine" };

            ScaleList.Add(ScaleType.MinAmount, new Scale(5.0f, 0.2f, 5.0f, 25.0f));
            ScaleList.Add(ScaleType.MaxAmount, new Scale(7.0f, 0.28f, 7.0f, 35.0f));
            ScaleList.Add(ScaleType.MinPercentSelected, new Scale(0.0f, 0.0f, 0.0f, 0.0f));
            ScaleList.Add(ScaleType.MaxPercentSelected, new Scale(1.0f, 0.0f, 1.0f, 1.0f));
        }

        public override bool AddEventIfOnly() => RoundManager.Instance.currentLevel.spawnableMapObjects.ToList().Exists(x => x.prefabToSpawn.name == Assets.ObjectNameList[Assets.ObjectName.Landmine]);

        public override void Execute()
        {
            var landmine = new IndoorMapHazard()
            {
                hazardType = new IndoorMapHazardType()
                {
                    prefabToSpawn = Assets.GetObject(Assets.ObjectName.Landmine),
                    spawnFacingAwayFromWall = true,
                    spawnFacingWall = false,
                    spawnWithBackToWall = false,
                    spawnWithBackFlushAgainstWall = false,
                    requireDistanceBetweenSpawns = false,
                    disallowSpawningNearEntrances = false
                },
                numberToSpawn = new AnimationCurve(new Keyframe(Getf(ScaleType.MinPercentSelected), Get(ScaleType.MinAmount)), new Keyframe(Getf(ScaleType.MaxPercentSelected), Get(ScaleType.MaxAmount)))
            };

            EventManager.hazards.Add(landmine);

            RoundManager.Instance.currentLevel.indoorMapHazards = RoundManager.Instance.currentLevel.indoorMapHazards.AddToArray(landmine);
        }
    }
}
