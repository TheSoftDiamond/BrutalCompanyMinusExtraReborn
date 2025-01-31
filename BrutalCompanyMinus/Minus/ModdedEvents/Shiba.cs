using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class Shiba : MEvent
    {
        public override string Name() => nameof(Shiba);

        public static Shiba Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "Awww its so cute!", "Innocent?", "BONK!", "Comes with a bat", "Hope you like bats" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                "ShibaEnemy",
                new Scale(60.0f, 0.43f, 60.0f, 100.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(1.0f, 0.0f, 1.0f, 1.0f),
                new Scale(2.0f, 0.2f, 2.0f, 4.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f))
            };
        }

        public override bool AddEventIfOnly() => Compatibility.ShibaPresent;

        public override void Execute() => ExecuteAllMonsterEvents();
    }
}
