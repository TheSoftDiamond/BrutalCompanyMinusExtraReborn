using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using Unity.Netcode;
using UnityEngine;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;

namespace BrutalCompanyMinus.Minus.Events
{
    [HarmonyPatch]
    internal class TimeChaos : MEvent
    {
        public override string Name() => nameof(TimeChaos);

        public static TimeChaos Instance;

        public static bool Active = false;

        public static float timeMultiplier;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "MADE IN HEAVEN", "Time is very messed up!", "Look at the sky" };
            ColorHex = "#CF9FFF";
            Type = EventType.VeryBad;

            EventsToRemove = new List<string>() { nameof(VeryEarlyShip), nameof(EarlyShip), nameof(LateShip), nameof(VeryLateShip), nameof(Hell) };

            ScaleList.Add(ScaleType.TimeSettings, new Scale(1.0001f, 0.00001f, 1.0001f, 1.001f));
        }

        public override bool AddEventIfOnly() => Assets.ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Enable Special Events?");

        public override void Execute()
        {
            // Declare the event active
            Active = true;
            // Get the time multiplier from the scale
            timeMultiplier = Getf(ScaleType.TimeSettings);
            // Create the Time Chaos Event
            GameObject netObject = new GameObject("TimeChaosEvent");
            //Add the TimeChaosEvent component to it
            netObject.AddComponent<TimeChaosEvent>();
        }

        public override void OnShipLeave()
        {
            Active = false;
            /*
            GameObject netObject = GameObject.Find("TimeChaosEvent");
            if (netObject != null)
            {
                GameObject.Destroy(netObject);
            }
            */

            Manager.timeSpeedMultiplier = 1.0f;
        }

        public override void OnGameStart()
        {
            // Declare the event false
            Active = false;
        }


    }
}