using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using MonsterPlushies;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class SpiderPlush : MEvent
    {
        public override string Name() => nameof(SpiderPlush);

        public static SpiderPlush Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Cute spiders", "Arachnophobia?", "These spiders are harmless" };
            ColorHex = "#008000";
            Type = EventType.Good; 

            scrapTransmutationEvent = new ScrapTransmutationEvent(
                new Scale(0.5f, 0.008f, 0.5f, 0.9f),
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem("CoilHeadPlush"), rarity = 1000}
            );
            
            EventsToRemove = new List<string>() { nameof(RealityShift), nameof(Pickles), nameof(TakeyPlush) };
            
            ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.0f, 0.005f, 1.0f, 1.5f));
        }

        public override bool AddEventIfOnly() 
        {
            if (!Compatibility.monsterPlushiesPresent) return false;
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
