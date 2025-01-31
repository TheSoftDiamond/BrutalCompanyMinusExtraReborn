using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class FoxEv : MEvent
    {
        public override string Name() => nameof(FoxEv);

        public static FoxEv Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 100;
            Descriptions = new List<string>() { "Whats this red furry thing?", "Beware of red bushes" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                Assets.EnemyName.KidnaperFox,
                new Scale(20.0f, 0.8f, 20.0f, 100.0f),
                new Scale(5.0f, 0.2f, 5.0f, 25.0f),
                new Scale(3.0f, 0.06f, 3.0f, 9.0f),
                new Scale(4.0f, 0.08f, 4.0f, 12.0f),
                new Scale(8.0f, 0.02f, 8.0f, 10.0f),
                new Scale(10.0f, 0.03f, 10.0f, 12.0f))
            };

            ScaleList.Add(ScaleType.OutsideEnemyRarity, new Scale(10f, 0.5f, 5f, 15f));
        }

      //  public override bool AddEventIfOnly() => Compatibility.scopophobiaPresent;

        public override void Execute() => ExecuteAllMonsterEvents();
    }
}
