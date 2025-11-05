using System.Collections.Generic;
using Unity.Netcode;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class LeverFailure : MEvent
    {
        public override string Name() => nameof(LeverFailure);

        public static LeverFailure Instance;

        public static Unity.Netcode.NetworkVariable<bool> LeverUnityNet = new NetworkVariable<bool> { Value = false };

        public override void Initalize()
        {
            Instance = this;

            Weight = 4;
            Descriptions = new List<string>() { "Ship hydraulics: OFFLINE", "Ship lever malfunction" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;
        }

        public override bool AddEventIfOnly() => !Compatibility.SuperEclipsePresent;

        public override void Execute()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetLeverUnityNetServerRpc(true);
            }
        }
        public override void OnShipLeave()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Net.Instance.SetLeverUnityNetServerRpc(false);
            }
        }
        public override void OnGameStart()
        {
            LeverUnityNet.Value = false; //Using Net throws an error, also there is no need to network this
        }
    }
}
