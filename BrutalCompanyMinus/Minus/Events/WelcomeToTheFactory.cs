using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class WelcomeToTheFactory : MEvent
    {
        public override string Name() => nameof(WelcomeToTheFactory);

        public static WelcomeToTheFactory Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3; //2
            Descriptions = new List<string>() { "Welcome To The Factory!", "Its all metallic??" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToSpawnWith = new List<string>() { nameof(HeavyRain) };

            EventsToRemove = new List<string>() { nameof(RealityShift) };

            scrapTransmutationEvent = new ScrapTransmutationEvent(
                new Scale(0.5f, 0.008f, 0.5f, 0.9f),
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem(Assets.ItemName.BigBolt), rarity = 10 },
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem(Assets.ItemName.CashRegister), rarity = 10 },
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem(Assets.ItemName.MetalSheet), rarity = 10 },
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem(Assets.ItemName.TeaKettle), rarity = 10 },
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem(Assets.ItemName.CookieMoldPan), rarity = 10 },
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem(Assets.ItemName.StopSign), rarity = 10 },
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem(Assets.ItemName.EggBeater), rarity = 10 },
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem(Assets.ItemName.V_TypeEngine), rarity = 10 },
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem(Assets.ItemName.LargeAxle), rarity = 10 },
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem(Assets.ItemName.SteeringWheel), rarity = 10 }
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
           // Manager.SetAtmosphere(Assets.AtmosphereName.Stormy, true);
        }
    }
}
