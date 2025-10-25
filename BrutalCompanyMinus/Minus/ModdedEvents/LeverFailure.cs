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
    internal class LeverFailure : MEvent
    {
        public override string Name() => nameof(LeverFailure);

        public static LeverFailure Instance;

        public static int LeverActive = 1;

        public static LethalNetworkVariable<int> LeverNet = new LethalNetworkVariable<int>(identifier: "leverid") { Value = 1 };

        public override void Initalize()
        {
            Instance = this;

            Weight = 4; //7
            Descriptions = new List<string>() { "Ship hydraulics: OFFLINE", "Ship lever malfunction" };
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

        //  public override bool AddEventIfOnly() => Compatibility.NonShippingAuthorisationPresent;

        public override bool AddEventIfOnly() => !Compatibility.SuperEclipsePresent;

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting LeverNet to prevent bugs ");
                LeverNet.Value = 0;
                LeverActive = 0;
              //  Log.LogError(LeverNet.Value);

            }
            LeverActive = 0;

        }
        public override void OnShipLeave()
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting LeverNet to prevent bugs ");
                LeverNet.Value = 1;
                LeverActive = 1;
              //  Log.LogError(LeverNet.Value);

            }
            LeverActive = 1;
        }
        public override void OnGameStart()
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting LeverNet to prevent bugs ");
                LeverNet.Value = 1;
                LeverActive = 1;
              //  Log.LogError(LeverNet.Value);

            }
            LeverActive = 1;
        }
    }
}
