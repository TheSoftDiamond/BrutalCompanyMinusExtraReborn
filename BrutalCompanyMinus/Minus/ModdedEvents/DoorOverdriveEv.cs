using System.Collections.Generic;
using Unity.Netcode;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class DoorOverdriveEv : MEvent
    {
        public override string Name() => nameof(DoorOverdriveEv);

        public static DoorOverdriveEv Instance;

        public static Unity.Netcode.NetworkVariable<bool> DoorOvUnityNet = new NetworkVariable<bool> { Value = false };

        public override void Initalize()
        {
            Instance = this;

            Weight = 2; //7
            Descriptions = new List<string>() { "Door system: OVERDRIVE", "Door overdrive" };
            ColorHex = "#008000";
            Type = EventType.Good;

            EventsToRemove = new List<string>() { nameof(DoorFailure), nameof(ShipCoreFailure) };
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
                Net.Instance.SetDoorOvUnityNetServerRpc(true);
            }
        }
        public override void OnShipLeave() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetDoorOvUnityNetServerRpc(false);
            }
        }
        public override void OnGameStart() //Patch to reset the network int value
        {
            DoorOvUnityNet.Value = false; //Using Net throws an error, also there is no need to network this
        }
    }
}
