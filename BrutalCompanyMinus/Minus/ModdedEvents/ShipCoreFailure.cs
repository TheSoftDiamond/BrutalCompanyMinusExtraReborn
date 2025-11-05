using System.Collections.Generic;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class ShipCoreFailure : MEvent
    {
        public override string Name() => nameof(ShipCoreFailure);

        public static ShipCoreFailure Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Ship core failure!", "This is bad, all ship systems are offline" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            EventsToSpawnWith = new List<string>() { nameof(DoorFailure), nameof(ItemChargerFailure), 
                nameof(LeverFailure), nameof(ManualCameraFailure), nameof(TeleporterFailure), nameof(TerminalFailure), nameof(WalkieFailure), nameof(ShipLightsFailure) };
        }

        public override bool AddEventIfOnly() => !Compatibility.SuperEclipsePresent;
    }
}
