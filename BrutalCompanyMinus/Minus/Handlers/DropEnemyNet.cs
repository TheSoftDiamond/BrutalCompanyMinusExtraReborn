using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using HarmonyLib;
using BepInEx.Configuration;
using BrutalCompanyMinus.Minus.Events;
using static BrutalCompanyMinus.Minus.MEvent;
using System.Collections;
using DunGen;
using UnityEngine.UIElements;
using UnityEngine.AI;
using Dawn;
using static UnityEngine.ParticleSystem.PlaybackState;

namespace BrutalCompanyMinus.Minus.MonoBehaviours
{
    [HarmonyPatch]
    internal class DropEnemyNet : NetworkBehaviour
    {
        public static DropEnemyNet instance;

        public void Awake()
        {
            if (instance != null) DestroyInstance();
            instance = this;
            Net.Instance.SetEventActiveServerRPC(nameof(DropEnemyNet), true);

            if (RoundManager.Instance.outsideAINodes == null)
            {
                RoundManager.Instance.outsideAINodes = GameObject.FindGameObjectsWithTag("OutsideAINode");
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ItemDropship), nameof(ItemDropship.OpenShipDoorsOnServer))]
        public static void OnShipDoorsOpen(ItemDropship __instance)
        {
            if (DropEnemy.Instance.Active)
            {
                float chance = UnityEngine.Random.Range(0f, 100f);

                if (chance <= MaskDrop.Instance.Getf(ScaleType.Percentage))
                {
                    int spawnPos = 0;

                    int amountToSpawn = UnityEngine.Random.Range(DropEnemy.Instance.Get(ScaleType.MinSpawned), DropEnemy.Instance.Get(ScaleType.MaxSpawned) + 1);

                    for (int i = 0; i < amountToSpawn; i++)
                    {
                        Vector3 exitPos = __instance.itemSpawnPositions[spawnPos].position;

                        bool spawnMask = UnityEngine.Random.value > 0.86f;

                        EnemyType? maskPrefab = spawnMask ? Assets.maskedPlayerPrefab : null;

                        RoundManager.Instance.SpawnEnemyGameObject(exitPos, 0, -3, maskPrefab);

                        spawnPos = (spawnPos + 1) % __instance.itemSpawnPositions.Length;
                    }
                }

            }
        }


        public static void DestroyInstance() // This handles the deletion of Time Chaosness
        {
            Events.DropEnemy.Instance.Active = false;
            GameObject netObject = GameObject.Find("DropEnemyObj");
            if (netObject != null)
            {
                GameObject.Destroy(netObject);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.ShipLeave))]
        public static void OnShipLeave()
        {
            DestroyInstance();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndGameServerRpc))]
        public static void DestroyForGodsSake()
        {
            DestroyInstance();
        }
    }
}
