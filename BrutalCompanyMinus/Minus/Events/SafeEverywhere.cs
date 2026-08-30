using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrutalCompanyMinus.Minus.Handlers;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class SafeEverywhere : MEvent
    {
        public override string Name() => nameof(SafeEverywhere);

        public static SafeEverywhere Instance;

        public override void Initalize()
        {
            Instance = this;

            EventsToRemove = new List<string>() { nameof(SafeOutside), nameof(SafeInside), nameof(NoOldBird), nameof(NoDogs), nameof(NoGiants), nameof(NoBaboons), nameof(NoWorm), nameof(NoMasks), nameof(NoBirds), nameof(Warzone), nameof(OutsideTurrets), nameof(OutsideLandmines), nameof(Masked), nameof(AllWeather) };

            Weight = 1;
            Descriptions = new List<string>() { "Probably the quietest it's ever been", "You will not regret today!" };
            ColorHex = "#00FFFF";
            Type = EventType.Rare;
            isBetaEvent = true;
            isSpecialEvent = true;
        }

        public override bool AddEventIfOnly() => !Compatibility.lethalEscapePresent;

        public override void Execute()
        {
            Net.Instance.SetEventActiveServerRPC(Name(), true);
            Manager.RemoveSpawn(Assets.EnemyName.Masked);
        }

        public override void OnShipLeave() => Active = false;

        public override void OnGameStart() => Active = false;
    }
}
