﻿using BrutalCompanyMinus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class EarlyShip : MEvent
    {
        public override string Name() => nameof(EarlyShip);

        public static EarlyShip Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "The ship has arrived a little early.", "Warp drive activated!", "The early bird catches the worm." };
            ColorHex = "#008000";
            Type = EventType.Good;

            EventsToRemove = new List<string>() { nameof(LateShip), nameof(VeryEarlyShip), nameof(VeryLateShip), nameof(Hell) };

            ScaleList.Add(ScaleType.TimeMax, new Scale(-45.0f, -0.55f, -100.0f, -45.0f));
            ScaleList.Add(ScaleType.TimeMin, new Scale(-60.0f, -0.55f, -100.0f, -60.0f));
        }

        public override void Execute() => Net.Instance.MoveTimeServerRpc(UnityEngine.Random.Range(Getf(ScaleType.MinAmount), Getf(ScaleType.MaxAmount)));
    }
}
