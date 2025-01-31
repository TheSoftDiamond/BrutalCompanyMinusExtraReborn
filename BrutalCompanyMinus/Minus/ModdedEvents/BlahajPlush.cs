using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using com.github.zehsteam.BlahajPlush;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class BlahajPlush : MEvent
    {
        public override string Name() => nameof(BlahajPlush);

        public static BlahajPlush Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 100; //6
            Descriptions = new List<string>() { "Cute Sharks", "Blahaj Plushie dettected!" };
            ColorHex = "#008000";
            Type = EventType.Good;

            scrapTransmutationEvent = new ScrapTransmutationEvent(
                new Scale(0.5f, 0.008f, 0.5f, 0.9f),
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItemByName("Blahaj Plush", false), rarity = 100 }
            );

            EventsToRemove = new List<string>() { nameof(RealityShift), nameof(Pickles), nameof(TakeyPlush), nameof(Dustpans), nameof(WelcomeToTheFactory), nameof(TakeyGokuPlush), nameof(TakeyGokuPlushBig) };

            ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.0f, 0.005f, 1.0f, 1.5f));
        }

        public override bool AddEventIfOnly()
        {
            if (!Compatibility.blahajPlushPresent) return false;
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
          //  com.github.zehsteam.BlahajPlush.ExtendedConfigEntry<bool> EnableConfiguration = true;
        }
    }
}
