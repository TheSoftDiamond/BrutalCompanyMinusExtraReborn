using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class Bellcrab : MEvent
    {
        public override string Name() => nameof(Bellcrab);

        public static Bellcrab Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Bells.. but are they real?", "Do you trust it?", "Dont get pinched!" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            monsterEvents = new List<MonsterEvent>()
            {
                new MonsterEvent(
                    "BellCrabAsset",
                    new Scale(100.0f, 0.0f, 100.0f, 100.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(1.0f, 0.09f, 1.0f, 4.0f),
                    new Scale(4.0f, 0.07f, 4.0f, 10.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f)),
            };
        }

        public override bool AddEventIfOnly() => Compatibility.SurfacedPresent;

        public override void Execute() => ExecuteAllMonsterEvents();
    }
}