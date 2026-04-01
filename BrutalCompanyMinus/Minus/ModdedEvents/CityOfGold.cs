using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class CityOfGold : MEvent
    {
        public override string Name() => nameof(CityOfGold);

        public static CityOfGold Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Everything is golden!!", "Gold rush!", "A stash of gold!" };
            ColorHex = "#00FF00";
            Type = EventType.VeryGood;

            EventsToRemove = new List<string>() { nameof(GoldenBars) };

            scrapTransmutationEvent = new ScrapTransmutationEvent(
                new Scale(0.5f, 0.008f, 0.5f, 0.9f),
                new SpawnableItemWithRarity(Assets.GetItem("GoldenEggbeaterLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldMugLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldJugLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("PurifiedMaskLCGoldScrapMod"), 1)    ,
                new SpawnableItemWithRarity(Assets.GetItem("GoldenBellLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldenHornLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GolderBarLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("TalkativeGoldBarLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("JacobsLadderLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldRegisterLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldAxleLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("CuddlyGoldLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldenGruntLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldNuggetGoldScrapShop"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("TiltControlsLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldenGlassLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("CookieGoldPanLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldSignLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldSpringLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldkeeperLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldenFlaskLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldToyRobotLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldenBootsLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldTypeEngineLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldenGuardianLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("ComedyGoldLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldPuzzleLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldBoltLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("GoldenAirhornLCGoldScrapMod"), 1),
                new SpawnableItemWithRarity(Assets.GetItem("DuckOfGoldLCGoldScrapMod"), 1)
            );

            EventsToRemove = new List<string>() { nameof(RealityShift) };

            ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.0f, 0.005f, 1.0f, 1.5f));
        }

        public override bool AddEventIfOnly()
        {
            if (!Compatibility.goldScrapPresent) return false;
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
