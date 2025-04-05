using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class MoaiEnemy : MEvent
    {
        public override string Name() => nameof(MoaiEnemy);

        public static MoaiEnemy Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Statues are on the move!", "The ancient guardians have awoken. Run.", "Prepare for a rocky reception" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            monsterEvents = new List<MonsterEvent>()
            {
                new MonsterEvent(
                    "MoaiEnemy",
                    new Scale(12.0f, 0.3f, 7.0f, 25.0f),
                    new Scale(20.0f, 0.4f, 15.0f, 30.0f),
                    new Scale(1.0f, 0.03f, 1.0f, 1.0f),
                    new Scale(1.0f, 0.04f, 0.0f, 2.0f),
                    new Scale(1.0f, 0.3f, 1.0f, 2.0f),
                    new Scale(1.0f, 0.4f, 1.0f, 3.0f)),


                new MonsterEvent(
                    "MoaiBlue",
                    new Scale(6.0f, 0.15f, 3.0f, 12.0f),
                    new Scale(3.0f, 0.15f, 2.5f, 8.0f),
                    new Scale(1.0f, 0.02f, 1.0f, 1.0f),
                    new Scale(1.0f, 0.03f, 0.0f, 1.0f),
                    new Scale(0.5f, 0.25f, 1.0f, 1.0f),
                    new Scale(1.0f, 0.25f, 1.0f, 2.0f)),

                new MonsterEvent(
                    "MoaiRed",
                    new Scale(14.0f, 0.4f, 9.0f, 25.0f),
                    new Scale(10.0f, 0.3f, 7.0f, 18.0f),
                    new Scale(1.0f, 0.03f, 1.0f, 1.0f),
                    new Scale(1.0f, 0.05f, 1.0f, 2.0f),
                    new Scale(0.0f, 0.35f, 1.0f, 1.0f),
                    new Scale(2.0f, 0.4f, 2.0f, 2.0f)),

                new MonsterEvent(
                    "MoaiGreen",
                    new Scale(5.0f, 0.15f, 3.0f, 10.0f),
                    new Scale(2.5f, 0.1f, 2.0f, 6.0f),
                    new Scale(1.0f, 0.02f, 1.0f, 1.0f),
                    new Scale(1.0f, 0.03f, 0.0f, 1.0f),
                    new Scale(1.0f, 0.25f, 1.0f, 1.0f),
                    new Scale(1.0f, 0.25f, 1.0f, 1.0f)),

                new MonsterEvent(
                    "MoaiGold",
                    new Scale(1.5f, 0.05f, 0.5f, 4.0f),
                    new Scale(1.0f, 0.05f, 0.5f, 2.5f),
                    new Scale(1.0f, 0.01f, 1.0f, 1.0f),
                    new Scale(1.0f, 0.01f, 1.0f, 1.0f),
                    new Scale(0.0f, 0.1f, 1.0f, 1.0f),
                    new Scale(1.0f, 0.1f, 1.0f, 1.0f)),

                new MonsterEvent(
                    "MoaiPurple",
                    new Scale(13.0f, 0.4f, 8.0f, 22.0f),
                    new Scale(7.0f, 0.25f, 5.0f, 14.0f),
                    new Scale(1.0f, 0.03f, 1.0f, 1.0f),
                    new Scale(1.0f, 0.05f, 1.0f, 2.0f),
                    new Scale(0.5f, 0.35f, 1.0f, 1.0f),
                    new Scale(1.0f, 0.4f, 2.0f, 2.0f)),
            };
        }

        public override bool AddEventIfOnly() => Compatibility.moaiEnemyPresent;

        public override void Execute() => ExecuteAllMonsterEvents();
    }
}