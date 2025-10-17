using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class BloodMoon : MEvent
    {
        public override string Name() => nameof(BloodMoon);

        public static BloodMoon Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "I would not be upset if you could not collect any scrap", "Avoid monsters at all costs", "Did you know that it is a Blood Moon?", "Somehow I think this is worse than Eclipsed", "Nowhere is gonna be safe", "All hail the blood moon!" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            EventsToRemove = new List<string>() { nameof(Hurricane), nameof(Hallowed), nameof(Forsaken), nameof(SolarFlare), nameof(Windy), nameof(Gloomy), nameof(Raining), nameof(AllWeather), nameof(MajoraMoon) };
        }

        public override bool AddEventIfOnly() => Compatibility.LegendWeathersPresent;

        public override void Execute()
        {
            if (Compatibility.LegendWeathersPresent)
            {
                Handlers.Modded.CustomWeather.SetCustomWeather("Blood Moon");
            }
        }
    }
}