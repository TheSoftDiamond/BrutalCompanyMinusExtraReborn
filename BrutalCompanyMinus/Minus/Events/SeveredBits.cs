using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using static BrutalCompanyMinus.Net;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class SeveredBits : MEvent
    {
        public override string Name() => nameof(SeveredBits);

        public static SeveredBits Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "Yoshikage Kira", "All the little pieces", "Company has interesting tastes", "I hope these are not real" };
            ColorHex = "#FFFFFF";
            Type = EventType.Neutral;

            scrapTransmutationEvent = new ScrapTransmutationEvent(
                new Scale(0.5f, 0.008f, 0.5f, 0.9f),
                new SpawnableItemWithRarity(Assets.GetItem(Assets.ItemName.SeveredBone), 13),
                new SpawnableItemWithRarity(Assets.GetItem(Assets.ItemName.SeveredBoneRib), 12),
                new SpawnableItemWithRarity(Assets.GetItem(Assets.ItemName.SeveredEar), 12),
                new SpawnableItemWithRarity(Assets.GetItem(Assets.ItemName.SeveredFoot), 12),
                new SpawnableItemWithRarity(Assets.GetItem(Assets.ItemName.SeveredHand), 13),
                new SpawnableItemWithRarity(Assets.GetItem(Assets.ItemName.SeveredHeart), 13),
                new SpawnableItemWithRarity(Assets.GetItem(Assets.ItemName.SeveredThigh), 12),
                new SpawnableItemWithRarity(Assets.GetItem(Assets.ItemName.SeveredTongue), 13)
            );

            ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.0f, 0.005f, 1.0f, 1.1f));
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
