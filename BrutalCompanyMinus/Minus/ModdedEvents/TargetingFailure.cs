using System.Collections.Generic;
using Unity.Netcode;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class TargetingFailure : MEvent
    {
        public override string Name() => nameof(TargetingFailure);

        public static TargetingFailure Instance;

        public static Unity.Netcode.NetworkVariable<bool> TargetingUnityNet = new NetworkVariable<bool> { Value = false };

        public override void Initalize()
        {
            Instance = this;

            Weight = 4;
            Descriptions = new List<string>() { "Teleporter targeting system: ERROR", "Teleporter works.... but its not what it seems" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(TeleporterFailure), nameof(ShipCoreFailure) };
        }

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetTargetingUnityNetServerRpc(true);
            }
        }
        public override void OnShipLeave()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetTargetingUnityNetServerRpc(false);
            }
        }
        public override void OnGameStart()
        {
            TargetingUnityNet.Value = false; //Using Net throws an error, also there is no need to network this
        }
    }
}
