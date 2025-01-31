using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class EasterEggs : MEvent
    {
        public override string Name() => nameof(EasterEggs);

        public static EasterEggs Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Egg Hunt", "Don't throw it too close", "Can you find them all?" };
            ColorHex = "#FFA500";
            Type = EventType.Neutral;

            EventsToRemove = new List<string>() { nameof(RealityShift), nameof(Honk), nameof(MaskItem) };

            scrapTransmutationEvent = new ScrapTransmutationEvent(
                new Scale(0.5f, 0.1f, 1.0f, 0.9f),
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem(Assets.ItemName.EasterEgg), rarity = 100 }
            );

            ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.0f, 0.005f, 1.0f, 1.5f));
        }

        public override bool AddEventIfOnly()
        {
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
