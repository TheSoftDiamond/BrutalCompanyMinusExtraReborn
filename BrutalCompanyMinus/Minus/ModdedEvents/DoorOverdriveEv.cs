using System.Collections.Generic;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class DoorOverdriveEv : MEvent
    {
        public override string Name() => nameof(DoorOverdriveEv);

        public static DoorOverdriveEv Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Door system: OVERDRIVE", "Door overdrive" };
            ColorHex = "#008000";
            Type = EventType.Good;

            EventsToRemove = new List<string>() { nameof(DoorFailure), nameof(ShipCoreFailure) };
        }

        public override void Execute()
        {
            Active = true;
            GameObject netObject = new GameObject("DoorOverdriveEvEvent");
            netObject.AddComponent<DoorOverDriveNet>();
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
