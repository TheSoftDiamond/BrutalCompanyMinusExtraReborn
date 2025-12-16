using System.Collections.Generic;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class ItemChargerFailure : MEvent
    {
        public override string Name() => nameof(ItemChargerFailure);

        public static ItemChargerFailure Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 4;
            Descriptions = new List<string>() { "Charging station: OFFLINE", "Dont waste your batteries" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(FlashLightsFailure) };
        }

        public override void Execute()
        {
            Active = false;
            GameObject netObject = new GameObject("ItemChargerFailureEvent");
            netObject.AddComponent<ItemChargerFailureNet>();
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
