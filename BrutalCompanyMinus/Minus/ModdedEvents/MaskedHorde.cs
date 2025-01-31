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
    internal class MaskedHorde : MEvent
    {
        public override string Name() => nameof(MaskedHorde);

        public static MaskedHorde Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2; //5
            Descriptions = new List<string>() { "The horde is near!", "Masked!", "Trust noone" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                Assets.EnemyName.Masked,
                new Scale(15.0f, 0.8f, 10.0f, 50.0f),
                new Scale(5.0f, 0.2f, 5.0f, 10.0f),
                new Scale(3.0f, 0.06f, 3.0f, 9.0f),
                new Scale(4.0f, 0.08f, 4.0f, 12.0f),
                new Scale(8.0f, 0.02f, 8.0f, 10.0f),
                new Scale(10.0f, 0.03f, 10.0f, 12.0f))
            };

            EventsToRemove = new List<string>() { nameof(Masked), nameof(ZombieApocalipse) };

        }
        
      //  public override bool AddEventIfOnly() => Compatibility.toilheadPresent; NotRequired

        public override void Execute()
        {
            ExecuteAllMonsterEvents();
          //  com.github.zehsteam.ToilHead.Api.forceMantiToilSpawns = true;
            // com.github.zehsteam.ToilHead.Api.forceSpawns = true;
            // Manager.RemoveSpawn(Assets.EnemyName.CoilHead); 
            // Manager.RemoveSpawn(Assets.antiCoilHead.enemyName = "Spring");
        }

        
    }
}
