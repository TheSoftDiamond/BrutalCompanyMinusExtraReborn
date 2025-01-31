using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class TakeyGokuPlushBig : MEvent
    {
        public override string Name() => nameof(TakeyGokuPlushBig);

        public static TakeyGokuPlushBig Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "TakeyGoku... but harmless", "Big plushie" };
            ColorHex = "#A0DB8E";
            Type = EventType.Neutral;

                scrapTransmutationEvent = new ScrapTransmutationEvent(
                    new Scale(0.5f, 0.008f, 0.5f, 0.9f),
                    new SpawnableItemWithRarity() { spawnableItem = Assets.GetItemByName("Takey Goku", false), rarity = 100 }
                );
            

            EventsToRemove = new List<string>() { nameof(RealityShift), nameof(Pickles), nameof(TakeyPlush), nameof(SussyPaintings), nameof(TakeyGokuPlush), nameof(Dustpans), nameof(Clock) };

            ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.0f, 0.005f, 1.0f, 1.5f));
        }

        public override bool AddEventIfOnly()
        {
            if (!Compatibility.takeyPlushPresent) return false;
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
