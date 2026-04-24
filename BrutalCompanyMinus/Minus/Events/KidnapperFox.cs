using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BrutalCompanyMinus.Minus.Handlers;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;
using static BrutalCompanyMinus.Net;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class KidnapperFox : MEvent
    {
        public override string Name() => nameof(KidnapperFox);

        public static KidnapperFox Instance;

        public static bool Active = false;

        public override void Initalize()
        {
            Instance = this;

            Weight = 3;
            Descriptions = new List<string>() { "He's back, and he's not happy." };
            ColorHex = "#FF0000";
            Type = EventType.Bad;

            monsterEvents = new List<MonsterEvent>() { new MonsterEvent(
                Assets.EnemyName.BushWolf,
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(0.0f, 0.0f, 0.0f, 0.0f),
                new Scale(1.0f, 0.0f, 1.0f, 1.0f),
                new Scale(1.0f, 0.10f, 1.0f, 4.0f))
            };

            ScaleList.Add(ScaleType.minMold, new Scale(10, 1f, 10f, 15f));
            ScaleList.Add(ScaleType.maxMold, new Scale(30f, 2f, 30f, 40f));
        }

        //public override bool AddEventIfOnly() => (Assets.ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Enable Beta Events?"));


        public override void Execute()
        {
            //Grab the state before
            Net.Instance.SaveOriginalMoldPreviousDataServerRpc();

            // Declare the Active state to true globally
            Net.Instance.SetKidnapperFoxNetServerRpc(true);

            // Bind the FlashLightFailure to an GameObject
            GameObject KidnapperFoxNet = new GameObject("KidnapperFoxNet");

            // Add the FlashlightItemChargerPatches component to the GameObject
            KidnapperFoxNet.AddComponent<kidnapperFoxNet>();

            // Actually spawn mold
            Net.Instance.SetMoldServerRpc(true, (int)UnityEngine.Random.Range(Getf(ScaleType.minMold), Getf(ScaleType.maxMold)));

            // KidnapperFox Spawn
            ExecuteAllMonsterEvents();
        }

        public override void OnShipLeave()
        {

            // Reset the Active state
            RestoreOriginalStates();
            Active = false;
        }
        public override void OnGameStart()
        {
            // Reset the Active state
            Active = false;
        }

        public override void OnLocalDisconnect()
        {
            try
            {
                RestoreOriginalStatesOnDisconnect();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error restoring original states on disconnect: {ex.Message}");
            }
        }


        public void RestoreOriginalStates()
        {
            SelectableLevel currentLevel = RoundManager.Instance.currentLevel;
            if (currentLevel != null)
            {
                if (Manager.originalMoonMoldData.TryGetValue(currentLevel.PlanetName, out var data))
                {
                    Log.LogInfo("Resetting the moon mold data for " + currentLevel.PlanetName);
                    (currentLevel.canSpawnMold, currentLevel.moldSpreadIterations) = data;
                }
            }

            Manager.originalMoonMoldData.Clear();
        }

        public void RestoreOriginalStatesOnDisconnect()
        {
            foreach (var entry in Manager.originalMoonMoldData)
            {
                string planetName = entry.Key;
                var (prevState, prevIterations) = entry.Value;

                SelectableLevel level = StartOfRound.Instance.levels.FirstOrDefault(l => l.PlanetName == planetName);

                if (level != null)
                {
                    Log.LogInfo("Resetting the moon mold data for " + planetName);
                    level.canSpawnMold = prevState;
                    level.moldSpreadIterations = prevIterations;
                }
            }
            Manager.originalMoonMoldData.Clear();
        }
    }
}
