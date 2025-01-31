using GameNetcodeStuff;
using LethalNetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class ButlerAiTechinalDifficulties : MEvent
    {
        public override string Name() => nameof(ButlerAiTechinalDifficulties);

        public static ButlerAiTechinalDifficulties Instance;

        public static int ButlerActive = 1;

        public static LethalNetworkVariable<int> ButlerNet = new LethalNetworkVariable<int>(identifier: "butlerid") { Value = 1 };

        public override void Initalize()
        {
            Instance = this;

            Weight = 100; //4
            Descriptions = new List<string>() { "Warning! ButlerEnemyAI failed to load correctly!", "Seems like Butlers are having some technical difficulties..."  };
            ColorHex = "#FFFF00";
            Type = EventType.Neutral;

           // EventsToSpawnWith = new List<string>() { nameof(Gloomy), nameof(Thumpers), nameof(Spiders), nameof(Masked) };
          //  EventsToRemove = new List<string>() { nameof(HeavyRain), nameof(Raining), nameof(Masked),/* nameof(MaskedHorde),*/ nameof(ZombieApocalipse) };

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                Assets.EnemyName.Butler,
                new Scale(10.0f, 0.4f, 10.0f, 50.0f),
                new Scale(5.0f, 0.2f, 5.0f, 25.0f),
                new Scale(1.0f, 0.05f, 1.0f, 6.0f),
                new Scale(1.0f, 0.07f, 1.0f, 8.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f))
            };
           // ScaleList.Add(ScaleType.ScrapValue, new Scale(15.35f, 3.5115f, 8.35f, 30.5f));
            //   ScaleList.Add(ScaleType.SpawnMultiplier, new Scale(5.25f, 2.0075f, 3.25f, 9.0f));
            //  ScaleList.Add(ScaleType.SpawnCapMultiplier, new Scale(4.4f, 1.016f, 2.4f, 8.0f));
        }

      //  readonly EndOfGameStats stats = new EndOfGameStats();
       // public override bool AddEventIfOnly() => stats.daysSpent < 6;

        public override void Execute() 
        {
            ExecuteAllMonsterEvents();

            if (NetworkManager.Singleton.IsHost)
            {
                //  Log.LogError(" BCME HOST ");
                ButlerNet.Value = 0;
                ButlerActive = 0;
                //  Log.LogError(DoorOvNet.Value);

            }
            ButlerActive = 0;
        }
        public override void OnShipLeave() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
                //  Log.LogWarning(" Reseting DoorOvNet to prevent bugs ");
                ButlerNet.Value = 1;
                ButlerActive = 1;
                //  Log.LogError(DoorOvNet.Value);

            }
            ButlerActive = 1;
        }
        public override void OnGameStart() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
                //  Log.LogWarning(" Reseting DoorOvNet to prevent bugs ");
                ButlerNet.Value = 1;
                ButlerActive = 1;
                //  Log.LogError(DoorOvNet.Value);

            }
            ButlerActive = 1;
        }
    }
}
