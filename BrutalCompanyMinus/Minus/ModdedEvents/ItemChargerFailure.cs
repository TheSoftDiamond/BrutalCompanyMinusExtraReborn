using System.Collections.Generic;
using Unity.Netcode;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class ItemChargerFailure : MEvent
    {
        public override string Name() => nameof(ItemChargerFailure);

        public static ItemChargerFailure Instance;

        public static Unity.Netcode.NetworkVariable<bool> ItemChargerUnityNet = new NetworkVariable<bool> { Value = false };
        public override void Initalize()
        {
            Instance = this;

            Weight = 4;
            Descriptions = new List<string>() { "Charging station: OFFLINE", "Dont waste your batteries" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(FlashLightsFailure) };
        }

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetItemChargerUnityNetServerRpc(true);
            }
        }
        public override void OnShipLeave() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetItemChargerUnityNetServerRpc(false);
            }
        }

        public override void OnGameStart() //Patch to reset the network int value
        {
            ItemChargerUnityNet.Value = false; //Using Net throws an error
        }
    }
}
