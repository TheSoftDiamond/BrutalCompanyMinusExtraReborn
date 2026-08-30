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
            Type = EventType.Insane;
            Aliases = new List<string>() { "NutslayerMore" };

            EventsToRemove = new List<string>() { nameof(HeavyRain), nameof(Raining), nameof(Masked)};

            monstersToSpawn = new List<MonsterEvent>() { new MonsterEvent(
                Assets.nutSlayer,
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(8.0f, 0.0f, 8.0f, 8.0f),
                new Scale(12.0f, 0.0f, 12.0f, 12.0f))
            };

            ScaleList.Add(ScaleType.ScrapValue, new Scale(4.5f, 0.0f, 4.5f, 4.5f));

            ScaleList.Add(ScaleType.SpawnChance, new Scale(2.0f, 0.0f, 2.0f, 2.0f));
        }

        public override void Execute() 
        {
            if (Configuration.enforceEscapeModChecks.Value && !Compatibility.StarLancereNemyEscapePresent)
            {
                Instance.monstersToSpawn[0].minOutside = new Scale(0f, 0f, 0f, 0f);
                Instance.monstersToSpawn[0].maxOutside = new Scale(0f, 0f, 0f, 0f);
                Instance.monstersToSpawn[0].outsideSpawnRarity = new Scale(0f, 0f, 0f, 0f);
            }

            ExecuteAllMonsterEvents();
            Manager.MultiplySpawnChance(RoundManager.Instance.currentLevel, Getf(ScaleType.SpawnChance));
            Manager.scrapValueMultiplier *= Getf(ScaleType.ScrapValue);
        }
    }
}
