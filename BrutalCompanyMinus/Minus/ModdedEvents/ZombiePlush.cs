using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class ZombiePlush : MEvent
    {
        public override string Name() => nameof(ZombiePlush);

       // public static bool IsZombiesPlushEvent = false;

        public static ZombiePlush Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "Apocalypse... but better", "Zombies plush" };
            ColorHex = "#008000";
            Type = EventType.Good;

                scrapTransmutationEvent = new ScrapTransmutationEvent(
                    new Scale(0.5f, 0.008f, 0.5f, 0.9f),
                    new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem("Smol Zombies"), rarity = 100 }
                );



            EventsToRemove = new List<string>() { nameof(RealityShift), nameof(Pickles), /*nameof(TakeyPlush),*/ nameof(SussyPaintings), /*nameof(TakeyGokuPlushBig),*/ nameof(Dustpans), nameof(Clock) };

            ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.0f, 0.005f, 1.0f, 1.5f));
        }

        public override bool AddEventIfOnly()
        {
            if (!Compatibility.zombiesPlushPresent) return false;
            if (!Manager.transmuteScrap)
            {
                Manager.transmuteScrap = true;
                return true;
            }
            return false;
        }

        /*  public unsafe override void Execute()
          {
              ZombiesBehaviour behaviour = new ZombiesBehaviour();
              behaviour.SetIsPlayingMusicOnServer(false);
              Manager.scrapAmountMultiplier *= Getf(ScaleType.ScrapAmount);
              scrapTransmutationEvent.Execute();
          }*/

        public override void Execute()
        {
            /* foreach (var behaviour in UnityEngine.Object.FindObjectsByType<ZombiesBehaviour>(FindObjectsSortMode.None))
             {
               //  Log.LogError("Execute");
                 behaviour.SetIsPlayingMusicOnServer(false);
             }*/
            
           /* if (!NetworkManager.Singleton.IsServer)
            {
                IsZombiesPlushEvent = true;
            }*/

            Manager.scrapAmountMultiplier *= Getf(ScaleType.ScrapAmount);
            scrapTransmutationEvent.Execute();
        }
      /*  public override void OnGameStart()
        {
            if (!NetworkManager.Singleton.IsServer)
            {
                IsZombiesPlushEvent = false;
            }
        }

        public override void OnShipLeave()
        {
            if (!NetworkManager.Singleton.IsServer)
            {
                IsZombiesPlushEvent = false;
            }
        }*/

    }
}
