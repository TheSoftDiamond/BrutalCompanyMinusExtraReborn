using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class WelcomeToOoblterra : MEvent
    {
        public override string Name() => nameof(WelcomeToOoblterra);

        public static WelcomeToOoblterra Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Welcome to Ooblterra", "Its in the name... Ooblterra!", "Ooblterra is a place of horror and monsters" };
            ColorHex = "#FF0000";
            Type = EventType.VeryBad;

            EventsToRemove = new List<string>() { nameof(Coilhead), nameof(AntiCoilhead), nameof(ToilHead), nameof(Mantitoil), nameof(ToilSlayer), nameof(MantiToilSlayer), nameof(NoSlayers), nameof(NoMantitoil) };

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                "Wanderer",
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(8.0f, 0.35f, 8.0f, 10.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(3.0f, 0.73f, 1.0f, 3.0f),
                new Scale(3.0f, 0.61f, 3.0f, 5.0f)), new MonsterEvent(
                "AdultWanderer",
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(8.0f, 0.35f, 8.0f, 10.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(1.0f, 0.73f, 1.0f, 2.0f),
                new Scale(2.0f, 0.61f, 2.0f, 2.0f)), new MonsterEvent(
                "EyeSecurity",
                new Scale(8.0f, 0.31f, 0.0f, 10.5f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(2.0f, 0.35f, 2.0f, 4.0f),
                new Scale(4.0f, 0.21f, 4.0f, 6.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f)), new MonsterEvent(
                "Gallenarma ",
                new Scale(0.5f, 0.11f, 0.0f, 8.5f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(1.0f, 0.32f, 1.0f, 2.0f),
                new Scale(2.0f, 0.65f, 1.0f, 3.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f))
            };
        }

        public override bool AddEventIfOnly() => Compatibility.OoblterraPresent;

        public override void Execute()
        {
            ExecuteAllMonsterEvents();
        }
    }
}
