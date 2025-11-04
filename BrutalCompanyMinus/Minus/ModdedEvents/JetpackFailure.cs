using System.Collections.Generic;
using Unity.Netcode;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class JetpackFailure : MEvent
    {
        public override string Name() => nameof(JetpackFailure);

        public static JetpackFailure Instance;

        public static Unity.Netcode.NetworkVariable<bool> JetpackUnityNet = new NetworkVariable<bool> { Value = false };

        public override void Initalize()
        {
            Instance = this;

            Weight = 4;
            Descriptions = new List<string>() { "Out of jet fuel", "Warning! Jetpacks not permited in this area!" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;
        }

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetJetpackUnityNetServerRpc(true);
            }
        }
        public override void OnShipLeave()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetJetpackUnityNetServerRpc(false);
            }
        }
        public override void OnGameStart()
        {
            JetpackUnityNet.Value = false; //Using Net throws an error, also there is no need to network this
        }
    }
}
