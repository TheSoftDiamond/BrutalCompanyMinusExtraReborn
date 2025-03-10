﻿//using FacilityMeltdown.API;
//using FacilityMeltdown.Behaviours;
//using FacilityMeltdown.MeltdownSequence.Behaviours;
//using FacilityMeltdown.Util;
using LethalLib.Modules;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static BrutalCompanyMinus.Modules.MeltdownInteriorMapper;
using static BrutalCompanyMinus.Modules.MeltdownMoonMapper;
using static BrutalCompanyMinus.Minus.Events.TerminalFailure;


namespace BrutalCompanyMinus.Modules
{
    public static class MeltdownEffects
    {
       /* public static List<(Light light, Color originalColour)> SetupEmergencyLights()
        {
            for (int i = 0; i < RoundManager.Instance.allPoweredLights.Count; i++)
            {
                RoundManager.Instance.allPoweredLights[i].color = MeltdownInteriorMapper.Instance.outsideEmergencyLightColour;
            }
            List<(Light light, Color originalColour)> outsideLights;
            for (int i = 0; i < MeltdownMoonMapper.Instance.outsideEmergencyLights.Count; i++)
            {
                outsideLights.Add((MeltdownMoonMapper.Instance.outsideEmergencyLights[i], MeltdownMoonMapper.Instance.outsideEmergencyLights[i].color));
            }
            return outsideLights;
        }*/

        public static IEnumerator RepeatUntilEndOfMeltdown(Func<IEnumerator> enumerator)
        {
            while (true)
            { // we be a bit silly :3
                BrutalCompanyMinus.Log.LogDebug("looping effect");
                yield return enumerator();
            }
        }
        public static IEnumerator WithDelay(IEnumerator enumerator, float delay)
        {
            yield return new WaitForSeconds(delay);
            yield return enumerator;
        }

        public static IEnumerator WithDelay(Action callback, float delay)
        {
            yield return new WaitForSeconds(delay);
            callback();
        }

        public static IEnumerator WithRandomDelay(IEnumerator enumerator, float min, float max)
        {
            yield return WithDelay(enumerator, UnityEngine.Random.Range(min, max));
        }
        public static IEnumerator WithRandomDelay(Action callback, float min, float max)
        {
            yield return WithDelay(callback, UnityEngine.Random.Range(min, max));
        }

      /*  public static IEnumerator AtProgress(IEnumerator enumerator, float progress)
        {
            while (MeltdownAPI.MeltdownStarted && MeltdownHandler.Instance.Progress > progress)
            {
                yield return null;
            }
            yield return enumerator;
        }*/
       /* public static IEnumerator AtProgress(Action callback, float progress)
        {
            while (MeltdownAPI.MeltdownStarted && MeltdownHandler.Instance.Progress > progress)
            {
                yield return null;
            }
            callback();
        }*/

       /* public static IEnumerator WithDynamicRandomDelay(IEnumerator enumerator)
        {
            yield return enumerator;
            switch (MeltdownHandler.Instance.Progress)
            {
                case > .75f:
                    yield return new WaitForSeconds(UnityEngine.Random.Range(10f, 14f));
                    break;
                case > .5f:
                    yield return new WaitForSeconds(UnityEngine.Random.Range(6f, 10f));
                    break;
                case > .25f:
                    yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 6.5f));
                    break;
                default:
                    yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 4f));
                    break;
            }
        }*/
      /*  public static IEnumerator WithDynamicRandomDelay(Action callback)
        {
            callback();
            switch (MeltdownHandler.Instance.Progress)
            {
                case > .75f:
                    yield return new WaitForSeconds(UnityEngine.Random.Range(10f, 14f));
                    break;
                case > .5f:
                    yield return new WaitForSeconds(UnityEngine.Random.Range(6f, 10f));
                    break;
                case > .25f:
                    yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 6.5f));
                    break;
                default:
                    yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 4f));
                    break;
            }
        }*/

