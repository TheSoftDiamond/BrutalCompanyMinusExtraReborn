using System.Collections.Generic;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class DoorFailure : MEvent
    {
        public override string Name() => nameof(DoorFailure);

        public static DoorFailure Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 4; //7
            Descriptions = new List<string>() { "Door system: ERROR", "Door malfunction" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(DoorOverdriveEv), nameof(DoorCircuitFailure) };
        }

        public override void Execute()
        {
            // Declare the event active
            Active = true;
            GameObject netObject = new GameObject("DoorFailureEvent");
            netObject.AddComponent<DoorFailureFailureNet>();
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
