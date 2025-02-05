using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class GiantsOutside : MEvent
    {
        public override string Name() => nameof(GiantsOutside);

        public static GiantsOutside Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "Attack on titan!!", "Battle of giants", "Take your bets..." };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            monsterEvents = new List<MonsterEvent>() {
                new MonsterEvent(
                Assets.EnemyName.ForestKeeper,
                new Scale(1.0f, 0.0f, 1.0f, 1.0f),
                new Scale(33.0f, 0.66f, 33.0f, 100.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(4.0f, 0.04f, 4.0f, 8.0f),
                new Scale(4.0f, 0.04f, 4.0f, 12.0f))
            };
            EventsToRemove = new List<string>() { nameof(GiantShowdown) };
        }

        public override bool AddEventIfOnly() => !Compatibility.theGiantSpecimensPresent;

        public override void Execute() => ExecuteAllMonsterEvents();
    }
}
