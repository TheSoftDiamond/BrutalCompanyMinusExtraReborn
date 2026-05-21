using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class TurretsEverywhere : MEvent
    {
        public override string Name() => nameof(TurretsEverywhere);

        public static TurretsEverywhere Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2; //2
            Descriptions = new List<string>() { "Unsafe outside", "Outside is nothing but turrets" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(RealityShift), nameof(NoMantitoil), nameof(SafeOutside) };

            EventsToSpawnWith = new List<string>() { nameof(OutsideTurrets), nameof(Trees), nameof(Mantitoil) };

        }

        public override bool AddEventIfOnly() => Compatibility.toilheadPresent;
    }
}
