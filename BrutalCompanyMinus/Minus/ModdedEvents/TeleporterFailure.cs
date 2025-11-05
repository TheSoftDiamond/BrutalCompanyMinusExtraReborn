using System.Collections.Generic;
using Unity.Netcode;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class TeleporterFailure : MEvent
    {
        public override string Name() => nameof(TeleporterFailure);

        public static TeleporterFailure Instance;

        public static Unity.Netcode.NetworkVariable<bool> TeleporterUnityNet = new NetworkVariable<bool> { Value = false };

        public override void Initalize()
        {
            Instance = this;

            Weight = 4;
            Descriptions = new List<string>() { "Teleportation system: ERROR", "Teleporter malfunction" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(TargetingFailure) };
        }

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetTeleporterUnityNetServerRpc(true);
            }
        }
        public override void OnShipLeave()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetTeleporterUnityNetServerRpc(false);
            }
        }
        public override void OnGameStart()
        {
            TeleporterUnityNet.Value = false; //Using Net throws an error, also there is no need to network this
        }
    }
}
