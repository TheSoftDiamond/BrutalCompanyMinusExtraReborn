﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using System.Collections;
using HarmonyLib;
using UnityEngine.Rendering;
using BrutalCompanyMinus.Minus.MonoBehaviours;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class FullAccess : MEvent
    {
        public override string Name() => nameof(FullAccess);

        public static FullAccess Instance;

        public static bool Active = false;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Everything is open!", "Someone left the door's open", "Every burgler's dream", "Experience true exploration", "You wont need be needing keys here" };
            ColorHex = "#008000";
            Type = EventType.Good;

            EventsToRemove = new List<string>() { nameof(FacilityGhost) };
        }

        public override void Execute() => Active = true;

        public override void OnShipLeave() => Active = false;

        public override void OnGameStart() => Active = false;
    }
}
