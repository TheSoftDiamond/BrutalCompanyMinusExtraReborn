using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class NoMantitoil : MEvent
    {
        public override string Name() => nameof(NoMantitoil);

        public static NoMantitoil Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 5;
            Descriptions = new List<string>() { "No flying turrets today", "You wont see any flying turrets today", "No air snipers", "NoMantiToils...phew..." };
            ColorHex = "#008000";
            Type = EventType.Remove;

      //      EventsToSpawnWith = new List<string>() { nameof(NoBirds)}; Deprecated
            EventsToRemove = new List<string>() { nameof(Mantitoil), nameof(NoSlayers), nameof(ToilSlayer) };
        }

        public override bool AddEventIfOnly() => Compatibility.toilheadPresent;

        public override void Execute()
        {
            if (!Compatibility.toilheadPresent) return;
            ExecuteAllMonsterEvents();
       //     Manager.RemoveSpawn(Assets.EnemyName.Manticoil);  Forgot to remove that
       //     com.github.zehsteam.ToilHead.Api.forceMantiToilSpawns = false; Deprecated
             com.github.zehsteam.ToilHead.Api.ForceMantiToilMaxSpawnCount = 0;
            // Manager.RemoveSpawn(Assets.EnemyName.CoilHead); Deprecated
            // Manager.RemoveSpawn(Assets.antiCoilHead.enemyName = "Spring"); Deprecated
        }

        
    }
}
