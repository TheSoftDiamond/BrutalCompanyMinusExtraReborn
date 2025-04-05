using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class SoulDev : MEvent
    {
        public override string Name() => nameof(SoulDev);

        public static SoulDev Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "When they said it could be worse.. this is that!", "You do not want to encounter this.", "It looks like a jester but without the box!" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            monsterEvents = new List<MonsterEvent>()
            {
                new MonsterEvent(
                    "SoulDev",
                    new Scale(1.0f, 0.05f, 0.3f, 3.0f),
                    new Scale(1.0f, 0.05f, 0.3f, 3.0f),
                    new Scale(1.0f, 0.01f, 0.0f, 1.0f),
                    new Scale(1.0f, 0.01f, 1.0f, 2.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f))
            };
        }

        public override bool AddEventIfOnly() => Compatibility.soulDevPresent;

        public override void Execute() => ExecuteAllMonsterEvents();
    }
}