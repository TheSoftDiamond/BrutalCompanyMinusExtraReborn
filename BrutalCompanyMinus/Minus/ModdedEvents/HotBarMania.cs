using System;
using System.Collections.Generic;
using UnityEngine;
using BrutalCompanyMinus.Minus.Handlers.Modded;
using System.ComponentModel.Design;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class HotBarMania : MEvent
    {
        public override string Name() => nameof(HotBarMania);

        public static HotBarMania Instance;

        private int newSize;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "You should be able to leave earlier than expected", "Imagine less trips", "I can work with this!" };
            ColorHex = "#008000";
            Type = EventType.Good;
            EventsToRemove = new List<string>() { nameof(HotBarHassle) };
        }

        public override bool AddEventIfOnly() => Compatibility.HotBarPlusPresent;

        public override void Execute()
        {
            if (Compatibility.HotBarPlusPresent)
            {
                // Resize the hotbar randomly
                HotBarPlusCompat.ResizeHotbarRandomly(ref newSize);
            }
        }

        public override void OnShipLeave()
        {
            if (Compatibility.HotBarPlusPresent)
            {
                // TODO: Force players to drop items (if present) from extra slots
                // //This will only be done for both HotBar Events where it's possible to have more item slots than specified.
                // This part is gonna be the "fun" part to code.

                // Reset the hotbar to the original size
                HotBarPlusCompat.ResetHotbar(ref newSize);
            }
        }
    }
}