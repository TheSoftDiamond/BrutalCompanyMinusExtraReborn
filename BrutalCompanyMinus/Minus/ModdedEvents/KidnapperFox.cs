using System;
using System.Collections.Generic;
using BepInEx;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class KidnapperFox : MEvent
    {
        public override string Name() => nameof(KidnapperFox);

        public static KidnapperFox Instance;

        public static bool Active = false;
        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "The fox roams around", "He is here for the food", "What do you mean he's cute?" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;
        }

        public override bool AddEventIfOnly() => (Compatibility.KidnapperFoxPresent || Assets.ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Enable Special Events?"));

        public override void Execute()
        {

        }
    }
}