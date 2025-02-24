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
    internal class VeryEarlyShip : MEvent
    {
        public override string Name() => nameof(VeryEarlyShip);

        public static VeryEarlyShip Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "The ship has arrived at a very nice time.", "Earlier than usual!", "Before Sunrise!" };
            ColorHex = "#00FF00";
            Type = EventType.VeryGood;

            EventsToRemove = new List<string>() { nameof(LateShip), nameof(EarlyShip), nameof(VeryLateShip), nameof(Hell) };

            ScaleList.Add(ScaleType.MaxAmount, new Scale(-342.0f, -0.55f, -342.0f, -300.0f));
            ScaleList.Add(ScaleType.MinAmount, new Scale(-480.0f, -0.55f, -480.0f, -240.0f));
        }

        public override void Execute() => Net.Instance.MoveTimeServerRpc(UnityEngine.Random.Range(Getf(ScaleType.MinAmount), Getf(ScaleType.MaxAmount)));
    }
}
