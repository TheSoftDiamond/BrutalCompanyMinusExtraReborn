using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using com.github.zehsteam.ZombiesPlush;
using System.Dynamic;
using BrutalCompanyMinus.Minus.Handlers;
using LethalNetworkAPI;
using GameNetcodeStuff;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class ISP : MEvent
    {
        public override string Name() => nameof(ISP);
       
        public static ISP Instance;

        public static int ISPActive = 1;

        public static LethalNetworkVariable<int> ISPNet = new LethalNetworkVariable<int>(identifier: "ispid") { Value = 1 };

        public override void Initalize()
        {  
            Instance = this;
            
            Weight = 100;
            Descriptions = new List<string>() { "Someone suspect nobody safe, there is a killer around and you need someone to blame!", "Someone suspect nobody knows, but they are comming for you and got nowhere to go!" };
            ColorHex = "#0000FF";  //800000
            Type = EventType.Neutral;

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                Assets.EnemyName.Butler,
                new Scale(9.0f, 0.8f, 10.0f, 60.0f),
                new Scale(5.0f, 0.2f, 5.0f, 12.0f),
                new Scale(3.0f, 0.06f, 3.0f, 9.0f),
                new Scale(4.0f, 0.08f, 4.0f, 12.0f),
                new Scale(8.0f, 0.02f, 8.0f, 10.0f),
                new Scale(10.0f, 0.03f, 10.0f, 12.0f))
            };

            //    scrapTransmutationEvent = new ScrapTransmutationEvent(
            //        new Scale(0.5f, 0.008f, 0.5f, 0.9f),
            //     ScriptableObject.CreateInstance<?>
            //        new SpawnableItemWithRarity() { spawnableItem = Assets.GetItem("networkHandlerPrefab"), rarity = 100 }
            //    );

            //Found error [Warning:BrutalCompanyMinus] GetItem(zombiesPlush) failed, returning an empty item
            //Found error [Warning: Unity Log] Item must be instantiated using the ScriptableObject.CreateInstance method instead of new Item.

          //  EventsToRemove = new List<string>() { nameof(MaskedHorde), nameof(Masked) };

            ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.0f, 0.005f, 1.0f, 1.5f));
          
        }

      /*  public override bool AddEventIfOnly()
        {
       //     if (!Compatibility.zombiesPlushPresent)  return false;
         //   if (!Manager.transmuteScrap)
         //   {
         //       Manager.transmuteScrap = true;
         //       return true;
         //   }
         //   return false;
            PlayerControllerB __instance = new PlayerControllerB();
         if(__instance.playerUsername == "Mr. Spy") 
         {
                return true;
         }
            return false;
        }*/
      
        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Log.LogWarning(" Reseting ISPNet to prevent bugs ");
                ISPNet.Value = 0;
                ISPActive = 0;
                //  Log.LogError(ItemChargerNet.Value);

            }
            ISPActive = 0;
        }
        public override void OnGameStart() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Log.LogWarning(" Reseting ISPNet to prevent bugs ");
                ISPNet.Value = 1;
                ISPActive = 1;
                // Log.LogError(ItemChargerNet.Value);

            }
            ISPActive = 1;
                    
            
        }
        public override void OnShipLeave() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Log.LogWarning(" Reseting ISPNet to prevent bugs ");
                ISPNet.Value = 1;
                ISPActive = 1;
                // Log.LogError(ItemChargerNet.Value);

            }
            ISPActive = 1;
        }
    }       
}
