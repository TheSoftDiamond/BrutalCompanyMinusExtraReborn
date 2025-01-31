using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class MantiToilSlayer : MEvent
    {
        public override string Name() => nameof(MantiToilSlayer);

        public static MantiToilSlayer Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "Flying miniguns", "MantiToils... but more advanced", "MantiSlayers!!!" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            EventsToRemove = new List<string>() { nameof(Coilhead), nameof(AntiCoilhead), nameof(ToilHead), nameof(Mantitoil), nameof(ToilSlayer), nameof(AllSlayers), nameof(NoSlayers), nameof(NoMantitoil) };
            
            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                Assets.EnemyName.Manticoil,
                new Scale(0.0f, 0.0f, 0.0f, 0.0f), //InsideEnemyRarity
                new Scale(0.0f, 0.0f, 0.0f, 0.0f), //OutsideEnemyRarity
                new Scale(0.0f, 0.0f, 0.0f, 0.0f), //MinInsideEnemy
                new Scale(0.0f, 0.0f, 0.0f, 0.0f), //MaxInsideEnemy
                new Scale(7.0f, 0.0f, 7.0f, 7.0f), //MinOutsideEnemy
                new Scale(10.0f, 0.0f, 10.0f, 10.0f)) //MaxOutsideEnemy
            };
        }

        public override bool AddEventIfOnly() => Compatibility.toilheadPresent;

        public override void Execute()
        {
            if (!Compatibility.toilheadPresent) return;
            ExecuteAllMonsterEvents();
            com.github.zehsteam.ToilHead.Api.ForceMantiSlayerMaxSpawnCount = 25;
            com.github.zehsteam.ToilHead.Api.ForceMantiSlayerSpawns = true;
            com.github.zehsteam.ToilHead.Api.ForceToilSlayerMaxSpawnCount = 0;
            com.github.zehsteam.ToilHead.Api.ForceToilHeadMaxSpawnCount = 0;
        }
    }
}
