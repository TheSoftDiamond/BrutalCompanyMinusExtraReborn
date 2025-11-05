using System.Collections.Generic;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class Dweller : MEvent
    {
        public override string Name() => nameof(Dweller);

        public static Dweller Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;//3
            Descriptions = new List<string>() { "Maneater... but outside, good luck", "Dont be scared" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                "CaveDweller",
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(1.0f, 0.0f, 1.0f, 1.0f),
                new Scale(1.0f, 0.0f, 1.0f, 1.0f))
            };
        }

        public override void Execute()
        {
            ExecuteAllMonsterEvents();
        }
    }
}
