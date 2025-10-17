using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class SolarFlare : MEvent
    {
        public override string Name() => nameof(SolarFlare);

        public static SolarFlare Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "Solar Flare Incoming!", "A massive solar flare is approaching!" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(Hurricane), nameof(Hallowed), nameof(Forsaken), nameof(BloodMoon), nameof(MajoraMoon), nameof(Heatwave), nameof(MeteorShower), nameof(Windy), nameof(Gloomy), nameof(Raining), nameof(AllWeather) };
        }

        public override bool AddEventIfOnly() => Compatibility.LethalElementsPresent;

        public override void Execute()
        {
            if (Compatibility.LethalElementsPresent)
            {
                Handlers.Modded.CustomWeather.SetCustomWeather("Solar Flare");
            }
        }
    }
}