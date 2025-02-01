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
    internal class ShipCoreFailure : MEvent
    {
        public override string Name() => nameof(ShipCoreFailure);

        public static ShipCoreFailure Instance;

        public static bool DoorActive = true;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2; //7
            Descriptions = new List<string>() { "Ship core failure!", "This is bad, all ship systems are offline" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

        //    monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
        //        Assets.EnemyName.Manticoil,
        //        new Scale(20.0f, 0.8f, 20.0f, 100.0f),
        //        new Scale(5.0f, 0.2f, 5.0f, 25.0f),
        //        new Scale(3.0f, 0.06f, 3.0f, 9.0f),
        //        new Scale(4.0f, 0.08f, 4.0f, 12.0f),
        //        new Scale(8.0f, 0.02f, 8.0f, 10.0f),
        //        new Scale(10.0f, 0.03f, 10.0f, 12.0f))
        //    };

            EventsToSpawnWith = new List<string>() { nameof(ShipLightsFailure), nameof(DoorFailure), nameof(ItemChargerFailure), 
                nameof(LeverFailure), nameof(ManualCameraFailure), nameof(TeleporterFailure), nameof(TerminalFailure), nameof(WalkieFailure) };

        }

        public override bool AddEventIfOnly() => !Compatibility.SuperEclipsePresent;

        //  public override bool AddEventIfOnly() => Compatibility.NonShippingAuthorisationPresent;

        /*    public override void Execute()
            {
              //  ExecuteAllMonsterEvents();
              //  com.github.zehsteam.ToilHead.Api.forceMantiToilSpawns = true;
                // com.github.zehsteam.ToilHead.Api.forceSpawns = true;
                // Manager.RemoveSpawn(Assets.EnemyName.CoilHead); 
                // Manager.RemoveSpawn(Assets.antiCoilHead.enemyName = "Spring");
                DoorActive = false;
            }
            public override void OnShipLeave()
            {
                //  ExecuteAllMonsterEvents();
                //  com.github.zehsteam.ToilHead.Api.forceMantiToilSpawns = true;
                // com.github.zehsteam.ToilHead.Api.forceSpawns = true;
                // Manager.RemoveSpawn(Assets.EnemyName.CoilHead); 
                // Manager.RemoveSpawn(Assets.antiCoilHead.enemyName = "Spring");
                DoorActive = true;
            }
            public override void OnGameStart()
            {
                //  ExecuteAllMonsterEvents();
                //  com.github.zehsteam.ToilHead.Api.forceMantiToilSpawns = true;
                // com.github.zehsteam.ToilHead.Api.forceSpawns = true;
                // Manager.RemoveSpawn(Assets.EnemyName.CoilHead); 
                // Manager.RemoveSpawn(Assets.antiCoilHead.enemyName = "Spring");
                DoorActive = true;
            } */
    }
}
