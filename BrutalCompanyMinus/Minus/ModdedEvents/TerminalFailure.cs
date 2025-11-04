using System.Collections.Generic;
using Unity.Netcode;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class TerminalFailure : MEvent
    {
        public override string Name() => nameof(TerminalFailure);

        public static TerminalFailure Instance;

        public static Unity.Netcode.NetworkVariable<bool> TerminalUnityNet = new NetworkVariable<bool> { Value = false };

        public override void Initalize()
        {
            Instance = this;

            Weight = 4; //7
            Descriptions = new List<string>() { "Terminal Error", "Terminal console: OFFLINE" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;
        }

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetTerminalUnityNetServerRpc(true);
            }
            
        }
        public override void OnShipLeave()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetTerminalUnityNetServerRpc(false);
            }
        }
        public override void OnGameStart()
        {
            TerminalUnityNet.Value = false; //Using Net throws an error, also there is no need to network this
        }
    }
}
