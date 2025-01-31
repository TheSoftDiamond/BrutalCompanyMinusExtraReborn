using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class MaskItem : MEvent
    {
        public override string Name() => nameof(MaskItem);

        public static MaskItem Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Funny Faces, Everywhere", "As long as you dont put it on", "Silly masks, guaranteed comedic fun", "Just dont become of one of them with this..." };
            ColorHex = "#FFFFFF";
            Type = EventType.Neutral;

            EventsToRemove = new List<string>() { nameof(RealityShift), nameof(Honk), nameof(EasterEggs) };

            scrapTransmutationEvent = new ScrapTransmutationEvent(
                new Scale(1.0f, 0.0f, 1.0f, 1.0f),
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem(Assets.ItemName.Comedy), rarity = 50 },
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem(Assets.ItemName.Tragedy), rarity = 50 }
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
