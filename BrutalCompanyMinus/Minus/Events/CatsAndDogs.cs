using BepInEx;
using BrutalCompanyMinus;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using static BrutalCompanyMinus.Minus.MonoBehaviours.EnemySpawnCycle;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class CatsAndDogs : MEvent
    {
        public override string Name() => nameof(CatsAndDogs);

        public static CatsAndDogs Instance;


        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "Who let the Dogs (And Cats) Out?", "It's raining Cats And Dogs", "It's raining Dogs And Cats" };
            ColorHex = "#000000";
            Type = EventType.VeryBad;

            EventsToSpawnWith = new List<string>() { nameof(Dogs), nameof(Puma) };
        }

        public override bool AddEventIfOnly() => Assets.ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Enable Beta Events?");


        public override void Execute()
        {
          
        }

        public override void OnShipLeave() { }

        public override void OnGameStart() { }
    }
}
