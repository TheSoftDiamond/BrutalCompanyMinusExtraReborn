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

namespace BrutalCompanyMinus.Minus.MonoBehaviours
{
    [HarmonyPatch]
    internal class MaskDropNet : NetworkBehaviour
    {
        public static MaskDropNet instance;

        public float nextDropTime;

        public float currentTime;

        public void Awake()
        {
            if (instance != null) DestroyInstance();
            instance = this;
            Net.Instance.SetEventActiveServerRPC(nameof(MaskDrop), true);

            if (RoundManager.Instance.IsServer)
            {
                currentTime = 0f;

                nextDropTime = UnityEngine.Random.Range(MaskDrop.Instance.Getf(ScaleType.MinIntervalTime), MaskDrop.Instance.Getf(ScaleType.MaxIntervalTime));
                //Log.LogInfo($"[MaskDrop] Next drop time: {nextDropTime}");
            }

            if (RoundManager.Instance.outsideAINodes == null)
            {
                RoundManager.Instance.outsideAINodes = GameObject.FindGameObjectsWithTag("OutsideAINode");
            }

            //shipPrefab = new GameObject("ship");

          

            //Material blueMat = new Material(Shader.Find("HDRP/Lit"));
            //blueMat.SetColor("_BaseColor", Color.blue);

            //GameObject startSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //startSphere.name = "StartSphere";
            //startSphere.transform.SetParent(shipPrefab.transform, false);
            //startSphere.GetComponent<MeshRenderer>().material = blueMat;

            //shipPrefab.SetActive(false);
        }

        public void Update()
        {
            if (RoundManager.Instance.IsServer)
            {
                if (!StartOfRound.Instance.shipHasLanded) return;

                float theTimeOfDay = TimeOfDay.Instance.normalizedTimeOfDay;

                if (theTimeOfDay <= MaskDrop.Instance.Getf(ScaleType.timeStart) || theTimeOfDay >= MaskDrop.Instance.Getf(ScaleType.timeEnd)) return;

                currentTime += Time.deltaTime;
                //Log.LogInfo($"[MaskDrop] Current time: {currentTime}, Next drop time: {nextDropTime}");
                if (currentTime > nextDropTime)
                {
                    nextDropTime = UnityEngine.Random.Range(MaskDrop.Instance.Getf(ScaleType.MinIntervalTime), MaskDrop.Instance.Getf(ScaleType.MaxIntervalTime));
                    currentTime = 0f;

                    float chance = UnityEngine.Random.Range(0f, 100f);
                    //Log.LogInfo($"[MaskDrop] Chance: {chance}, Percentage: {MaskDrop.Instance.Getf(ScaleType.Percentage)}");

                    if (chance <= MaskDrop.Instance.Getf(ScaleType.Percentage))
                    {
                        int seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);

                        int height = UnityEngine.Random.Range(170, 220);

                        float speed = UnityEngine.Random.Range(MaskDrop.Instance.Getf(ScaleType.SpeedMin), MaskDrop.Instance.Getf(ScaleType.SpeedMax));
                        //Log.LogInfo($"[MaskDrop] Spawning ship with seed: {seed}, height: {height}, speed: {speed}");

                        float x = UnityEngine.Random.Range(0, 360);
                        float y = UnityEngine.Random.Range(0, 360);
                        float z = UnityEngine.Random.Range(0, 360);
                        float timeToDespawn = UnityEngine.Random.Range(MaskDrop.Instance.Getf(ScaleType.TimeWaitMinDespawn), MaskDrop.Instance.Getf(ScaleType.TimeWaitMaxDespawn));

                        Net.Instance.SpawnMaskShipServerRpc(seed, height, speed, x, y, z, timeToDespawn);
                    }
                }
            }
        }

        public static void DestroyInstance() // This handles the deletion of Time Chaosness
        {
            Events.MaskDrop.Instance.Active = false;
            GameObject netObject = GameObject.Find("MaskDropObj");
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
