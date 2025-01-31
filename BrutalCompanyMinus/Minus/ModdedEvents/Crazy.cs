using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class Crazy : MEvent
    {
        public override string Name() => nameof(Crazy);

        public static Crazy Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 4; 
            Descriptions = new List<string>() { "Sealed", "Hope u have someone on the terminal", "You like doors right?", "Hmmm... Locked" };
            ColorHex = "#280000";
            Type = EventType.VeryBad;

            ScaleList.Add(ScaleType.MinAmount, new Scale(3.0f, 0.12f, 3.0f, 15.0f));
            ScaleList.Add(ScaleType.MaxAmount, new Scale(4.0f, 0.16f, 4.0f, 20.0f));

            EventsToSpawnWith = new List<string>() { nameof(Turrets), nameof(Landmines)};
        }

        public override bool AddEventIfOnly()
        { 
            if (!Compatibility.NonShippingAuthorisationPresent == true)
            {
                return RoundManager.Instance.currentLevel.spawnableMapObjects.ToList().Exists(x => x.prefabToSpawn.name == Assets.ObjectNameList[Assets.ObjectName.BigDoor]);
            }
            return false;
        }

        public override void Execute()
        {
            

       //     RoundManager.Instance.currentLevel.spawnableMapObjects = RoundManager.Instance.currentLevel.spawnableMapObjects.Add(new SpawnableMapObject()
       //     {
         //       prefabToSpawn = Assets.GetObject(Assets.ObjectName.SpikeRoofTrap),
         //       numberToSpawn = new AnimationCurve(new Keyframe(0f, Get(ScaleType.MinAmount)), new Keyframe(1f, Get(ScaleType.MaxAmount))),
         //       spawnFacingAwayFromWall = false,
         //       spawnFacingWall = true,
         //       spawnWithBackToWall = true,
         //       spawnWithBackFlushAgainstWall = false,
         //       requireDistanceBetweenSpawns = false,
         //       disallowSpawningNearEntrances = false
        //    });

            RoundManager.Instance.currentLevel.spawnableMapObjects = RoundManager.Instance.currentLevel.spawnableMapObjects.Add(new SpawnableMapObject()
            {
                prefabToSpawn = Assets.GetObject(Assets.ObjectName.BigDoor),
                numberToSpawn = new AnimationCurve(new Keyframe(0f, Get(ScaleType.MinAmount)), new Keyframe(1f, Get(ScaleType.MaxAmount))),
                spawnFacingAwayFromWall = true,
                spawnFacingWall = true,
                spawnWithBackToWall = true,
                spawnWithBackFlushAgainstWall = true,
                requireDistanceBetweenSpawns = false,
                disallowSpawningNearEntrances = false
            });


        }
    }
}
