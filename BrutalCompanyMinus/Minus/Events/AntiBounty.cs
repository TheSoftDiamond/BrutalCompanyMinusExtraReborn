using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.XR.OpenVR;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class AntiBounty : MEvent
    {
        public static bool AntiBountyActive = false;
        public override string Name() => nameof(AntiBounty);

        public static AntiBounty Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "The Company is issuing a fine for disturbing the wildlife", "Pacifist Run", "You will literally pay for kills..." };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(Bounty) };

            ScaleList.Add(ScaleType.MinValue, new Scale(20.0f, 0.4f, 20.0f, 60.0f));
            ScaleList.Add(ScaleType.MaxValue, new Scale(30.0f, 0.6f, 30.0f, 90.0f));
        }

        public override void Execute()
        {
            Handlers.AntiBounty.enemyObjectIDs.Clear();
            AntiBountyActive = true;
        }

        public override void OnShipLeave() => AntiBountyActive = false;

        public override void OnGameStart() => AntiBountyActive = false;
    }
}
