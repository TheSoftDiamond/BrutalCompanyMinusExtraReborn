using System;
using System.Collections.Generic;
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
            Descriptions = new List<string>() { "YARHAR", "Five nights...?", "It will be a nightmare time" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;
        }

        public override bool AddEventIfOnly() => Compatibility.KidnapperFoxPresent;

        public override void Execute()
        {

        }
    }
}