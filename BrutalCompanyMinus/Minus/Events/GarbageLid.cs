using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class GarbageLid : MEvent
    {
        public override string Name() => nameof(GarbageLid);

        public static GarbageLid Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;//2
            Descriptions = new List<string>() { "Trash", "GarbageLid... lot of them" };
            ColorHex = "#e84343";
            Type = EventType.Neutral;

            scrapTransmutationEvent = new ScrapTransmutationEvent(
                new Scale(0.5f, 0.008f, 0.5f, 0.9f),
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItemByName("Garbage lid", false), rarity = 95 }
            );

            EventsToRemove = new List<string>() { nameof(RealityShift), nameof(Pickles), nameof(SussyPaintings), /*nameof(TakeyGokuPlush), nameof(TakeyGokuPlushBig),*/ nameof(Dustpans), nameof(Clock), nameof(ControlPad), nameof(ZedDog), nameof(PlasticCup), nameof(ToiletPaper), nameof(FootballScrap) };

            ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.0f, 0.005f, 1.0f, 1.5f));
        }

        public override bool AddEventIfOnly()
        {
          //  if (!Compatibility.takeyPlushPresent & streamerEventsEnabled) return false;
            if (!Manager.transmuteScrap)
            {
                Manager.transmuteScrap = true;
                return true;
            }
            return false;
        }

        public override void Execute()
        {
          //  if (!Compatibility.takeyPlushPresent) return;
            Manager.scrapAmountMultiplier *= Getf(ScaleType.ScrapAmount);
            scrapTransmutationEvent.Execute();
        }
    }
}
