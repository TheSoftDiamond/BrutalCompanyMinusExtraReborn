using System.Collections.Generic;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class NutSlayersMore : MEvent
    {
        public override string Name() => nameof(NutSlayersMore);

        public static NutSlayersMore Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Warning: Multiple NutSlayers detected in the area!", "Oh...Oh my god..."  };
            ColorHex = "#000000";
            Type = EventType.VeryBad;

            EventsToRemove = new List<string>() { nameof(HeavyRain), nameof(Raining), nameof(Masked)};

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                Assets.nutSlayer,
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(8.0f, 0.0f, 8.0f, 8.0f),
                new Scale(12.0f, 0.0f, 12.0f, 12.0f))
            };
            ScaleList.Add(ScaleType.MinAmount, new Scale(1f,1f,1f,2f));
            ScaleList.Add(ScaleType.MaxAmount, new Scale(1f, 1f, 1f, 5f));
        }

        public override void Execute() 
        {
            ExecuteAllMonsterEvents();
            Manager.MultiplySpawnChance(RoundManager.Instance.currentLevel, 2f);
            Manager.scrapValueMultiplier *= 4.5f;
        }
    }
}
