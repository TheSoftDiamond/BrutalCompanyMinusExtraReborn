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
    internal class Bertha : MEvent
    {
        public override string Name() => nameof(Bertha);

        public static Bertha Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "What if you touch it?", "Squarepants", "Say goodbye to your quota", "It will go boom" };
            ColorHex = "#800000";
            Type = EventType.VeryBad;

            ScaleList.Add(ScaleType.MinDensity, new Scale(0.0025f, 0.0001f, 0.004f, 0.0215f));
            ScaleList.Add(ScaleType.MaxDensity, new Scale(0.0042f, 0.00148f, 0.0032f, 0.045f));
        }

        public override bool AddEventIfOnly() => Compatibility.SurfacedPresent;
        public override void Execute()
        {
            Manager.insideObjectsToSpawnOutside.Add(new Manager.ObjectInfo(Assets.GetObject("Bertha"), UnityEngine.Random.Range(Getf(ScaleType.MinDensity), Getf(ScaleType.MaxDensity))));
        }
    }
}
