using System.Collections.Generic;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class LateShip : MEvent
    {
        public override string Name() => nameof(LateShip);

        public static LateShip Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "The ship has arrived a little late.", "Warp drive failed!", "Behind schedule." };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(VeryEarlyShip), nameof(EarlyShip), nameof(VeryLateShip), nameof(Hell) };

            ScaleList.Add(ScaleType.TimeMin, new Scale(50.0f, 1.0f, 50.0f, 150.0f));
            ScaleList.Add(ScaleType.TimeMax, new Scale(60.0f, 1.2f, 60.0f, 180.0f));
        }

        public override void Execute() => Net.Instance.MoveTimeServerRpc(UnityEngine.Random.Range(Getf(ScaleType.TimeMin), Getf(ScaleType.TimeMax)));
    }
}
