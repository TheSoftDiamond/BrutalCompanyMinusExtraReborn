using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class Nemo : MEvent
    {
        public override string Name() => nameof(Nemo);

        public static Nemo Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Finding Nemo", "You found him" };
            ColorHex = "#008000";
            Type = EventType.Good;

            monsterEvents = new List<MonsterEvent>()
            {
                new MonsterEvent(
                    "NemoAsset",
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(100.0f, 0.0f, 100.0f, 100.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(1.0f, 0.0f, 1.0f, 1.0f),
                    new Scale(1.0f, 0.0f, 1.0f, 1.0f)),
            };
        }

        public override bool AddEventIfOnly() => Compatibility.SurfacedPresent;

        public override void Execute() => ExecuteAllMonsterEvents();
    }
}