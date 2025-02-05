using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class ItsPlaytime : MEvent
    {
        public override string Name() => nameof(ItsPlaytime);

        public static ItsPlaytime Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2; //2
            Descriptions = new List<string>() { "Prepare yourself for the next chapter...", "You will regret this...", "The critters and the bigger critters" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            EventsToSpawnWith = new List<string>() { nameof(PlaytimeBig), nameof(Critters) };
        }

        public override bool AddEventIfOnly() => Compatibility.PlaytimePresent && Compatibility.CrittersPresent;
    }
}
