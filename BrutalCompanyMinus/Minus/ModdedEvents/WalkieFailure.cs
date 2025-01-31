using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using LethalNetworkAPI;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class WalkieFailure : MEvent
    {
        public override string Name() => nameof(WalkieFailure);

        public static WalkieFailure Instance;

        public static int WalkieActive = 1;

        public static LethalNetworkVariable<int> WalkiesNet = new LethalNetworkVariable<int>(identifier: "walkiesid") { Value = 1 };

        public override void Initalize()
        {
            Instance = this;

            Weight = 4; //7
            Descriptions = new List<string>() { "Radio system: OFFLINE", "Walkies are unusable" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

        //    monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
        //        Assets.EnemyName.Manticoil,
        //        new Scale(20.0f, 0.8f, 20.0f, 100.0f),
        //        new Scale(5.0f, 0.2f, 5.0f, 25.0f),
        //        new Scale(3.0f, 0.06f, 3.0f, 9.0f),
        //        new Scale(4.0f, 0.08f, 4.0f, 12.0f),
        //        new Scale(8.0f, 0.02f, 8.0f, 10.0f),
        //        new Scale(10.0f, 0.03f, 10.0f, 12.0f))
        //    };

          //  EventsToRemove = new List<string>() { nameof(NoMantitoil) };

        }
        
  //      public override bool AddEventIfOnly() => Compatibility.NonShippingAuthorisationPresent;

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogError(" BCME HOST ");
                WalkiesNet.Value = 0;
                WalkieActive = 0;
              //  Log.LogError(WalkiesNet.Value);

            }
            WalkieActive = 0;
        }
        public override void OnShipLeave() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting WalkiesNet to prevent bugs ");
                WalkiesNet.Value = 1;
                WalkieActive = 1;
              //  Log.LogError(WalkiesNet.Value);

            }
            WalkieActive = 1;
        }
        public override void OnGameStart() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting WalkiesNet to prevent bugs ");
                WalkiesNet.Value = 1;
                WalkieActive = 1;
              //  Log.LogError(WalkiesNet.Value);

            }
            WalkieActive = 1;
        }
    }
}
