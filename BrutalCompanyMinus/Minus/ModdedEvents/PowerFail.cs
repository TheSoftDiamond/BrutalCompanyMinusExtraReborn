using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using Malfunctions;
using static Malfunctions.Config;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class PowerFail : MEvent
    {
        public override string Name() => nameof(PowerFail);

        public static PowerFail Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 100; //5
            Descriptions = new List<string>() { "Power failure", "Ship power: Offline" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            
            
                EventsToRemove = new List<string>() { nameof(NoMantitoil) };

        }

        //    public override bool AddEventIfOnly() => Compatibility.toilheadPresent;

        public override void Execute()
        {

            // ExecuteAllMonsterEvents();
            //   com.github.zehsteam.ToilHead.Api.forceMantiToilSpawns = true;
           // Malfunctions.Config.MalfunctionChancePower.BoxedValue = 100;
            MalfunctionPassedDaysPower.Value = 0;
            MalfunctionChancePower.Value = 100;
            MalfunctionMiscAllowConsecutive.Value = true;
        }

        
    }
}
