using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    public class Barbers : MEvent
    {
        public override string Name() => nameof(Barbers);

        public static Barbers Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "What is that noise?", "Barbers", "Wanna haircut?" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                "ClaySurgeon",
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(2.0f, 0.0f, 2.0f, 2.0f),
                new Scale(5.0f, 0.0f, 5.0f, 5.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f))
            };
        }

      //  public override bool AddEventIfOnly() => Compatibility.takeyGokuPresent & Compatibility.takeyGokuEssentialPresent;


        public override void Execute()
        {
                ExecuteAllMonsterEvents();
        }
    }
}
