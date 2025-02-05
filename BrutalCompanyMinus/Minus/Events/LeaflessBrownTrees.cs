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
    internal class LeaflessBrownTrees : MEvent
    {
        public override string Name() => nameof(LeaflessBrownTrees);

        public static LeaflessBrownTrees Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 4;
            Descriptions = new List<string>() { "These trees look spooky", "Brown leafless trees", "Ok" };
            ColorHex = "#FFFFFF";
            Type = EventType.Neutral;

            EventsToRemove = new List<string>() { nameof(Trees), nameof(LeaflessTrees) };

            ScaleList.Add(ScaleType.MinDensity, new Scale(0.018f, 0.0f, 0.018f, 0.018f));
            ScaleList.Add(ScaleType.MaxDensity, new Scale(0.025f, 0.0f, 0.025f, 0.025f));
        }

        public override void Execute()
        {
            if (LeaflessTrees.Instance.Executed || Trees.Instance.Executed) return;

            Net.Instance.outsideObjectsToSpawn.Add(new OutsideObjectsToSpawn(UnityEngine.Random.Range(Getf(ScaleType.MinDensity) * 0.5f, Getf(ScaleType.MaxDensity) * 0.5f) , (int)Assets.ObjectName.TreeLeaflessBrown1));
        }
    }
}
