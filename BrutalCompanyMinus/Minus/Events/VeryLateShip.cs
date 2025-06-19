using BrutalCompanyMinus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class VeryLateShip : MEvent
    {
        public override string Name() => nameof(VeryLateShip);

        public static VeryLateShip Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "Way behind schedule.", "Quitting Time!!", "We should probably leave!" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            ScaleList.Add(ScaleType.TimeMin, new Scale(780f, 0f, 780f, 780f));
            ScaleList.Add(ScaleType.TimeMax, new Scale(860f, 0f, 860f, 860f));

            EventsToRemove = new List<string>() { nameof(VeryEarlyShip), nameof(EarlyShip), nameof(LateShip), nameof(Hell) };
        }

        public override void Execute()
        {
            if (Configuration.VeryLateShipAdjustment.Value)
            {
                // For slower moving time
                int random = (int)UnityEngine.Random.Range(Getf(ScaleType.TimeMin), Getf(ScaleType.TimeMax));
                Net.Instance.MoveTimeServerRpc(random, 0.139534883721f);
            }
            else
            {
                // For regular moving time
                int Regularrandom = (int)UnityEngine.Random.Range(Getf(ScaleType.TimeMin), Getf(ScaleType.TimeMax));
                Net.Instance.MoveTimeServerRpc(Regularrandom);
            }
        }
    }
}
