using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class SCP939 : MEvent
    {
        public override string Name() => nameof(SCP939);

        public static SCP939 Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "SCP939...", "Are you ready for them?", "The facility could not contain them..." };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            monsterEvents = new List<MonsterEvent>()
            {
                new MonsterEvent(
                    "SCP939",
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(1.0f, 0.13f, 1.0f, 2.0f),
                    new Scale(2.0f, 0.06f, 4.0f, 4.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f)),
            };
        }

        public override bool AddEventIfOnly() => Compatibility.SCP939Present;

        public override void Execute() => ExecuteAllMonsterEvents();
    }
}