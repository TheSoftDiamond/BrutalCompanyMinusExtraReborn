using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using System.Dynamic;

namespace BrutalCompanyMinus.Minus.Events
{
    public class RGBShipLights : MEvent
    {
        public override string Name() => nameof(RGBShipLights);

        public static RGBShipLights Instance;

        /// <summary>
        /// Set true to override RGBShipLights Event
        /// </summary>
        public static bool ColorsActiveOverride = false;

        public static bool ColorsActive = false;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "What are these new commands?", "For some reason your ship now supports RGB lighting" };
            ColorHex = "#00ffea";  //800000
            Type = EventType.Neutral;

            /*     monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                     Assets.EnemyName.Masked,
                     new Scale(9.0f, 0.8f, 10.0f, 60.0f),
                     new Scale(5.0f, 0.2f, 5.0f, 12.0f),
                     new Scale(3.0f, 0.06f, 3.0f, 9.0f),
                     new Scale(4.0f, 0.08f, 4.0f, 12.0f),
                     new Scale(8.0f, 0.02f, 8.0f, 10.0f),
                     new Scale(10.0f, 0.03f, 10.0f, 12.0f))
                 };*/

            //    scrapTransmutationEvent = new ScrapTransmutationEvent(
            //        new Scale(0.5f, 0.008f, 0.5f, 0.9f),
            //     ScriptableObject.CreateInstance<?>
            //        new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem("networkHandlerPrefab"), rarity = 100 }
            //    );

            //Found error [Warning:BrutalCompanyMinus] GetItem(zombiesPlush) failed, returning an empty item
            //Found error [Warning: Unity Log] Item must be instantiated using the ScriptableObject.CreateInstance method instead of new Item.

            EventsToRemove = new List<string>() { nameof(ShipLightsFailure), nameof(ShipCoreFailure) };

            //    ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.0f, 0.005f, 1.0f, 1.5f));

        }

        public override bool AddEventIfOnly()/* => Compatibility.zombiesPlushPresent;*/
        {
            if (ColorsActiveOverride == false)
            {
                return true;
            }
            //     if (!Compatibility.zombiesPlushPresent)  return false;
            //   if (!Manager.transmuteScrap)
            //   {
            //       Manager.transmuteScrap = true;
            //       return true;
            //   }
            return false;
        }

        public override void Execute()
        {
            ColorsActive = true;
        }

        public override void OnGameStart()
        {
            ColorsActive = false;
        }

        public override void OnShipLeave()
        {
            ColorsActive = false;
        }



    }
}