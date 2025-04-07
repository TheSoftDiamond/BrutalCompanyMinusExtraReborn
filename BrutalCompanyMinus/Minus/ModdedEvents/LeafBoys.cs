using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class LeafBoys : MEvent
    {
        public override string Name() => nameof(LeafBoys);

        public static LeafBoys Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Unwanted attention", "The boys from the trees", "Imagine dogs and leafs" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToSpawnWith = new List<string>() { nameof(Dogs) };

            monsterEvents = new List<MonsterEvent>()
            {
                new MonsterEvent(
                    "LeafBoiEnemyType",
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                    new Scale(0.0f, 0.00f, 0.0f, 0.0f),
                    new Scale(0.0f, 0.00f, 0.0f, 0.0f),
                    new Scale(2.0f, 0.13f, 2.0f, 4.0f),
                    new Scale(4.0f, 0.15f, 4.0f, 8.0f))
            };
        }

        public override bool AddEventIfOnly() => Compatibility.BiodiversityPresent;

        public override void Execute() => ExecuteAllMonsterEvents();
    }
}