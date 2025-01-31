using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using LethalNetworkAPI;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class RustedDoorsEv : MEvent
    {
        public override string Name() => nameof(RustedDoorsEv);

        public static RustedDoorsEv Instance;

        public static int RustedDoorsActive = 1;

        public static LethalNetworkVariable<int> RustedDoorsNet = new LethalNetworkVariable<int>(identifier: "rusteddoorsid") { Value = 1 };

        public override void Initalize()
        {
            Instance = this;

            Weight = 1000; //7
            Descriptions = new List<string>() { "Rusted doors", "How old are theese doors?" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

        //    monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
        //        Assets.EnemyName.Manticoil,
        //        new Scale(20.0f, 0.8f, 20.0f, 100.0f),
        //        new Scale(5.0f, 0.2f, 5.0f, 25.0f),
        //        new Scale(3.0f, 0.06f, 3.0f, 9.0f),
        //        new Scale(4.0f, 0.08f, 4.0f, 12.0f),
        //        new Scale(8.0f, 0.02f, 8.0f, 10.0f),
        //        new Scale(10.0f, 0.03f, 10.0f, 12.0f))
        //    };

            EventsToRemove = new List<string>() { nameof(FullAccess) };

        }
        
     //   public override bool AddEventIfOnly() => Compatibility.NonShippingAuthorisationPresent;

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogError(" BCME HOST ");
                RustedDoorsNet.Value = 0;
                RustedDoorsActive = 0;
               // Log.LogError(DoorNet.Value);

            }
            RustedDoorsActive = 0;
        }
        public override void OnShipLeave() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
                //  Log.LogWarning(" Reseting DoorNet to prevent bugs ");
                RustedDoorsNet.Value = 1;
                RustedDoorsActive = 1;
              //  Log.LogError(DoorNet.Value);

            }
            RustedDoorsActive = 1;
        }
        public override void OnGameStart() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
                //  Log.LogWarning(" Reseting DoorNet to prevent bugs ");
                RustedDoorsNet.Value = 1;
                RustedDoorsActive = 1;
               // Log.LogError(DoorNet.Value);

            }
            RustedDoorsActive = 1;
        }
    }
}
