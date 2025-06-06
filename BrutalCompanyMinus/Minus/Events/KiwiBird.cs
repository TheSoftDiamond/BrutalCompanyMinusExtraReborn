using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.Events
{
    internal class KiwiBird : MEvent
    {
        public override string Name() => nameof(KiwiBird);

        public static KiwiBird Instance;

        public override void Initalize()
        {
            Instance = this;

            Weight = 1;
            Descriptions = new List<string>() { "Eggs for breakfast!", "Egging you on", "Is it worth it?", "Beware of the Giant Kiwi", "Whats that pecking noise?", "Its like a woodpecker but..." };
            ColorHex = "#800000";
            Type = EventType.VeryBad;
        }

        public override void Execute()
        {
            // I hate this method used to spawn the Giant Kiwi Bird but it somehow works.
            GameObject hangarShip = Assets.hangarShip;
            if (hangarShip == null)
            {
                return;
            }

            try
            {
                Vector3 shipPosition = hangarShip.transform.position;
                Vector3 spawnSpot = RoundManager.Instance.GetRandomNavMeshPositionInRadiusSpherical(shipPosition, 170.0f); // Hope the position you receive is a good one. This position has a chance to be "undesirable" for players depending on the moon.
                EnemyType giantKiwiType = Assets.GetEnemy(Assets.EnemyName.GiantKiwi);
                RoundManager.Instance.SpawnEnemyGameObject(spawnSpot, 0, 1, giantKiwiType); // Spawn the Giant Kiwi
                if (GameObject.FindObjectOfType<EnemyAINestSpawnObject>() == null)
                {
                    Log.LogWarning("A nest was not found. Spawning Next");
                    GiantKiwiAI giantKiwiAI = GameObject.FindObjectOfType<GiantKiwiAI>();
                    try
                    {
                        giantKiwiAI.SpawnBirdNest(); // Should things go wrong? Who knows if this is needed.
                    }
                    catch (Exception ex)
                    {
                        Log.LogError($"Error while spawning GiantKiwiAI nest: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while spawning GiantKiwiAI: {ex.Message}");
            }
        }

        public override bool AddEventIfOnly() => Assets.ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Enable Special Events?");
    }
}

