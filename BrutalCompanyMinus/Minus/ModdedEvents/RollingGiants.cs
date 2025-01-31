﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class RollingGiants : MEvent
    {
        public override string Name() => nameof(RollingGiants);

        public static RollingGiants Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "Rolling Giants!!", "It wants to touch...", "What even is this thing?????" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                "RollingGiant_EnemyType",
                new Scale(10.0f, 0.4f, 10.0f, 50.0f),
                new Scale(5.0f, 0.1f, 5.0f, 15.0f),
                new Scale(1.0f, 0.02f, 1.0f, 3.0f),
                new Scale(1.0f, 0.02f, 1.0f, 3.0f),
                new Scale(0.0f, 0.0075f, 0.0f, 1.0f),
                new Scale(0.0f, 0.02f, 0.0f, 2.0f))
            };
        }

        public override bool AddEventIfOnly() => Compatibility.rollinggiantPresent;

        public override void Execute() => ExecuteAllMonsterEvents();
    }
}
