using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using HarmonyLib;
using Unity.Netcode;
using WeatherRegistry.Enums;
using WeatherRegistry.Managers;

namespace BrutalCompanyMinus.Minus.Handlers.Modded
{
    internal static class CustomWeatherConfigCompat
    {
        public static Dictionary<string, Weather> weatherAdditivesModded = new Dictionary<string, Weather>();

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void PatchAll(Harmony harmony)
        {
            AplyWeatherInitPatch(harmony);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void AplyWeatherInitPatch(Harmony harmony)
        {
            var originalMethod = AccessTools.Method(typeof(WeatherRegistry.Managers.StartupManager), nameof(WeatherRegistry.Managers.StartupManager.Init));
            var postfixMethod = AccessTools.Method(typeof(CustomWeatherConfigCompat), nameof(StartupManagerInit_Postfix));

            if (originalMethod != null && postfixMethod != null)
            {
                harmony.Patch(originalMethod, postfix: new HarmonyMethod(postfixMethod));
                Log.LogInfo("Successfully patched StartupManager.Init");
            }
            else
            {
                Log.LogError("Failed to locate methods for conditional patching.");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCustomWeather()
        {
            if (!Compatibility.WeatherRegistryPresent) return "";

            try
            {
                var current = WeatherRegistry.WeatherManager.GetCurrentWeather(StartOfRound.Instance.currentLevel);
                return current != null ? current.ToString() : "";
            }
            catch
            {
                return "";
            }
        }

        internal static void SetupCustomWeatherConfigs()
        {
            Weather createWeatherSettings(WeatherRegistry.Weather weather, float scrapValueMultiplier, float scrapAmountMultiplier, float weatherAdditive)
            {
                string configHeader = $"({weather.ToString()}) Modded Weather multipliers";

                float valueMultiplierSetting = Configuration.weatherConfig.Bind(configHeader, "Scrap Value Multiplier", scrapValueMultiplier, "Multiply Scrap value for " + weather.ToString()).Value;
                float amountMultiplierSetting = Configuration.weatherConfig.Bind(configHeader, "Scrap Amount Multiplier", scrapAmountMultiplier, "Multiply Scrap amount for " + weather.ToString()).Value;
                float weatherAdditives = Configuration.weatherConfig.Bind(configHeader, "Difficulty Additive", weatherAdditive, "Difficulty additive for " + weather.ToString()).Value;

                return new Weather(weather.VanillaWeatherType, valueMultiplierSetting, amountMultiplierSetting, weatherAdditives);
            }

            List<WeatherRegistry.Weather> weatherObjects = WeatherRegistry.WeatherManager.Weathers;
            for (int i = 0; i < weatherObjects.Count; i++)
            {
                var weather = weatherObjects[i];
                Log.LogInfo($"Found Weather: {weather?.Name} with Type: {weather?.Type}");
                if (weather != null 
                && !weather.Name.Equals("None", StringComparison.InvariantCultureIgnoreCase)
                && weather.Type != WeatherType.Vanilla)
                {
                    Log.LogInfo($"{weather.Name} passed checks!");
                    weatherAdditivesModded[weather.Name] = createWeatherSettings(weather, 1.0f, 1.0f, 0.0f);
                }
            }

            // Save the new config options added
            Configuration.weatherConfig.Save();
        }

        public static void InitalizeModdedWeatherMultipliers(ref NetworkList<Weather> currentWeatherMultipliers)
        {
            foreach (var weatherKVPair in weatherAdditivesModded)
            {
                var weather = weatherKVPair.Value;
                string weatherName = weatherKVPair.Key;
                Log.LogInfo($"Added {weatherName} Multiplier: {weather}");
                currentWeatherMultipliers.Add(weather);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static float? GetCustomWeatherAdditive()
        {
            var moddedWeather = WeatherRegistry.WeatherManager.GetCurrentLevelWeather();
            if (weatherAdditivesModded.TryGetValue(moddedWeather.Name, out var weather))
            {
                return weather.weatherAdditive;
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static float? GetCustomWeatherScrapValueMultiplier()
        {
            var moddedWeather = WeatherRegistry.WeatherManager.GetCurrentLevelWeather();
            if (weatherAdditivesModded.TryGetValue(moddedWeather.Name, out var weather))
            {
                return weather.scrapValueMultiplier;
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static float? GetCustomWeatherScrapAmountMultiplier()
        {
            var moddedWeather = WeatherRegistry.WeatherManager.GetCurrentLevelWeather();
            if (weatherAdditivesModded.TryGetValue(moddedWeather.Name, out var weather))
            {
                return weather.scrapAmountMultiplier;
            }
            return null;
        }

        // Postfix so custom weather is correctly registered
        //[HarmonyPatch(typeof(WeatherRegistry.Managers.StartupManager), "Init")]
        //[HarmonyPostfix]
        // I actually hate this - Soft & T-Rizzle12
        internal static void StartupManagerInit_Postfix()
        {
            if (Compatibility.WeatherRegistryPresent)
            {
                // Setup Modded Weather configs
                SetupCustomWeatherConfigs();

                // Register between clients
                Log.LogInfo("Adding Mod Added Weathers!");
                CustomWeatherConfigCompat.InitalizeModdedWeatherMultipliers(ref Net.Instance.currentWeatherMultipliers);
                if (NetworkManager.Singleton.IsServer)
                {
                    Net.Instance.UpdateCurrentWeatherMultipliersServerRpc();
                }
            }
        }
    }
}
