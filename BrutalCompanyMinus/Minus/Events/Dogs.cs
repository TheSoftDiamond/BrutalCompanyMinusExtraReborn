﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class Dogs : MEvent
    {
        public override string Name() => nameof(Dogs);

        public static Dogs Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "They can hear you", "Who's a good boy?", "They aren't good boys ;(", "The ground trembles under their paws", "Bring out your whoopie cushions!", "Make sure to close the door behind you" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                Assets.EnemyName.EyelessDog,
                new Scale(10.0f, 0.4f, 10.0f, 50.0f),
                new Scale(20.0f, 0.8f, 20.0f, 100.0f),
                new Scale(1.0f, 0.03f, 1.0f, 4.0f),
                new Scale(1.0f, 0.04f, 1.0f, 5.0f),
                new Scale(1.0f, 0.03f, 1.0f, 4.0f),
                new Scale(2.0f, 0.04f, 2.0f, 6.0f))
            };
        }

        public override void Execute() => ExecuteAllMonsterEvents();
    }
}
