using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class AntiJester : MEvent
    {
        public override string Name() => nameof(AntiJester);

        public static AntiJester Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "Do not make a sound", "It hears", "It sees with its ears", "Sound is its vision", "The sibling of the jester" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            EventsToRemove = new List<string>() { nameof(Jester) };

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                Assets.antiJester,
                new Scale(20.0f, 0.8f, 20.0f, 100.0f),
                new Scale(5.0f, 0.2f, 5.0f, 25.0f),
                new Scale(2.0f, 0.04f, 2.0f, 6.0f),
                new Scale(2.0f, 0.06f, 2.0f, 8.0f),
                new Scale(0.0f, 0.02f, 0.0f, 1.0f),
                new Scale(0.0f, 0.03f, 0.0f, 3.0f))
            };
        }

        public override void Execute()
        {
            Manager.RemoveSpawn(Assets.EnemyName.CoilHead);
            ExecuteAllMonsterEvents();
        }
    }
}
