using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class StrawberryDonut : MEvent
    {
        public override string Name() => nameof(StrawberryDonut);

        public static StrawberryDonut Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 100;
            Descriptions = new List<string>() { "Mmmm... strawberry donut" };
            ColorHex = "#008000";
            Type = EventType.Good;

            scrapTransmutationEvent = new ScrapTransmutationEvent(
                new Scale(0.5f, 0.008f, 0.5f, 0.9f),
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem("val3"), rarity = 100 }
            );

            EventsToRemove = new List<string>() { nameof(RealityShift), nameof(Pickles) };

            ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.0f, 0.005f, 1.0f, 1.5f));
        }

     /*   public override bool AddEventIfOnly()
        {
            if (!Compatibility.takeyPlushPresent) return false;
            if (!Manager.transmuteScrap)
            {
                Manager.transmuteScrap = true;
                return true;
            }
            return false;
        }*/

        public override void Execute()
        {
            Manager.scrapAmountMultiplier *= Getf(ScaleType.ScrapAmount);
            scrapTransmutationEvent.Execute();
        }
       /* public override void OnGameStart()
        {
            ScriptableObject.CreateInstance("val3");
            base.OnGameStart();
        }*/
    }
}
