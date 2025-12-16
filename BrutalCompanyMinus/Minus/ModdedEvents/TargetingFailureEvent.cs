using System.Collections.Generic;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class TargetingFailureEvent : MEvent
    {
        public override string Name() => nameof(TargetingFailureEvent);

        public static TargetingFailureEvent Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 4;
            Descriptions = new List<string>() { "Teleporter targeting system: ERROR", "Teleporter works.... but its not what it seems" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(TeleporterFailure), nameof(ShipCoreFailure) };
        }

        public override void Execute()
        {
            Active = true;
            GameObject netObject = new GameObject("TargetingFailureEvent");
            netObject.AddComponent<TargetingFailureNet>();
        }
        public override void OnShipLeave()
        {
            Active = false;
        }
        public override void OnGameStart()
        {
            Active = false;
        }
    }
}
