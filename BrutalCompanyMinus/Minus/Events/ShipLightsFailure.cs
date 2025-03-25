using LethalNetworkAPI;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class ShipLightsFailure : MEvent
    {
        public override string Name() => nameof(ShipLightsFailure);

        public static ShipLightsFailure Instance;

        public static int LightsActive = 0;

        /*public static LethalNetworkVariable<int> ShipLightsNet = new LethalNetworkVariable<int>(identifier: "shiplightsid") { Value = 1 };*/

        /*  private void NewValue(int newValue)
          {
              Log.LogError("NewValue");
              LightsActive = 0;
          }*/

        public override void Initalize()
        {
            Instance = this;

            Weight = 3; //7
            Descriptions = new List<string>() { "Lighting system: OFFLINE", "The lights are busted!" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;


            EventsToSpawnWith = new List<string>() { nameof(HeavyRain) };

        }


        /*     public override bool AddEventIfOnly()
             {
                 if (!Compatibility.NonShippingAuthorisationPresent == true)
                 {
                     return true;
                 }
                 return false;
             }*/

        public override void Execute() //Not being executed on clients
        {
           // if (NetworkManager.Singleton.IsHost)
          //  {
                //  Log.LogError(" BCME HOST ");
                /*ShipLightsNet.Value = 0;*/
            //    LightsActive = 0;
                // Log.LogError(ShipLightsNet.Value);
            //    foreach (var Light in UnityEngine.Object.FindObjectsByType<ShipLights>(FindObjectsSortMode.None))
               // {
                //    BrutalCompanyMinus.Minus.Handlers.ShipLightsPatches.StartOfRoundPatchesV2.PreventLightsOn(Light);
            //    }


           // }
            /* else if (NetworkManager.Singleton.IsConnectedClient) 
              {   
                  Log.LogError(" BCME CLIENT ");
                 if (ShipLightsNet.Value == 0) 
                  {
                      LightsActive = 0;
                  }

                  ShipLightsNet.OnValueChanged += NewValue;
              } */

           // LightsActive = 0;
            //  Log.LogError("Execute");



       }
        public override void OnShipLeave() //Patch to reset the network int value
        {
            // Log.LogError("OnShipLeave");
          //  if (NetworkManager.Singleton.IsHost)
           // {
                //  Log.LogWarning(" Reseting ShipLightsNet to prevent bugs ");
                /*ShipLightsNet.Value = 1;*/
           //     LightsActive = 1;
                //  Log.LogError(ShipLightsNet.Value);

          //  }
            // Net.Instance.ShipLightsServerRpc(LightsActive);
          //  LightsActive = 1;
          //  BCMECodeSecurityCheck.Modules.EventRelatedStuff.ToggleLightsEvent(true);
        }
        public override void OnGameStart() //Patch to reset the network int value
        {
            // Log.LogError("OnGameStart");
           // if (NetworkManager.Singleton.IsHost)
           // {
                //  Log.LogWarning(" Reseting ShipLightsNet to prevent bugs ");
                /*ShipLightsNet.Value = 1;*/
              //  LightsActive = 1;
                // Log.LogError(ShipLightsNet.Value);

           // }
            // Net.Instance.ShipLightsServerRpc(LightsActive);
           // BCMECodeSecurityCheck.Modules.EventRelatedStuff.ToggleLightsEvent(true);
            //LightsActive = 1;

        }
    }
}