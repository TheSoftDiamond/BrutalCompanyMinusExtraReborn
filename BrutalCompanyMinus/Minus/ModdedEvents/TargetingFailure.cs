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
    internal class TargetingFailure : MEvent
    {
        public override string Name() => nameof(TargetingFailure);

        public static TargetingFailure Instance;

        public static int TargetingActive = 1;

        public static LethalNetworkVariable<int> TargetingNet = new LethalNetworkVariable<int>(identifier: "targetingid") { Value = 1 };

        public override void Initalize()
        {
            Instance = this;

            Weight = 4; //4
            Descriptions = new List<string>() { "Teleporter targeting system: ERROR", "Teleporter works.... but its not what it seems" };
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

            EventsToRemove = new List<string>() { nameof(TeleporterFailure), nameof(ShipCoreFailure) };

        }
        
     //   public override bool AddEventIfOnly() => Compatibility.NonShippingAuthorisationPresent;

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting TargetingNet to prevent bugs ");
                TargetingNet.Value = 0;
                TargetingActive = 0;
               // Log.LogError(TargetingNet.Value);

            }
            TargetingActive = 0;
        }
        public override void OnShipLeave()
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting TargetingNet to prevent bugs ");
                TargetingNet.Value = 1;
                TargetingActive = 1;
              //  Log.LogError(TargetingNet.Value);

            }
            TargetingActive = 1;
        }
        public override void OnGameStart()
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting TargetingNet to prevent bugs ");
                TargetingNet.Value = 1;
                TargetingActive = 1;
               // Log.LogError(TargetingNet.Value);

            }
            TargetingActive = 1;
        }
    }
}
