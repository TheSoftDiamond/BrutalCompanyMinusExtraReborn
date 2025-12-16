using System.Collections.Generic;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class LeverFailure : MEvent
    {
        public override string Name() => nameof(LeverFailure);

        public static LeverFailure Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 4;
            Descriptions = new List<string>() { "Ship hydraulics: OFFLINE", "Ship lever malfunction" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;
        }

        public override bool AddEventIfOnly() => !Compatibility.SuperEclipsePresent;

        public override void Execute()
        {
            Active = true;
            GameObject netObject = new GameObject("LeverEvent");
            netObject.AddComponent<LeverNet>();
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
