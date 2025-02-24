using BrutalCompanyMinus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;

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

            EventsToRemove = new List<string>() { nameof(VeryEarlyShip), nameof(EarlyShip), nameof(LateShip), nameof(Hell) };
        }

        public override void Execute()
        {
            if (Configuration.VeryLateShipAdjustment.Value)
            {
                // For slower moving time
                int random = UnityEngine.Random.Range(780, 860);
                Net.Instance.MoveTimeServerRpc(random, 0.139534883721f);
            }
            else
            {
                // For regular moving time
                int Regularrandom = UnityEngine.Random.Range(780, 860);
                Net.Instance.MoveTimeServerRpc(Regularrandom);
            }
        }
    }
}
