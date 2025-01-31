using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class HallucinoceptsEv : MEvent
    {
        bool overrideVar = true;
        bool isDev = false;
        public override string Name() => nameof(HallucinoceptsEv);

        public static HallucinoceptsEv Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "Minzy!", "Fakes!", "Minzyshrooms", "Hope u like getting high..." };
            ColorHex = "#FFFFFF";
            Type = EventType.Neutral;

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                "DeafMakerShroom",
                new Scale(10.0f, 0.4f, 10.0f, 50.0f),
                new Scale(5.0f, 0.1f, 5.0f, 15.0f),
                new Scale(1.0f, 0.04f, 1.0f, 5.0f),
                new Scale(1.0f, 0.06f, 1.0f, 7.0f),
                new Scale(0.0f, 0.0075f, 0.0f, 1.0f),
                new Scale(0.0f, 0.02f, 0.0f, 2.0f)), new MonsterEvent (
                Assets.EnemyName.Shrooms,
                new Scale(10.0f, 0.4f, 10.0f, 50.0f),
                new Scale(5.0f, 0.1f, 5.0f, 15.0f),
                new Scale(1.0f, 0.04f, 1.0f, 5.0f),
                new Scale(1.0f, 0.06f, 1.0f, 7.0f),
                new Scale(0.0f, 0.0075f, 0.0f, 1.0f),
                new Scale(0.0f, 0.02f, 0.0f, 2.0f))

                //          ScaleList.Add(ScaleType.MinAmount, new Scale(3.0f, 0.12f, 3.0f, 15.0f));
                //          ScaleList.Add(ScaleType.MaxAmount, new Scale(4.0f, 0.16f, 4.0f, 20.0f));

                //     EventsToSpawnWith = new List<string>() { nameof(Turrets), nameof(Landmines)};
            };
        }
        public override bool AddEventIfOnly()
        {
            if (isDev == true)
                if (!Compatibility.hallucinoceptsPresent)
            {

                return Compatibility.hallucinoceptsPresent;
            }
          else if (overrideVar == true) 
          {
                return true;
          }
                return false;
        }

        //    {
        //         return RoundManager.Instance.currentLevel.spawnableMapObjects.ToList().Exists(x => x.prefabToSpawn.name == Assets.ObjectNameList[Assets.ObjectName.Turret]);
        //   }

        public override void Execute()
        {
            if (!Compatibility.hallucinoceptsPresent) return;

            ExecuteAllMonsterEvents();
       //     Hallucinoceps.DeafMakerShroom.allShrooms;

            //        RoundManager.Instance.currentLevel.spawnableMapObjects = RoundManager.Instance.currentLevel.spawnableMapObjects.Add(new SpawnableMapObject()
            //        {
            //            prefabToSpawn = Assets.GetEnemy(Assets.EnemyName.Shrooms),
            //            numberToSpawn = new AnimationCurve(new Keyframe(0f, Get(ScaleType.MinAmount)), new Keyframe(1f, Get(ScaleType.MaxAmount))),
            //            spawnFacingAwayFromWall = false,
            //            spawnFacingWall = false,
            //            spawnWithBackToWall = false,
            //            spawnWithBackFlushAgainstWall = false,
            //            requireDistanceBetweenSpawns = false,
            //            disallowSpawningNearEntrances = false
            //        });


        }
    }
}
