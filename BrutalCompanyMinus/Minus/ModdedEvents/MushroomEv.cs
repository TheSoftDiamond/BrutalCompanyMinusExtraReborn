using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class MushroomEv : MEvent
    {
        public override string Name() => nameof(MushroomEv);

        public static MushroomEv Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 100; //5
            Descriptions = new List<string>() { "Lasso man", "Walking rope?", "MantiToils???" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                "Mushroom",
                new Scale(8.0f, 0.8f, 8.0f, 9.0f),
                new Scale(5.0f, 0.2f, 5.0f, 6.0f),
                new Scale(3.0f, 0.06f, 3.0f, 9.0f),
                new Scale(4.0f, 0.08f, 4.0f, 12.0f),
                new Scale(8.0f, 0.02f, 8.0f, 9.0f),
                new Scale(6.0f, 0.03f, 7.0f, 9.0f))
            };

            EventsToRemove = new List<string>() { nameof(NoMantitoil) };

        }
        
        public override bool AddEventIfOnly() => Compatibility.hallucinoceptsPresent;

        public override void Execute()
        {
          //  Hallucinoceps.DeafMakerShroom.allShrooms.Add
            ExecuteAllMonsterEvents();
         //   com.github.zehsteam.ToilHead.Api.forceMantiToilSpawns = true;
            // com.github.zehsteam.ToilHead.Api.forceSpawns = true;
            // Manager.RemoveSpawn(Assets.EnemyName.CoilHead); 
            // Manager.AddInsideSpawnChance(Assets.antiCoilHead.enemyName = "Spring");
        }

        
    }
}
