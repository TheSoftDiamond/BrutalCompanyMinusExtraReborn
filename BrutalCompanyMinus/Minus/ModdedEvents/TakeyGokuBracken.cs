
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
    public class TakeyGokuBracken : MEvent
    {
        public override string Name() => nameof(TakeyGokuBracken);

        public static TakeyGokuBracken Instance;
        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "Wait...thats not a plushie", "Thats scary", "TakeyGoku" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                Assets.EnemyName.Bracken,
                new Scale(20.0f, 0.8f, 20.0f, 100.0f),
                new Scale(5.0f, 0.2f, 5.0f, 25.0f),
                new Scale(3.0f, 0.06f, 3.0f, 9.0f),
                new Scale(4.0f, 0.08f, 4.0f, 12.0f),
                new Scale(0.0f, 0.02f, 0.0f, 1.0f),
                new Scale(0.0f, 0.03f, 0.0f, 3.0f))
            };
        }

        public override bool AddEventIfOnly() => Compatibility.takeyGokuPresent & Compatibility.takeyGokuEssentialPresent & Compatibility.officialExternalModulePresent;


        public override void Execute()
        {
            if (Compatibility.takeyPlushPresent & Compatibility.takeyGokuEssentialPresent & Compatibility.officialExternalModulePresent) return;
            com.github.zehsteam.TakeyGokuBracken.Api.ForceTakeyGokuBrackenSpawns = true;
            com.github.zehsteam.TakeyGokuBracken.Api.ForceTakeyGokuBrackenMaxSpawnCount = 25;
                ExecuteAllMonsterEvents();
                
            
        }
    }
}
