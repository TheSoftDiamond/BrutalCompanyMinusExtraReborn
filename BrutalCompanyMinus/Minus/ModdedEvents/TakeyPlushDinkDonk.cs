//using com.github.zehsteam.TakeyPlush.MonoBehaviours;
using com.github.zehsteam.TakeyPlush.Data;
using com.github.zehsteam.TakeyPlush.Enums;
using com.github.zehsteam.TakeyPlush.MonoBehaviours;
using LethalModDataLib.Attributes;
using LethalModDataLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class TakeyPlushDinkDonk : MEvent
    {
        public override string Name() => nameof(TakeyPlushDinkDonk);

        internal static bool isTakeyPlushDinkDonk = false;

        public static TakeyPlushDinkDonk Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1000;//3
            Descriptions = new List<string>() { "Hear ye hear ye", "TakeyDinkDonk" };
            ColorHex = "#008000";
            Type = EventType.Good;

            scrapTransmutationEvent = new ScrapTransmutationEvent(
                new Scale(0.5f, 0.008f, 0.5f, 0.9f),
                new SpawnableItemWithRarity() { spawnableItem = Assets.GetItemByName("Smol Takey", false), rarity = 95 }
            );

            EventsToRemove = new List<string>() { nameof(RealityShift), nameof(Pickles), nameof(TakeyPlush), nameof(SussyPaintings), nameof(TakeyGokuPlush), nameof(TakeyGokuPlushBig), nameof(Dustpans), nameof(Clock) };

            ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.0f, 0.005f, 1.0f, 1.5f));
        }

        public override bool AddEventIfOnly()
        {
            if (!Compatibility.takeyPlushPresent/* & streamerEventsEnabled*/) return false;
            if (!Manager.transmuteScrap)
            {
                Manager.transmuteScrap = true;
                return true;
            }
            return false;
        }

        public override void Execute()
        {
            if (!Compatibility.takeyPlushPresent) return;
            isTakeyPlushDinkDonk = true;
           /* com.github.zehsteam.TakeyPlush.MonoBehaviours.Takey takeyCheck = new com.github.zehsteam.TakeyPlush.MonoBehaviours.Takey();
            if (takeyCheck.isInShipRoom == false)
            {

                foreach (var takey in UnityEngine.Object.FindObjectsByType<com.github.zehsteam.TakeyPlush.MonoBehaviours.Takey>(FindObjectsSortMode.None))
                {
                    takey.Variant.VariantType = com.github.zehsteam.TakeyPlush.Enums.TakeyVariantType.Cake;
                }
            }*/
            Manager.scrapAmountMultiplier *= Getf(ScaleType.ScrapAmount);
            scrapTransmutationEvent.Execute();
        }
    }
}
