using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class MajoraMoon : MEvent
    {
        public override string Name() => nameof(MajoraMoon);

        public static MajoraMoon Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "The moon falls", "It has a face on it", "Time is ticking" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            EventsToRemove = new List<string>() { nameof(Hurricane), nameof(Hallowed), nameof(Forsaken), nameof(SolarFlare), nameof(Windy), nameof(Gloomy), nameof(Raining), nameof(AllWeather), nameof(BloodMoon), nameof(VeryLateShip), nameof(Hell), nameof(LateShip) };
        }

        public override bool AddEventIfOnly() => Compatibility.LegendWeathersPresent;

        public override void Execute()
        {
            if (Compatibility.LegendWeathersPresent)
            {
                Handlers.Modded.CustomWeather.SetCustomWeather("Majora Moon");
            }
        }
    }
}