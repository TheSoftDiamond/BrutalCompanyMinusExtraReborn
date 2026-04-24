using System;
using System.Collections.Generic;
using BrutalCompanyMinus.Minus.Handlers;
using BrutalCompanyMinus.Minus.Handlers.Modded;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class CruiserFailure : MEvent
    {
        public override string Name() => nameof(CruiserFailure);

        public static CruiserFailure Instance;

        public static bool Active = false;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "You should have got that extended warranty", "CRUISER SYSTEM FAILURE", "The cruiser has broken down", "Vehicles are not reliable today!", "Your ignition subscription expired", "We are here for your extended warranty" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;
        }

        public override void Execute()
        {
            Net.Instance.SetCruiserOfflineServerRpc();

            // Declare the event active
            Active = true;

            if (Compatibility.ScanVanPresent)
            {
                GameObject netObject = new GameObject("ScanVanFailure");
                //Add the TimeChaosEvent component to it
                netObject.AddComponent<ScanVanNet>();
            }

        }

        public override void OnShipLeave() //Patch to reset the network int value
        {
            // Reset the Active state
            Active = false;
        }

        public override void OnGameStart() //Patch to reset the network int value
        {
            // Reset the Active state
            Active = false;
        }
    }
}
