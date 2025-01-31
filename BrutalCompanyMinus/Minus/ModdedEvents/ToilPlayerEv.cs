using GameNetcodeStuff;
using HarmonyLib;
using LethalNetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using static com.github.zehsteam.ToilHead.Api;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class ToilPlayerEv : MEvent
    {
        public override string Name() => nameof(ToilPlayerEv);

        public static ToilPlayerEv Instance;
        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "Perfect disguise", "Turrets cant hurt you", "ToilPlayer?" };
            ColorHex = "#008000";
            Type = EventType.Good;

           // EventsToRemove = new List<string>() { nameof(Coilhead), nameof(AntiCoilhead) };


        }

        public override bool AddEventIfOnly()
        {
            return Compatibility.toilheadPresent && Compatibility.NonShippingAuthorisationPresent;
        }

        public override void Execute()
        {
            if (!Compatibility.toilheadPresent) return;

          //  ExecuteAllMonsterEvents();
            com.github.zehsteam.ToilHead.Api.ForceToilPlayerSpawns = true;
            com.github.zehsteam.ToilHead.Api.ForceToilPlayerMaxSpawnCount = -1;
            

                

        }



    }
}
