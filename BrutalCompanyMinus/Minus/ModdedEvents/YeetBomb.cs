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
    internal class YeetBomb : MEvent
    {
        public override string Name() => nameof(YeetBomb);

        public static YeetBomb Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 2;
            Descriptions = new List<string>() { "You will get launched", "YEET", "Do you like to go Yeet" };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            ScaleList.Add(ScaleType.MinDensity, new Scale(0.01f, 0.0004f, 0.016f, 0.086f));
            ScaleList.Add(ScaleType.MaxDensity, new Scale(0.0168f, 0.00592f, 0.0128f, 0.180f));
        }

        public override bool AddEventIfOnly() => Compatibility.VarietyPresent;
        public override void Execute()
        {
            Manager.insideObjectsToSpawnOutside.Add(new Manager.ObjectInfo(Assets.GetObject("Yeetmine"), UnityEngine.Random.Range(Getf(ScaleType.MinDensity), Getf(ScaleType.MaxDensity))));
        }
    }
}