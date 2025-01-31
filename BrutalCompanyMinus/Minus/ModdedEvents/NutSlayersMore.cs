using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

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

           // EventsToSpawnWith = new List<string>() { nameof(Gloomy), nameof(Thumpers), nameof(Spiders), nameof(Masked) };
            EventsToRemove = new List<string>() { nameof(HeavyRain), nameof(Raining), nameof(Masked)/* nameof(MaskedHorde),*/ };

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
           // ScaleList.Add(ScaleType.ScrapValue, new Scale(15.35f, 3.5115f, 8.35f, 30.5f));
            //   ScaleList.Add(ScaleType.SpawnMultiplier, new Scale(5.25f, 2.0075f, 3.25f, 9.0f));
            //  ScaleList.Add(ScaleType.SpawnCapMultiplier, new Scale(4.4f, 1.016f, 2.4f, 8.0f));
        }

        readonly EndOfGameStats stats = new EndOfGameStats();
        public override bool AddEventIfOnly() => stats.daysSpent < 6;

        public override void Execute() 
        {
            ExecuteAllMonsterEvents();
              Manager.MultiplySpawnChance(RoundManager.Instance.currentLevel, 2f);
            Manager.scrapValueMultiplier *= 4.5f; //Getf(ScaleType.ScrapValue);
            //  Manager.MultiplySpawnChance(RoundManager.Instance.currentLevel, Getf(ScaleType.SpawnMultiplier));
            //  Manager.MultiplySpawnCap(Getf(ScaleType.SpawnCapMultiplier));
        }
    }
}
