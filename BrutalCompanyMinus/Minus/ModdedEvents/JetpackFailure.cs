using System.Collections.Generic;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class JetpackFailure : MEvent
    {
        public override string Name() => nameof(JetpackFailure);

        public static JetpackFailure Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 4;
            Descriptions = new List<string>() { "Out of jet fuel", "Warning! Jetpacks not permited in this area!" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;
        }

        public override void Execute()
        {
            Active = true;
            GameObject netObject = new GameObject("JetpackFailureEvent");
            netObject.AddComponent<JetpackFailureNet>();
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
