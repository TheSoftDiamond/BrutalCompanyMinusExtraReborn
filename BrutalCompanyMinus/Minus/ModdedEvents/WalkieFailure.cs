using System.Collections.Generic;
using Unity.Netcode;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class WalkieFailure : MEvent
    {
        public override string Name() => nameof(WalkieFailure);

        public static WalkieFailure Instance;

        public static Unity.Netcode.NetworkVariable<bool> WalkiesUnityNet = new NetworkVariable<bool> { Value = false };

        public override void Initalize()
        {
            Instance = this;

            Weight = 4;
            Descriptions = new List<string>() { "Radio system: OFFLINE", "Walkies are unusable" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;
        }

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetWalkiesUnityNetServerRpc(true);
            }
        }
        public override void OnShipLeave() //Patch to reset the network int value
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetWalkiesUnityNetServerRpc(false);
            }
        }
        public override void OnGameStart() //Patch to reset the network int value
        {
            WalkiesUnityNet.Value = false; //Using Net throws an error, also there is no need to network this
        }
    }
}
