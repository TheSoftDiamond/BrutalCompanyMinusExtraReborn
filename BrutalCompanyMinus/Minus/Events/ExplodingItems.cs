using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using Unity.Netcode;
using UnityEngine;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using UnityEngine.PlayerLoop;

namespace BrutalCompanyMinus.Minus.Events
{
    [HarmonyPatch]
    internal class ExplodingItems : MEvent
    {
        public override string Name() => nameof(ExplodingItems);

        public static ExplodingItems Instance;

        public static bool Active = false;

        public static float AmountValue;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "Any item could explode", "This is not going to be fun" };
            ColorHex = "#280000";
            Type = EventType.VeryBad;
            Aliases = new List<string>() { "ExplodingScrap", "ScrapExplodes", "ScrapGoBoom" };
            isBetaEvent = true;

            ScaleList.Add(ScaleType.Percentage, new Scale(40.0f, 2.5f, 40.0f, 85.0f));
        }

        //public override bool AddEventIfOnly() => Assets.ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Enable Special Events?");

        public override void Execute()
        {
            Active = true;
            GameObject netObject = new GameObject("ExplodingItemsEvent");
            netObject.AddComponent<ExplodingItemsNet>();

            Net.Instance.SetExplosivePerecentServerRpc(Getf(ScaleType.Percentage));
        }

        public override void OnShipLeave()
        {
            Active = false;
        }

        public override void OnGameStart()
        {
            Active = false;
        }
    }
}