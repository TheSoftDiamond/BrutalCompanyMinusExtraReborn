using System;
using System.Collections.Generic;
using BrutalCompanyMinus.Minus.Handlers.Modded;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class Hallowed : MEvent
    {
        public override string Name() => nameof(Hallowed);

        public static Hallowed Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "The spirit of Halloween is upon us", "Spooky vibes are everywhere", "Trick or treat!" };
            ColorHex = "#FFA500";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(Hurricane), nameof(Forsaken), nameof(SolarFlare), nameof(Windy), nameof(Gloomy), nameof(Raining), nameof(AllWeather), nameof(MajoraMoon) };
        }

        public override bool AddEventIfOnly()
        {
            if (Compatibility.IsModPresent("mrov.WeatherRegistry") && CustomWeather.isWeatherPresent("Hallowed"))
            {
                return true;
            }
            return false;
        }

        public override void Execute()
        {
            if (Compatibility.IsModPresent("mrov.WeatherRegistry") && CustomWeather.isWeatherPresent("Hallowed"))
            {
                Handlers.Modded.CustomWeather.SetCustomWeather("Hallowed");
            }
        }
    }
}