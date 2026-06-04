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
    internal class TeleporterTraps : MEvent
    {
        public override string Name() => nameof(TeleporterTraps);

        public static TeleporterTraps Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "Teleporter Traps!", "These will teleport enemies" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            ScaleList.Add(ScaleType.MinAmount, new Scale(5.0f, 0.2f, 5.0f, 25.0f));
            ScaleList.Add(ScaleType.MaxAmount, new Scale(7.0f, 0.28f, 7.0f, 35.0f));
        }

        public override bool AddEventIfOnly() => RoundManager.Instance.currentLevel.spawnableMapObjects.ToList().Exists(x => x.prefabToSpawn.name == "TeleporterTrap");

        public override void Execute()
        {
            var TeleporterTrap = new IndoorMapHazard()
            {
                hazardType = new IndoorMapHazardType()
                {
                    prefabToSpawn = Assets.GetObject("TeleporterTrap"),
                    spawnFacingAwayFromWall = false,
                    spawnFacingWall = false,
                    spawnWithBackToWall = false,
                    spawnWithBackFlushAgainstWall = false,
                    requireDistanceBetweenSpawns = false,
                    disallowSpawningNearEntrances = false
                },
                numberToSpawn = new AnimationCurve(new Keyframe(0f, Get(ScaleType.MinAmount)), new Keyframe(1f, Get(ScaleType.MaxAmount)))
            };

            EventManager.hazards.Add(TeleporterTrap);

            RoundManager.Instance.currentLevel.indoorMapHazards = RoundManager.Instance.currentLevel.indoorMapHazards.AddToArray(TeleporterTrap);
        }
    }
}
