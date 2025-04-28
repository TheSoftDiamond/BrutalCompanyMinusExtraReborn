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
    internal class ItemChargerFailure : MEvent
    {
        public override string Name() => nameof(ItemChargerFailure);

        public static ItemChargerFailure Instance;

        public static LethalNetworkVariable<int> ItemChargerNet = new LethalNetworkVariable<int>(identifier: "itemchargerid") { Value = 1 };

        public static int ChargerActive = 1;
        public override void Initalize()
        {
            Instance = this;

            Weight = 4; //7
            Descriptions = new List<string>() { "Charging station: OFFLINE", "Dont waste your batteries" };
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

            EventsToRemove = new List<string>() { nameof(FlashLightsFailure) };

        }
        
     //   public override bool AddEventIfOnly() => Compatibility.NonShippingAuthorisationPresent;

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting ItemChargerNet to prevent bugs ");
                ItemChargerNet.Value = 0;
                ChargerActive = 0;
              //  Log.LogError(ItemChargerNet.Value);

            }
            ChargerActive = 0;
        }
        public override void OnShipLeave() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting ItemChargerNet to prevent bugs ");
                ItemChargerNet.Value = 1;
                ChargerActive = 1;
               // Log.LogError(ItemChargerNet.Value);

            }
            ChargerActive = 1;
        }

        public override void OnGameStart() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
              //  Log.LogWarning(" Reseting ItemChargerNet to prevent bugs ");
                ItemChargerNet.Value = 1;
                ChargerActive = 1;
               // Log.LogError(ItemChargerNet.Value);

            }
            // Net.Instance.ShipLightsServerRpc(LightsActive);
            ChargerActive = 1;

        }

    }
}