        public static IEnumerator EmergencyLights(float onTime, float offTime, List<(Light light, Color originalColour)> originalLightColours)
        { if(TerminalNet.Value == 0)
            BrutalCompanyMinus.Log.LogDebug("Switching lights ON");
            for (int i = 0; i < RoundManager.Instance.allPoweredLightsAnimators.Count; i++)
            {
                RoundManager.Instance.allPoweredLightsAnimators[i].SetBool("on", true);
            }
            for (int i = 0; i < MeltdownMoonMapper.Instance.outsideEmergencyLights.Count; i++)
            {
                MeltdownMoonMapper.Instance.outsideEmergencyLights[i].color = MeltdownMoonMapper.Instance.outsideEmergencyLightColour;
            }

            yield return new WaitForSeconds(onTime);
            Log.LogDebug("Switching lights OFF");

            for (int i = 0; i < RoundManager.Instance.allPoweredLightsAnimators.Count; i++)
            {
                RoundManager.Instance.allPoweredLightsAnimators[i].SetBool("on", false);
            }

            foreach ((Light light, Color originalColour) in originalLightColours)
            {
                light.color = originalColour;
            }

            yield return new WaitForSeconds(offTime);
        }

        public static void InsideParticleEffects()
        {
            /*if (MeltdownPlugin.clientConfig.ScreenShake)
            {
                for (int i = 0; i < UnityEngine.Random.Range(5, 15); i++)
                {
                    Vector3 position = GetRandomPositionInsideFacility() + Vector3.up;
                    RaycastHit hit;
                    if (Physics.Raycast(new Ray(position, Vector3.up), out hit, 20f, 256))
                    {
                        GameObject prefab = MeltdownPlugin.assets.facilityEffects[UnityEngine.Random.Range(0, MeltdownPlugin.assets.facilityEffects.Length)];
                        GameObject created = GameObject.Instantiate(prefab);
                        prefab.transform.position = hit.point;
                        created.transform.parent = MeltdownHandler.Instance.transform;
                    }

                }
            }*/

            if (GameNetworkManager.Instance.localPlayerController.isInsideFactory)
                GameObject.Instantiate(StartOfRound.Instance.explosionPrefab, GetRandomPositionNearPlayer(), Quaternion.Euler(-90f, 0f, 0f), RoundManager.Instance.mapPropsContainer.transform);
            /*if (MeltdownPlugin.clientConfig.ScreenShake)
            {
                if (MeltdownHandler.Instance.Progress > .5f)
                {
                    HUDManager.Instance.ShakeCamera(ScreenShakeType.Big);
                }
                else
                {
                    HUDManager.Instance.ShakeCamera(ScreenShakeType.VeryStrong);
                }

            }*/
        }

       /* public static IEnumerator WarningAnnouncer(AudioSource source)
        {
            source.volume = MeltdownPlugin.clientConfig.MusicVolume;

            AudioClip sound = MeltdownPlugin.assets.warnings[UnityEngine.Random.Range(0, MeltdownPlugin.assets.warnings.Length)];
            source.clip = sound;
            source.Play();

            yield return new WaitForSeconds(source.clip.length);
        }*/

        #region HELPER FUNCTIONS
        private static Vector3 PlacePositionInsideFacility(Vector3 position, float radius = 10f)
        {
            return RoundManager.Instance.GetRandomNavMeshPositionInBoxPredictable(position, 10f, layerMask: -1, randomSeed: new System.Random());
        }

        private static Vector3 GetRandomPositionNearPlayer(float radius = 15f)
        {
            return PlacePositionInsideFacility(GameNetworkManager.Instance.localPlayerController.transform.position + (UnityEngine.Random.insideUnitSphere * radius));
        }

        private static Vector3 GetRandomPositionInsideFacility()
        {
            return PlacePositionInsideFacility(RoundManager.Instance.insideAINodes[UnityEngine.Random.Range(0, RoundManager.Instance.insideAINodes.Length)].transform.position);
        }

        #endregion
    }
}