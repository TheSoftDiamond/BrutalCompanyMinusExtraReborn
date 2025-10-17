using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class Heatwave : MEvent
    {
        public override string Name() => nameof(Heatwave);

        public static Heatwave Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "The temperature is rising!", "A heatwave is coming!", "It's getting hot in here!" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(BloodMoon), nameof(MajoraMoon), nameof(SolarFlare), nameof(MeteorShower), nameof(Windy), nameof(Gloomy), nameof(Raining), nameof(AllWeather) };
        }

        public override bool AddEventIfOnly() => Compatibility.LethalElementsPresent;

        public override void Execute()
        {
            if (Compatibility.LethalElementsPresent)
            {
                Handlers.Modded.CustomWeather.SetCustomWeather("Heatwave");
            }
        }
    }
}