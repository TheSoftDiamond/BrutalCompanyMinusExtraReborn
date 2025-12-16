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
    internal class BerserkTurrets : MEvent
    {
        public override string Name() => nameof(BerserkTurrets);

        public static BerserkTurrets Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "Absolute Hell", "Berserk... and I dont mean the anime.", "Don't get in line of sight", "You will have a VERY bad day!", "Loudness incoming!" };
            ColorHex = "#280000";
            Type = EventType.VeryBad;
            EventsToSpawnWith = new List<string>() { nameof(Turrets) };
        }

        //public override bool AddEventIfOnly() => Assets.ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Enable Special Events?");

        public override void Execute()
        {
            Active = true;
            GameObject netObject = new GameObject("BerserkTurretsEvent");
            netObject.AddComponent<BerserkTurretsNet>();
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