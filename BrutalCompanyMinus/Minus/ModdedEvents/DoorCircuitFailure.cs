using System.Collections.Generic;
using Unity.Netcode;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class DoorCircuitFailure : MEvent
    {
        public override string Name() => nameof(DoorCircuitFailure);

        public static DoorCircuitFailure Instance;

        public static Unity.Netcode.NetworkVariable<bool> CircuitUnityNet = new NetworkVariable<bool> { Value = false };

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Door control circuit: FAILURE", "Door control circuit malfunction" };
            ColorHex = "#FF0000";
            Type = EventType.VeryBad;

            EventsToRemove = new List<string>() { nameof(DoorOverdriveEv), nameof(DoorFailure), nameof(ShipCoreFailure) };
        }

        public override bool AddEventIfOnly()
        {
            if (Compatibility.crowdControlPresent == true) 
            {
                return false;
            }
            else if (Compatibility.crowdControlPresent != true)
            {  
                return true; 
            }
            return false;
        }

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetCircuitUnityNetServerRpc(true);
            }
        }
        public override void OnShipLeave() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetCircuitUnityNetServerRpc(false);
            }
        }
        public override void OnGameStart() //Patch to reset the network int value
        {
            CircuitUnityNet.Value = false;
        }
    }
}
