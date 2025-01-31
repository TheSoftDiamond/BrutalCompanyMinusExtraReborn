
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
    public class GokuBracken : MEvent
    {
        public override string Name() => nameof(GokuBracken);

        public static GokuBracken Instance;
        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Best Paired with Goku Mod", "Hey its me Goku", "It's the best day ever", "Goku Day", "How to Goku", "Neck Snapping Day", "Your neck will snap today... But worse", "You are feeling powerful today" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            EventsToRemove = new List<string>() { nameof(Bracken) };

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                Assets.EnemyName.Bracken,
                new Scale(30.0f, 0.5f, 30.0f, 80.0f),
                new Scale(33.0f, 0.7f, 33.0f, 100.0f),
                new Scale(1.0f, 0.05f, 1.0f, 4.0f),
                new Scale(5.0f, 0.1f, 2.0f, 12.0f),
                new Scale(1.0f, 0.02f, 2.0f, 6.0f),
                new Scale(6.0f, 0.08f, 5.0f, 12.0f))
            };
        }

        public override bool AddEventIfOnly() => Compatibility.takeyGokuEssentialPresent;


        public override void Execute()
        {
            ExecuteAllMonsterEvents();


        }
    }
}
