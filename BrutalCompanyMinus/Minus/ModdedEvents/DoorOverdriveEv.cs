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
    internal class DoorOverdriveEv : MEvent
    {
        public override string Name() => nameof(DoorOverdriveEv);

        public static DoorOverdriveEv Instance;

        public static int DoorOvActive = 1;

        public static LethalNetworkVariable<int> DoorOvNet = new LethalNetworkVariable<int>(identifier: "doorovid") { Value = 1 };

        public override void Initalize()
        {
            Instance = this;

            Weight = 2; //7
            Descriptions = new List<string>() { "Door system: OVERDRIVE", "Door overdrive" };
            ColorHex = "#008000";
            Type = EventType.Good;

        //    monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
        //        Assets.EnemyName.Manticoil,
        //        new Scale(20.0f, 0.8f, 20.0f, 100.0f),
        //        new Scale(5.0f, 0.2f, 5.0f, 25.0f),
        //        new Scale(3.0f, 0.06f, 3.0f, 9.0f),
        //        new Scale(4.0f, 0.08f, 4.0f, 12.0f),
        //        new Scale(8.0f, 0.02f, 8.0f, 10.0f),
        //        new Scale(10.0f, 0.03f, 10.0f, 12.0f))
        //    };

            EventsToRemove = new List<string>() { nameof(DoorFailure), nameof(ShipCoreFailure) };

        }

        public override bool AddEventIfOnly()
        {
            if (Compatibility.crowdControlPresent == true)
            {
                return false;
            }
            else if (Compatibility.crowdControlPresent != true)
            {
                return true;
            }
            return false;
        }

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogError(" BCME HOST ");
                DoorOvNet.Value = 0;
                DoorOvActive = 0;
              //  Log.LogError(DoorOvNet.Value);

            }
            DoorOvActive = 0;
        }
        public override void OnShipLeave() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting DoorOvNet to prevent bugs ");
                DoorOvNet.Value = 1;
                DoorOvActive = 1;
              //  Log.LogError(DoorOvNet.Value);

            }
            DoorOvActive = 1;
        }
        public override void OnGameStart() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting DoorOvNet to prevent bugs ");
                DoorOvNet.Value = 1;
                DoorOvActive = 1;
              //  Log.LogError(DoorOvNet.Value);

            }
            DoorOvActive = 1;
        }
    }
}
