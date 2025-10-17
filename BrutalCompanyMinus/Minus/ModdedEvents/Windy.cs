using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class Windy : MEvent
    {
        public override string Name() => nameof(Windy);

        public static Windy Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "Tornado Warning!", "A tornado is approaching!", "Seek shelter immediately!" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(Hurricane), nameof(Hallowed), nameof(Forsaken), nameof(BloodMoon), nameof(MajoraMoon), nameof(SolarFlare), nameof(MeteorShower), nameof(Gloomy), nameof(Raining), nameof(AllWeather) };
        }

        public override bool AddEventIfOnly() => Compatibility.CodeRebirthPresent;

        public override void Execute()
        {
            if (Compatibility.CodeRebirthPresent)
            {
                Handlers.Modded.CustomWeather.SetCustomWeather("Tornado");
            }
        }
    }
}