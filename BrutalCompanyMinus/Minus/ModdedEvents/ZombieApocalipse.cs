using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using System.Dynamic;
using BrutalCompanyMinus.Minus.Handlers;
using LethalModDataLib.Attributes;
using LethalModDataLib.Enums;

namespace BrutalCompanyMinus.Minus.Events
{
    public class ZombieApocalipse : MEvent
    {
        public override string Name() => nameof(ZombieApocalipse);
       
        public static ZombieApocalipse Instance;

        [ModData(SaveWhen.OnSave, LoadWhen.OnLoad, SaveLocation.CurrentSave)]
        protected static bool streamerEventsEnabled = true;
        
        public override void Initalize()
        {  
            Instance = this;
            
            Weight = 1;
            Descriptions = new List<string>() { "Apocalypse", "The zombie horde is near... RUN!!!" };
            ColorHex = "#800000";  //800000
            Type = EventType.VeryBad;

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                Assets.EnemyName.Masked,
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(4.0f, 0.0f, 4.0f, 4.0f),
                new Scale(6.0f, 0.0f, 6.0f, 6.0f))
            };

            //    scrapTransmutationEvent = new ScrapTransmutationEvent(
            //        new Scale(0.5f, 0.008f, 0.5f, 0.9f),
            //     ScriptableObject.CreateInstance<?>
            //        new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem("networkHandlerPrefab"), rarity = 100 }
            //    );

            //Found error [Warning:BrutalCompanyMinus] GetItem(zombiesPlush) failed, returning an empty item
            //Found error [Warning: Unity Log] Item must be instantiated using the ScriptableObject.CreateInstance method instead of new Item.

            EventsToRemove = new List<string>() { /*nameof(MaskedHorde),*/ nameof(Masked) };

        //    ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.0f, 0.005f, 1.0f, 1.5f));
          
        }

        public override bool AddEventIfOnly() => Compatibility.zombiesPlushPresent & streamerEventsEnabled;
       // {
       //     if (!Compatibility.zombiesPlushPresent)  return false;
         //   if (!Manager.transmuteScrap)
         //   {
         //       Manager.transmuteScrap = true;
         //       return true;
         //   }
         //   return false;
      //  }
      
        public override void Execute()
        { if (!Compatibility.zombiesPlushPresent) return;

            com.github.zehsteam.ZombiesPlush.Api.ForceMaskedZombiesSpawns = true;

            ExecuteAllMonsterEvents();
        }
    }
}
