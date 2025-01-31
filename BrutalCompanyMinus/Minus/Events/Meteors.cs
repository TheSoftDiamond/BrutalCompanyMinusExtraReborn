using BrutalCompanyMinus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class Meteors : MEvent
    {
        public override string Name() => nameof(Meteors);

        public static Meteors Instance;

      //  private readonly int meteorStartTime = (TimeOfDay.Instance.hour = 5);

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Maybe looking up isnt such a bad idea", "Meteors, seek cover!", "Falling rocks... great" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            EventsToRemove = new List<string>() { nameof(Raining), nameof(Gloomy), nameof(HeavyRain) };
        }

        public override void Execute() 
        {
            TimeOfDay.Instance.overrideMeteorChance = 1000;
            TimeOfDay.Instance.meteorShowerAtTime = 6f/* (float)meteorStartTime*/;
        }

        public override void OnShipLeave()
        {
            TimeOfDay.Instance.overrideMeteorChance = -1;
            TimeOfDay.Instance.meteorShowerAtTime = -1f;
        }

        public override void OnGameStart()
        {
            TimeOfDay.Instance.overrideMeteorChance = -1;
            TimeOfDay.Instance.meteorShowerAtTime = -1f;
        }
    }
}
