using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class Barbers : MEvent
    {
        public override string Name() => nameof(Barbers);

        public static Barbers Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "Haircut time", "The Barber called!", "You are due for a trim" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            monsterEvents = new List<MonsterEvent>()
            {
                new MonsterEvent(
                    "ClaySurgeon",
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(1.0f, 0.13f, 1.0f, 4.0f),
                    new Scale(4.0f, 0.06f, 4.0f, 8.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f)),
            };
        }

        public override bool AddEventIfOnly() => Compatibility.BarberFixesPresent;

        public override void Execute() => ExecuteAllMonsterEvents();
    }
}