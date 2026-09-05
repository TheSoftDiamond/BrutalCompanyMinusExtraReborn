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
    internal class DropEnemy : MEvent
    {
        public override string Name() => nameof(DropEnemy);

        public static DropEnemy Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "The Drop Ship was rigged", "The enemies can come from there?!", "Drop ship is dangerous?!" };
            ColorHex = "#280000";
            Type = EventType.VeryBad;
            Aliases = new List<string>() { "BadDropShip", "DropEnemies" };
            isBetaEvent = true;
            isSpecialEvent = true;

            ScaleList.Add(ScaleType.MinSpawned, new Scale(1.0f, 0.0f, 1.0f, 1.0f));
            ScaleList.Add(ScaleType.MaxSpawned, new Scale(4.0f, 0.0f, 4.0f, 4.0f));
            ScaleList.Add(ScaleType.Percentage, new Scale(80.0f, 2.5f, 80.0f, 98.0f));
            ScaleList.Add(ScaleType.DistanceMax, new Scale(5.0f, 0.0f, 5.0f, 5.0f));
        }

        //public override bool AddEventIfOnly() => Assets.ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Enable Special Events?");

        public override void Execute()
        {
            Active = true;
            GameObject netObject = new GameObject("DropEnemyObj");
            netObject.AddComponent<DropEnemyNet>();
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