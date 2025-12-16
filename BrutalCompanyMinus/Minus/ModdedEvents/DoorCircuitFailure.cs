using System.Collections.Generic;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class DoorCircuitFailure : MEvent
    {
        public override string Name() => nameof(DoorCircuitFailure);

        public static DoorCircuitFailure Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Door control circuit: FAILURE", "Door control circuit malfunction" };
            ColorHex = "#FF0000";
            Type = EventType.VeryBad;

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
        }

        public override void Execute()
        {
            Active = true;
            GameObject netObject = new GameObject("DoorCircuitFailureEvent");
            netObject.AddComponent<DoorCircuitFailureNet>();
        }
        public override void OnShipLeave() //Patch to reset the network int value
        {
            Active = false;
        }
        public override void OnGameStart() //Patch to reset the network int value
        {
            Active = false;
        }
    }
}
