using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class Toilet : MEvent
    {
        public override string Name() => nameof(Toilet);

        public static Toilet Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 100;
            Descriptions = new List<string>() { "Miniguns", "Toilheads... but more advanced", "ToilSlayers!!!" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

        //    EventsToRemove = new List<string>() { nameof(Coilhead), nameof(AntiCoilhead), nameof(ToilHead), nameof(MantiToilSlayer), nameof(NoToilSlayer), nameof(NoMantitoil),nameof(NoSlayers), nameof(AllSlayers) };

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                "SkibidiToilet",
                new Scale(30.0f, 1.0f, 30.0f, 90.0f),
                new Scale(10.0f, 0.34f, 10.0f, 30.0f),
                new Scale(1.0f, 0.034f, 1.0f, 3.0f),
                new Scale(2.0f, 0.034f, 1.0f, 4.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f))
            };
        }

      //  public override bool AddEventIfOnly() => Compatibility.toilheadPresent;

        public override void Execute()
        {
       //     if (!Compatibility.toilheadPresent) return;
            ExecuteAllMonsterEvents();
       //     com.github.zehsteam.ToilHead.Api.ForceToilSlayerMaxSpawnCount = 15;
       //     com.github.zehsteam.ToilHead.Api.ForceToilSlayerSpawns = true;
       //     com.github.zehsteam.ToilHead.Api.ForceMantiSlayerMaxSpawnCount = 0;
            
        }
    }
}
