using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class SafeInside : MEvent
    {
        public override string Name() => nameof(SafeInside);

        public static SafeInside Instance;

        public override void Initalize()
        {
            Instance = this;

            EventsToRemove = new List<string>() { nameof(SafeOutside), nameof(SafeEverywhere), nameof(NoOldBird), nameof(NoDogs), nameof(NoGiants), nameof(NoBaboons), nameof(NoWorm), nameof(NoMasks), nameof(NoBirds), nameof(Warzone), nameof(OutsideTurrets), nameof(OutsideLandmines), nameof(Masked), nameof(AllWeather) };

            Weight = 1;
            Descriptions = new List<string>() { "Inside is safe!", "It's unusally quiet inside", "You might find dust but that's it.", "You can hear your own footstep's echo as you walk inside." };
            ColorHex = "#00FF00";
            Type = EventType.VeryGood;
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
