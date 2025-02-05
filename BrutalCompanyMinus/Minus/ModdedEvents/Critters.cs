using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class Critters : MEvent
    {
        public override string Name() => nameof(Critters);

        public static Critters Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "They Come In Numbers", "Big smiles", "They are super cute.. or not?!" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                "KickinRuinedEnemyType",
                new Scale(45.0f, 3.0f, 45.0f, 90.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(1.0f, 0.017f, 1.0f, 2.0f),
                new Scale(1.0f, 0.0045f, 1.0f, 3.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f)), new MonsterEvent(
                "HoppyRuinedEnemyType",
                new Scale(45.0f, 3.0f, 45.0f, 90.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(1.0f, 0.017f, 1.0f, 2.0f),
                new Scale(1.0f, 0.0045f, 1.0f, 3.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f)), new MonsterEvent(
                "BobbyRuinedEnemyType",
                new Scale(45.0f, 3.0f, 45.0f, 90.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(1.0f, 0.017f, 1.0f, 2.0f),
                new Scale(1.0f, 0.0045f, 1.0f, 3.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f)), new MonsterEvent(
                "PickyRuinedEnemyType",
                new Scale(45.0f, 3.0f, 45.0f, 90.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(1.0f, 0.017f, 1.0f, 2.0f),
                new Scale(1.0f, 0.0045f, 1.0f, 3.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f)), new MonsterEvent(
                "BubbaRuinedEnemyType",
                new Scale(45.0f, 3.0f, 45.0f, 90.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(1.0f, 0.017f, 1.0f, 2.0f),
                new Scale(1.0f, 0.0045f, 1.0f, 3.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f)), new MonsterEvent(
                "CraftyRuinedEnemyType",
                new Scale(45.0f, 3.0f, 45.0f, 90.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(1.0f, 0.017f, 1.0f, 2.0f),
                new Scale(1.0f, 0.0045f, 1.0f, 3.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f)), new MonsterEvent(
                "CatnapRuinedEnemyType",
                new Scale(45.0f, 3.0f, 45.0f, 90.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(1.0f, 0.017f, 1.0f, 2.0f),
                new Scale(1.0f, 0.0045f, 1.0f, 4.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f)), new MonsterEvent(
                "DogdayRuinedEnemyType",
                new Scale(45.0f, 3.0f, 45.0f, 90.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(1.0f, 0.017f, 1.0f, 2.0f),
                new Scale(1.0f, 0.0045f, 1.0f, 3.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f))
            };
        }

        public override bool AddEventIfOnly() => Compatibility.PlaytimePresent;

        public override void Execute() => ExecuteAllMonsterEvents();
    }
}
