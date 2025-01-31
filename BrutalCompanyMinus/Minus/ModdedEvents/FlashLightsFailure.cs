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
    internal class FlashLightsFailure : MEvent
    {
        public override string Name() => nameof(FlashLightsFailure);

        public static FlashLightsFailure Instance;

        public static int FlashLightActive = 1;

        public static LethalNetworkVariable<int> FlashLightNet = new LethalNetworkVariable<int>(identifier: "flashlightid") { Value = 1 };

        public override void Initalize()
        {
            Instance = this;

            Weight = 3; //7
            Descriptions = new List<string>() { "I think the batteries leaked", "Who broke the flashlights?" };
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
        
     //   public override bool AddEventIfOnly() => Compatibility.NonShippingAuthorisationPresent;

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting FlashLightNet to prevent bugs ");
                FlashLightNet.Value = 0;
                FlashLightActive = 0;
              //  Log.LogError(CameraNet.Value);

            }
            FlashLightActive = 0;
        }
        public override void OnShipLeave()
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting FlashLightNet to prevent bugs ");
                FlashLightNet.Value = 1;
                FlashLightActive = 1;
              //  Log.LogError(CameraNet.Value);

            }
            FlashLightActive = 1;
        }
        public override void OnGameStart()
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting FlashLightNet to prevent bugs ");
                FlashLightNet.Value = 1;
                FlashLightActive = 1;
              //  Log.LogError(CameraNet.Value);

            }
            FlashLightActive = 1;
        }
    }
}
