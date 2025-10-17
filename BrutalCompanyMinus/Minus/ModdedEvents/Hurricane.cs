using System;
using System.Collections.Generic;
using BrutalCompanyMinus.Minus.Handlers.Modded;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class Hurricane : MEvent
    {
        public override string Name() => nameof(Hurricane);

        public static Hurricane Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "The weather is very violent", "High winds and heavy rain", "This is not a fun time for the outdoors" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            EventsToRemove = new List<string>() { nameof(Hallowed), nameof(Forsaken), nameof(SolarFlare), nameof(Windy), nameof(Gloomy), nameof(Raining), nameof(AllWeather), nameof(MajoraMoon) };
        }

        public override bool AddEventIfOnly()
        {
            if (Compatibility.IsModPresent("mrov.WeatherRegistry") && CustomWeather.isWeatherPresent("Hurricane"))
            {
                return true;
            }
            return false;
        }

        public override void Execute()
        {
            if (Compatibility.IsModPresent("mrov.WeatherRegistry") && CustomWeather.isWeatherPresent("Hurricane"))
            {
                Handlers.Modded.CustomWeather.SetCustomWeather("Hurricane");
            }
        }
    }
}