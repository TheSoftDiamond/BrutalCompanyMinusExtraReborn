using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class Dustpans : MEvent
    {
        public override string Name() => nameof(Dustpans);

        public static Dustpans Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1; 
            Descriptions = new List<string>() { "What even is this thing?", "Dust pans?" };
            ColorHex = "#FFA500";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(RealityShift) };

            scrapTransmutationEvent = new ScrapTransmutationEvent(
                new Scale(0.5f, 0.008f, 0.5f, 0.9f),
          //    new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem(Assets.ItemName.AirHorn), rarity = 50 },
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem(Assets.ItemName.DustPan), rarity = 90 }
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
