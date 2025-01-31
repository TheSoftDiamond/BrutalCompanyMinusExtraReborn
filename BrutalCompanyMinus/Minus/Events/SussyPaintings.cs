using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class SussyPaintings : MEvent
    {
        public override string Name() => nameof(SussyPaintings);

        public static SussyPaintings Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3; //3
            Descriptions = new List<string>() { "All the sussy paintings", "69" };
            ColorHex = "#FFA500";
            Type = EventType.Neutral;

            EventsToRemove = new List<string>() { nameof(RealityShift) };

            scrapTransmutationEvent = new ScrapTransmutationEvent(
                new Scale(0.5f, 0.008f, 0.5f, 0.9f),
          //    new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem(Assets.ItemName.AirHorn), rarity = 50 },
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItemByName("Painting", false), rarity = 95 }
            );

            EventsToRemove = new List<string>() { nameof(RealityShift), nameof(Pickles), /*nameof(TakeyGokuPlush),*/ nameof(Dustpans) };

            ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.0f, 0.005f, 1.0f, 1.5f));
            ScaleList.Add(ScaleType.MaxValue, new Scale(30f, 0.1f, 25f, 39f));
           // ScaleList.Add(ScaleType.ScrapValue, new Scale(30f, 0.1f, 25f, 69f));
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
