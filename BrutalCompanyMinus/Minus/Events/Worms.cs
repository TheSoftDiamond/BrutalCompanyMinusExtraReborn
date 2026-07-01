using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class Worms : MEvent
    {
        public override string Name() => nameof(Worms);

        public static Worms Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "Bug Breach Detected", "The ultimate fine dining experience", "Dont make out with those things" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;
            Aliases = new List<string>() { "Worm" };

            EventsToRemove = new List<string>() { nameof(SnareFleas) };

            monstersToSpawn = new List<MonsterEvent>() { new MonsterEvent(
                Assets.EnemyName.EarthLeviathan,
                new Scale(2.0f, 0.08f, 2.0f, 10.0f),
                new Scale(33.0f, 0.66f, 33.0f, 100.0f),
                new Scale(1.0f, 0.0f, 1.0f, 1.0f),
                new Scale(1.0f, 0.0f, 1.0f, 1.0f),
                new Scale(1.0f, 0.02f, 1.0f, 3.0f),
                new Scale(1.0f, 0.02f, 1.0f, 3.0f)), new MonsterEvent(
                Assets.EnemyName.SnareFlea,
                new Scale(20.0f, 0.8f, 20.0f, 100.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(9.0f, 0.14f, 9.0f, 31.0f),
                new Scale(10.0f, 0.2f, 10.0f, 30.0f),
                new Scale(0.0f, 0.00f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f))
            };
        }

        public override void Execute()
        {
            if (Configuration.enforceEscapeModChecks.Value && !Compatibility.StarLancereNemyEscapePresent)
            {
                Instance.monstersToSpawn[0].minInside = new Scale(0f, 0f, 0f, 0f);
                Instance.monstersToSpawn[0].maxInside = new Scale(0f, 0f, 0f, 0f);
                Instance.monstersToSpawn[0].insideSpawnRarity = new Scale(0f, 0f, 0f, 0f);

                Instance.monstersToSpawn[1].minOutside = new Scale(0f, 0f, 0f, 0f);
                Instance.monstersToSpawn[1].maxOutside = new Scale(0f, 0f, 0f, 0f);
                Instance.monstersToSpawn[1].outsideSpawnRarity = new Scale(0f, 0f, 0f, 0f);
            }

            ExecuteAllMonsterEvents();
        }
    }
}