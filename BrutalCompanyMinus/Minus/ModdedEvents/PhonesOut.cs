using System.Collections.Generic;
using UnityEngine;
using BrutalCompanyMinus.Minus.Handlers.Modded;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class PhonesOut : MEvent
    {
        public override string Name() => nameof(PhonesOut);

        public static PhonesOut Instance;

        public static bool Active = false;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Please leave a message after the beep..", "Bad reception", "Phone lines are down", "I think the phones are broken" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

        }

        public override bool AddEventIfOnly() => Compatibility.LethalPhonesPresent;

        public override void Execute()
        {

            // Declare the Active state to true globally
            Net.Instance.SetPhonesOutServerRpc(true);

            // Bind the PhonesOut to an GameObject
            GameObject PhonesOutObject = new GameObject("PhonesOutFailureObject");

            // Add the PhonesOutPatches component to the GameObject
            PhonesOutObject.AddComponent<PhonesOutPatches>();
        }
        public override void OnShipLeave()
        {

            // Reset the Active state
            Active = false;
        }
        public override void OnGameStart()
        {
            // Reset the Active state
            Active = false;
        }

        public override void OnLocalDisconnect()
        {
        }
    }
}