using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class SkullEnemy : MEvent
    {
        public override string Name() => nameof(SkullEnemy);

        public static SkullEnemy Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "The skull chases you...", "Do what you will, but it follows..." };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            monsterEvents = new List<MonsterEvent>()
            {
                new MonsterEvent(
                    "SkullEnemy",
                    new Scale(100.0f, 0.0f, 100.0f, 100.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(1.0f, 0.0f, 1.0f, 1.0f),
                    new Scale(1.0f, 0.0f, 1.0f, 1.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f)),
            };
        }

        public override bool AddEventIfOnly() => Compatibility.SkullEnemyPresent;

        public override void Execute() => ExecuteAllMonsterEvents();
    }
}