using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class KiwiBird : MEvent
    {
        public override string Name() => nameof(KiwiBird);

        public static KiwiBird Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 4;
            Descriptions = new List<string>() { "Eggs for breakfast!", "Egging you on", "Is it worth it?", "Beware of the Giant Kiwi", "Whats that pecking noise?", "Its like a woodpecker but..." };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                Assets.EnemyName.GiantKiwi,
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(44.0f, 3.2f, 44.0f, 100.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(1.0f, 0.0f, 1.0f, 1.0f),
                new Scale(2.0f, 0.04f, 2.0f, 3.0f))
            };
        }

        public override void Execute() => ExecuteAllMonsterEvents();

        public override bool AddEventIfOnly() => Assets.ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Enable Special Events?");
    }
}
