using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BrutalCompanyMinus.Minus.Handlers;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using GameNetcodeStuff;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;
using static BrutalCompanyMinus.Net;

namespace BrutalCompanyMinus.Minus.Events
{
    [HarmonyPatch]
    internal class MaskDrop : MEvent
    {
        public override string Name() => nameof(MaskDrop);

        public static MaskDrop Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "You are not alone", "WARNING: SHIPS ARE NEARBY", "They came from outer space.", "Out of the world" };
            ColorHex = "#8B008B";
            Type = EventType.Insane;
            isSpecialEvent = true;
            showTip = true;
            TipTitle = new List<string>() { "WARNING", "ALERT", "DANGER" };
            TipMessages = new List<string>() { "UNAUTHORIZED SHIPS DETECTED", "DETECTING RADIO WAVES", "WHO ARE THEY...?", "UNAUTHORIZED SHIPS IN ORBIT" };

            ScaleList.Add(ScaleType.MinIntervalTime, new Scale(20.0f, -0.015f, 10.0f, 20.0f));
            ScaleList.Add(ScaleType.MaxIntervalTime, new Scale(40.0f, -0.035f, 20.0f, 40.0f));
            ScaleList.Add(ScaleType.Percentage, new Scale(70.0f, 3.55f, 70.0f, 100.0f));
            ScaleList.Add(ScaleType.SpeedMin, new Scale(30.0f, 0.015f, 30.0f, 40.0f));
            ScaleList.Add(ScaleType.SpeedMax, new Scale(50.0f, 0.035f, 50.0f, 70.0f));
            ScaleList.Add(ScaleType.DistanceMax, new Scale(5.0f, 0.015f, 5.0f, 5.0f));
            ScaleList.Add(ScaleType.MinSpawned, new Scale(0.0f, 0.124f, 0.0f, 1.0f));
            ScaleList.Add(ScaleType.MaxSpawned, new Scale(2.0f, 0.57f, 2.0f, 4.0f));
            ScaleList.Add(ScaleType.TimeWaitMinDespawn, new Scale(2.0f, 0.0f, 2.0f, 2.0f));
            ScaleList.Add(ScaleType.TimeWaitMaxDespawn, new Scale(5.0f, 0.0f, 5.0f, 5.0f));
            ScaleList.Add(ScaleType.timeStart, new Scale(0.0f, 0.0f, 0.0f, 0.0f));
            ScaleList.Add(ScaleType.timeEnd, new Scale(1.0f, 0.0f, 1.0f, 1.0f));
        }

        public override void Execute()
        {
            Net.Instance.SetEventActiveServerRPC(Name(), true);

            GameObject MaskDropObj = new GameObject("MaskDropObj");

            MaskDropObj.AddComponent<MaskDropNet>();
        }

        public override void OnShipLeave()
        {
            // Reset the Active state
            Active = false;
        }
        public override void OnGameStart()
        {
            // Reset the Active state
            Active = false;
        }

        public override void OnLocalDisconnect()
        {
        }
    }
}
