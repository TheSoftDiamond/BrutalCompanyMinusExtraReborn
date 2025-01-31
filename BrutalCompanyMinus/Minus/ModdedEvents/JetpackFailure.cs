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
    internal class JetpackFailure : MEvent
    {
        public override string Name() => nameof(JetpackFailure);

        public static JetpackFailure Instance;

        public static int JetpackActive = 1;

        public static LethalNetworkVariable<int> JetpackNet = new LethalNetworkVariable<int>(identifier: "jetpackid") { Value = 1 };

        public override void Initalize()
        {
            Instance = this;

            Weight = 4; //7
            Descriptions = new List<string>() { "Out of jet fuel", "Warning! Jetpacks not permited in this area!" };
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
              //  Log.LogWarning(" Reseting JetpackNet to prevent bugs ");
                JetpackNet.Value = 0;
                JetpackActive = 0;
              //  Log.LogError(CameraNet.Value);

            }
            JetpackActive = 0;
        }
        public override void OnShipLeave()
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting JetpackNet to prevent bugs ");
                JetpackNet.Value = 1;
                JetpackActive = 1;
              //  Log.LogError(CameraNet.Value);

            }
            JetpackActive = 1;
        }
        public override void OnGameStart()
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting JetpackNet to prevent bugs ");
                JetpackNet.Value = 1;
                JetpackActive = 1;
              //  Log.LogError(CameraNet.Value);

            }
            JetpackActive = 1;
        }
    }
}
