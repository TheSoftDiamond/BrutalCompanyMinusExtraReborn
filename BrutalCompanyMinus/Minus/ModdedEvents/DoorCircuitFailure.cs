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
    internal class DoorCircuitFailure : MEvent
    {
        public override string Name() => nameof(DoorCircuitFailure);

        public static DoorCircuitFailure Instance;

        public static int DoorCircuitActive = 1;

        public static LethalNetworkVariable<int> DoorCircuitNet = new LethalNetworkVariable<int>(identifier: "doorcircuitid") { Value = 1 };

        public override void Initalize()
        {
            Instance = this;

            Weight = 2; //4
            Descriptions = new List<string>() { "Door control circuit: FAILURE", "Door control circuit malfunction" };
            ColorHex = "#FF0000";
            Type = EventType.VeryBad;

        //    monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
        //        Assets.EnemyName.Manticoil,
        //        new Scale(20.0f, 0.8f, 20.0f, 100.0f),
        //        new Scale(5.0f, 0.2f, 5.0f, 25.0f),
        //        new Scale(3.0f, 0.06f, 3.0f, 9.0f),
        //        new Scale(4.0f, 0.08f, 4.0f, 12.0f),
        //        new Scale(8.0f, 0.02f, 8.0f, 10.0f),
        //        new Scale(10.0f, 0.03f, 10.0f, 12.0f))
        //    };

            EventsToRemove = new List<string>() { nameof(DoorOverdriveEv), nameof(DoorFailure), nameof(ShipCoreFailure) };

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
           // return Compatibility.NonShippingAuthorisationPresent;
        }

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogError(" BCME HOST ");
                DoorCircuitNet.Value = 0;
                DoorCircuitActive = 0;
               // Log.LogError(DoorNet.Value);

            }
            DoorCircuitActive = 0;
        }
        public override void OnShipLeave() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting DoorNet to prevent bugs ");
                DoorCircuitNet.Value = 1;
                DoorCircuitActive = 1;
              //  Log.LogError(DoorNet.Value);

            }
            DoorCircuitActive = 1;
        }
        public override void OnGameStart() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting DoorNet to prevent bugs ");
                DoorCircuitNet.Value = 1;
                DoorCircuitActive = 1;
               // Log.LogError(DoorNet.Value);

            }
            DoorCircuitActive = 1;
        }
    }
}
