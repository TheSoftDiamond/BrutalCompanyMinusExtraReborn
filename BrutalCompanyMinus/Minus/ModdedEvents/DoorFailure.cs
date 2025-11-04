using System.Collections.Generic;
using Unity.Netcode;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class DoorFailure : MEvent
    {
        public override string Name() => nameof(DoorFailure);

        public static DoorFailure Instance;

        public static Unity.Netcode.NetworkVariable<bool> DoorUnityNet = new Unity.Netcode.NetworkVariable<bool> { Value = false };

        public override void Initalize()
        {
            Instance = this;

            Weight = 4; //7
            Descriptions = new List<string>() { "Door system: ERROR", "Door malfunction" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(DoorOverdriveEv), nameof(DoorCircuitFailure) };
        }

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetDoorUnityNetServerRpc(true);
            }
        }
        public override void OnShipLeave() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetDoorUnityNetServerRpc(false);
            }
        }
        public override void OnGameStart() //Patch to reset the network int value
        {
            DoorUnityNet.Value = false; //Using Net throws an error, also there is no need to network this
        }
    }
}
