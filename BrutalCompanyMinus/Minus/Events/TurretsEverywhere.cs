using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class TurretsEverywhere : MEvent
    {
        public override string Name() => nameof(TurretsEverywhere);

        public static TurretsEverywhere Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2; //2
            Descriptions = new List<string>() { "Unsafe outside", "Outside is nothing but turrets" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(RealityShift), nameof(NoMantitoil), nameof(SafeOutside) };

            EventsToSpawnWith = new List<string>() { nameof(OutsideTurrets), nameof(Trees), nameof(Mantitoil) };

           // scrapTransmutationEvent = new ScrapTransmutationEvent(
           //     new Scale(0.5f, 0.008f, 0.5f, 0.9f),
          //      new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem(Assets.ItemName.AirHorn), rarity = 50 },
           //     new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem(Assets.ItemName.MappingDevice), rarity = 10 }
          //  );

          //  ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.0f, 0.005f, 1.0f, 1.5f));
        }

        public override bool AddEventIfOnly() => Compatibility.toilheadPresent;
        //  {
        //      if (!Manager.transmuteScrap)
        //      {
        //          Manager.transmuteScrap = true;
        //          return true;
        //      }
        //      return false;
        //  }

        //  public override void Execute()
        //  {
        //      Manager.scrapAmountMultiplier *= Getf(ScaleType.ScrapAmount);
        //      scrapTransmutationEvent.Execute();
        //  }
    }
}
