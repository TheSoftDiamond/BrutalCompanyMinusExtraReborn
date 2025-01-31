
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using static BrutalCompanyMinus.Assets;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class Dweller : MEvent
    {
        public override string Name() => nameof(Dweller);

        public static Dweller Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;//3
            Descriptions = new List<string>() { "Maneater... but outside, good luck", "Dont be scared" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                "CaveDweller",
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(1.0f, 0.0f, 1.0f, 1.0f),
                new Scale(1.0f, 0.0f, 1.0f, 1.0f))
            };

          //  EventsToRemove = new List<string>() { nameof(RealityShift), nameof(Pickles), nameof(TakeyPlush) };

        }

       // public override bool AddEventIfOnly() => (!Compatibility.NonShippingAuthorisationPresent);
        

        public override void Execute()
        {
           // EnemyType dweller = Assets.GetEnemy(Assets.EnemyName.CaveDweller);

           // Manager.AddEnemyToPoolWithRarity(ref RoundManager.Instance.currentLevel.OutsideEnemies, dweller, monsterEvents[0].outsideSpawnRarity.Compute(Type));


           // Manager.Spawn.OutsideEnemies(dweller, UnityEngine.Random.Range(monsterEvents[0].minOutside.Compute(Type), monsterEvents[0].maxOutside.Compute(Type) + 1));




            
            ExecuteAllMonsterEvents();
        }
    }
}
