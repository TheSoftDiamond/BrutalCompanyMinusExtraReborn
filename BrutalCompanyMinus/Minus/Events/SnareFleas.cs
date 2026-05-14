using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class SnareFleas : MEvent
    {
        public override string Name() => nameof(SnareFleas);

        public static SnareFleas Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "Ceiling campers!", "A delicacy", "The finest of creatures", "Look up", "Look down" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            monstersToSpawn = new List<MonsterEvent>() { new MonsterEvent(
                Assets.EnemyName.SnareFlea,
                new Scale(10.0f, 0.4f, 10.0f, 50.0f),
                new Scale(2.0f, 0.08f, 2.0f, 10.0f),
                new Scale(1.0f, 0.05f, 1.0f, 6.0f),
                new Scale(2.0f, 0.08f, 2.0f, 10.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f))
            };
        }

        public override void Execute()
        {
            if (Configuration.enforceEscapeModChecks.Value && !Compatibility.StarLancereNemyEscapePresent)
            {
                Instance.monstersToSpawn[0].minOutside = new Scale(0f, 0f, 0f, 0f);
                Instance.monstersToSpawn[0].maxOutside = new Scale(0f, 0f, 0f, 0f);
                Instance.monstersToSpawn[0].outsideSpawnRarity = new Scale(0f, 0f, 0f, 0f);
            }

            ExecuteAllMonsterEvents();
        }
    }
}