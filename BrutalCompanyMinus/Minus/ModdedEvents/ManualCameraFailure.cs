using System.Collections.Generic;
using Unity.Netcode;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class ManualCameraFailure : MEvent
    {
        public override string Name() => nameof(ManualCameraFailure);

        public static ManualCameraFailure Instance;

        public static Unity.Netcode.NetworkVariable<bool> CameraUnityNet = new NetworkVariable<bool> { Value = false };

        public override void Initalize()
        {
            Instance = this;

            Weight = 4;
            Descriptions = new List<string>() { "Monitoring system: ERROR", "The screens are broken" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;
        }

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetCameraUnityNetServerRpc(true);
            }
        }
        public override void OnShipLeave()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetCameraUnityNetServerRpc(false);
            }
        }
        public override void OnGameStart()
        {
            CameraUnityNet.Value = false; //Using Net throws an error, also there is no need to network this
        }
    }
}
