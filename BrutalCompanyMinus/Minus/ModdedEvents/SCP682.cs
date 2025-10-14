using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class SCP682 : MEvent
    {
        public override string Name() => nameof(SCP682);

        public static SCP682 Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "SCP682!", "The Hard-to-Destroy Reptile", "Avoid the reptile!" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            monsterEvents = new List<MonsterEvent>()
            {
                new MonsterEvent(
                    "SCP682ET",
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(1.0f, 0.13f, 1.0f, 2.0f),
                    new Scale(2.0f, 0.06f, 3.0f, 3.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f)),
            };
        }

        public override bool AddEventIfOnly() => Compatibility.SCP682Present;

        public override void Execute() => ExecuteAllMonsterEvents();
    }
}