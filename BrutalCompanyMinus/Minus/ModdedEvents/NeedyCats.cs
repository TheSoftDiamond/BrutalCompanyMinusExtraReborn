using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class NeedyCats : MEvent
    {
        public override string Name() => nameof(NeedyCats);

        public static NeedyCats Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "MEOW!", "Aw, its a kitty cat", "They need your help", "Protect them from the dogs" };
            ColorHex = "#FFFFFF";
            Type = EventType.Neutral;

            scrapTransmutationEvent = new ScrapTransmutationEvent(
                new Scale(0.1f, 0.02f, 0.1f, 0.15f),
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem("CatItem"), rarity = 60 },
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem("CatFoodItem"), rarity = 40 }
            );

            ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.0f, 0.004f, 1.0f, 1.5f));

        }

        public override bool AddEventIfOnly()
        {
            if (!Compatibility.NeedyCatsPresent) return false;
            if (!Manager.transmuteScrap)
            {
                Manager.transmuteScrap = true;
                return true;
            }
            return false;
        }

        public override void Execute()
        {
            Manager.scrapAmountMultiplier *= Getf(ScaleType.ScrapAmount);
            scrapTransmutationEvent.Execute();
        }
    }
}