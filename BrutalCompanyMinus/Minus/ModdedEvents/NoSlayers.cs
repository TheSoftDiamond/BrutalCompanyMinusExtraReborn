using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class NoSlayers : MEvent
    {
        public override string Name() => nameof(NoSlayers);

        public static NoSlayers Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "No miniguns at all for today", "Unloaded Hangar corp. has removed all slayers in this area!" };
            ColorHex = "#008000";
            Type = EventType.Remove;

            EventsToRemove = new List<string>() { nameof(Coilhead), nameof(AntiCoilhead), nameof(ToilHead),/* nameof(Mantitoil), */nameof(ToilSlayer), nameof(MantiToilSlayer), nameof(NoMantiToilSlayer), nameof(NoToilSlayer), nameof(AllSlayers) };


            
        }

        public override bool AddEventIfOnly() => Compatibility.toilheadPresent;

        public override void Execute()
        {
            if (!Compatibility.toilheadPresent) return;
            ExecuteAllMonsterEvents();
            com.github.zehsteam.ToilHead.Api.ForceMantiSlayerMaxSpawnCount = 0;
            com.github.zehsteam.ToilHead.Api.ForceMantiSlayerSpawns = false;
            com.github.zehsteam.ToilHead.Api.ForceToilSlayerSpawns = false;
            com.github.zehsteam.ToilHead.Api.ForceToilSlayerMaxSpawnCount = 0;
            com.github.zehsteam.ToilHead.Api.ForceToilHeadMaxSpawnCount = 0;
        }
    }
}
