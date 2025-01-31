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

            EventsToRemove = new List<string>() { nameof(VeryEarlyShip), nameof(EarlyShip), nameof(LateShip) };

            ScaleList.Add(ScaleType.MinAmount, new Scale(700.0f, 1.0f, 700.0f, 2300.0f));
            ScaleList.Add(ScaleType.MaxAmount, new Scale(780.0f, 1.2f, 780.0f, 2780.0f));
        }

        public override void Execute() => Net.Instance.MoveTimeServerRpc(UnityEngine.Random.Range(Getf(ScaleType.MinAmount), Getf(ScaleType.MaxAmount)));
    }
}
