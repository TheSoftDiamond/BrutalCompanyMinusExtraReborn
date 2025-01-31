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
    internal class SafeBridges : MEvent
    {
        public override string Name() => nameof(SafeBridges);

        public static SafeBridges Instance;

        public static int SafeBridgesActive = 1;

        public static LethalNetworkVariable<int> SafeBridgesNet = new LethalNetworkVariable<int>(identifier: "safebridgesid") { Value = 1 };

        public override void Initalize()
        {
            Instance = this;

            Weight = 100; //4
            Descriptions = new List<string>() { "Titan bridges", "Theese bridges look sturdy" };
            ColorHex = "#008000";
            Type = EventType.Good;

        //    monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
        //        Assets.EnemyName.Manticoil,
        //        new Scale(20.0f, 0.8f, 20.0f, 100.0f),
        //        new Scale(5.0f, 0.2f, 5.0f, 25.0f),
        //        new Scale(3.0f, 0.06f, 3.0f, 9.0f),
        //        new Scale(4.0f, 0.08f, 4.0f, 12.0f),
        //        new Scale(8.0f, 0.02f, 8.0f, 10.0f),
        //        new Scale(10.0f, 0.03f, 10.0f, 12.0f))
        //    };

            EventsToRemove = new List<string>() { nameof(DoorOverdriveEv), nameof(DoorFailure), nameof(ShipCoreFailure) };

        }

        public override bool AddEventIfOnly()
        {
            
          /*  SelectableLevel selectableLevel = new SelectableLevel();
            if (selectableLevel.PlanetName == "AssuranceLevel") 
            {
                return false;
            }
            else if (selectableLevel.PlanetName == "AdamanceLevel")
            {  
                return true; 
            }
            else if (selectableLevel.PlanetName == "VowLevel")
            {
                return true;
            }
            else if (selectableLevel.levelID == 1)
            {
                return true;
            }


            return false;

            */

            if (RoundManager.Instance.currentLevel.PlanetName == "AssuranceLevel")
            {
                return false;
            }
            else if (RoundManager.Instance.currentLevel.PlanetName == "AdamanceLevel")
            {
                return true;
            }
            else if (RoundManager.Instance.currentLevel.PlanetName == "VowLevel")
            {
                return true;
            }
            else if (RoundManager.Instance.currentLevel.levelID == 1)
            {
                return true;
            }


            return false;
            // return Compatibility.NonShippingAuthorisationPresent; RoundManager.Instance.currentLevel.ToString == "AdamanceLevel"
        }

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                //  Log.LogError(" BCME HOST ");
                SafeBridgesNet.Value = 0;
                SafeBridgesActive = 0;
               // Log.LogError(DoorNet.Value);

            }
            SafeBridgesActive = 0;
        }
        public override void OnShipLeave() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
                //  Log.LogWarning(" Reseting DoorNet to prevent bugs ");
                SafeBridgesNet.Value = 1;
                SafeBridgesActive = 1;
              //  Log.LogError(DoorNet.Value);

            }
            SafeBridgesActive = 1;
        }
        public override void OnGameStart() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
                //  Log.LogWarning(" Reseting DoorNet to prevent bugs ");
                SafeBridgesNet.Value = 1;
                SafeBridgesActive = 1;
               // Log.LogError(DoorNet.Value);

            }
            SafeBridgesActive = 1;
        }
    }
}
