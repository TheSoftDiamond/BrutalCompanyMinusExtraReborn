using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class MeteorShower : MEvent
    {
        public override string Name() => nameof(MeteorShower);

        public static MeteorShower Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "A different kind of shower...", "More meteors?!", "What the.. Meteors..?" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(SolarFlare), nameof(Windy), nameof(Gloomy), nameof(Raining), nameof(AllWeather) };
        }

        public override bool AddEventIfOnly() => Compatibility.CodeRebirthPresent;

        public override void Execute()
        {
            if (Compatibility.CodeRebirthPresent)
            {
                Handlers.Modded.CustomWeather.SetCustomWeather("Meteor Shower");
            }
        }
    }
}