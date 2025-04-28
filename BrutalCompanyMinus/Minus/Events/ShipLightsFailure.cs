using LethalNetworkAPI;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using HarmonyLib;
using BrutalCompanyMinus.Minus.Handlers;

namespace BrutalCompanyMinus.Minus.Events
{
    [HarmonyPatch]
    internal class ShipLightsFailure : MEvent
    {
        public override string Name() => nameof(ShipLightsFailure);

        public static ShipLightsFailure Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3; //7
            Descriptions = new List<string>() { "Lighting system: OFFLINE", "The lights are busted!" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;
        }

        public override void Execute()
        {
            Net.Instance.LightsOffServerRpc();
        }

        public override void OnShipLeave()
        {
            Net.Instance.LightsOnServerRpc();
        }

        public override void OnLocalDisconnect()
        {
        }

    }
}