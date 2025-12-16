using System.Collections.Generic;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class TeleporterFailure : MEvent
    {
        public override string Name() => nameof(TeleporterFailure);

        public static TeleporterFailure Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 4;
            Descriptions = new List<string>() { "Teleportation system: ERROR", "Teleporter malfunction" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(TargetingFailureEvent) };
        }

        public override void Execute()
        {
            Active = true;
            GameObject netObject = new GameObject("TeleporterFailureEvent");
            netObject.AddComponent<TeleporterFailureNet>();
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
