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
    internal class LassoEv : MEvent
    {
        public override string Name() => nameof(LassoEv);

        public static LassoEv Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 4; //5
            Descriptions = new List<string>() { "Lasso man", "Walking rope?", "Removed feature dettected!" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                Assets.EnemyName.Lasso,
                new Scale(10.0f, 0.4f, 10.0f, 50.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(3.0f, 0.06f, 3.0f, 9.0f),
                new Scale(4.0f, 0.09f, 4.0f, 12.0f),
                new Scale(3.0f, 0.12f, 3.0f, 15.0f),
                new Scale(4.0f, 0.16f, 4.0f, 20.0f))
            };

         //   EventsToRemove = new List<string>() { nameof(NoMantitoil) };

        }
        
        public override bool AddEventIfOnly() => Compatibility.NonShippingAuthorisationPresent;

        public override void Execute()
        {
            ExecuteAllMonsterEvents();
         //   com.github.zehsteam.ToilHead.Api.forceMantiToilSpawns = true;
            // com.github.zehsteam.ToilHead.Api.forceSpawns = true;
            // Manager.RemoveSpawn(Assets.EnemyName.CoilHead); 
            // Manager.RemoveSpawn(Assets.antiCoilHead.enemyName = "Spring");
        }

        
    }
}
