using BrutalCompanyMinus.Minus.Handlers.Modded;
using System.Collections.Generic;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class HotBarHassle : MEvent
    {
        public override string Name() => nameof(HotBarHassle);

        public static HotBarHassle Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "What items do you prioritize?", "Smaller trips this time", "Guess you might not get everything" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;
            EventsToRemove = new List<string>() { nameof(HotBarMania) };
        }

        public override bool AddEventIfOnly() => Compatibility.HotBarPlusPresent;

        public override void Execute()
        {
            if (Compatibility.HotBarPlusPresent)
            {
                // Resize the hotbar randomly
                HotBarPlusCompat.ResizeHotbarRandomlySmall();
            }

            Net.Instance.ResizeHotbarRandomlySmallClientRpc();
        }

        public override void OnShipLeave()
        {
            if (Compatibility.HotBarPlusPresent)
            {
                // Reset the hotbar to the original size
                HotBarPlusCompat.ResetHotbar();
            }

            Net.Instance.ResetHotbarClientRpc();
        }
    }
}