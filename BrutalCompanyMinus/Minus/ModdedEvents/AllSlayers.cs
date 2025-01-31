using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class AllSlayers : MEvent
    {
        public override string Name() => nameof(AllSlayers);

        public static AllSlayers Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Hope you like miniguns, because they are everywhere", "MantiSlayers and ToilSlayers.... good luck!" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            EventsToRemove = new List<string>() { nameof(Coilhead), nameof(AntiCoilhead), nameof(ToilHead), nameof(Mantitoil), nameof(ToilSlayer), nameof(MantiToilSlayer), nameof(NoSlayers), nameof(NoMantitoil) };

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                Assets.EnemyName.Manticoil,
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(5.0f, 0.0f, 5.0f, 5.0f),
                new Scale(10.0f, 0.0f, 10.0f, 10.0f)), new MonsterEvent(
                Assets.EnemyName.CoilHead,
                new Scale(30.0f, 1.0f, 30.0f, 90.0f),
                new Scale(10.0f, 0.34f, 10.0f, 30.0f),
                new Scale(1.0f, 0.034f, 1.0f, 3.0f),
                new Scale(2.0f, 0.034f, 1.0f, 4.0f),
                new Scale(1.0f, 1.0f, 1.0f, 1.0f),
                new Scale(2.0f, 2.0f, 2.0f, 2.0f))
            };
        }

        public override bool AddEventIfOnly() => Compatibility.toilheadPresent;

        public override void Execute()
        {
            if (!Compatibility.toilheadPresent) return;
            ExecuteAllMonsterEvents();
            com.github.zehsteam.ToilHead.Api.ForceMantiSlayerMaxSpawnCount = 10;
            com.github.zehsteam.ToilHead.Api.ForceMantiSlayerSpawns = true;
            com.github.zehsteam.ToilHead.Api.ForceToilSlayerSpawns = true;
            com.github.zehsteam.ToilHead.Api.ForceToilSlayerMaxSpawnCount = 25;
            com.github.zehsteam.ToilHead.Api.ForceToilHeadMaxSpawnCount = 0;
        }
    }
}
