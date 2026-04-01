using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BrutalCompanyMinus.Minus.Handlers.Modded
{
    public class CustomWeather
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SetCustomWeather(string WeatherName)
        {
            if (!Compatibility.WeatherRegistryPresent) return;

            try
            {
                WeatherRegistry.Weather weather = null;
                foreach (var w in WeatherRegistry.WeatherManager.Weathers)
                {
                    if (string.Equals(w.Name, WeatherName, StringComparison.OrdinalIgnoreCase))
                    {
                        weather = w;
                        break;
                    }
                }

                if (weather == null)
                {
                    Log.LogError($"Weather '{WeatherName}' not found.");
                    return;
                }

                WeatherRegistry.WeatherManager.CurrentWeathers.SetWeather(StartOfRound.Instance.currentLevel, weather);
            }
            catch (Exception ex)
            {
                Log.LogError($"Error setting '{WeatherName}' weather: {ex.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool isWeatherPresent(string WeatherName)
        {
            if (!Compatibility.WeatherRegistryPresent) return false;

            try
            {
                foreach (var w in WeatherRegistry.WeatherManager.Weathers)
                {
                    if (string.Equals(w.Name, WeatherName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error checking for '{WeatherName}' weather: {ex.Message}");
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AddCustomWeather(string WeatherName)
        {
            if (!Compatibility.WeatherRegistryPresent) return;

            try
            {
                WeatherRegistry.Weather weather = null;
                foreach (var w in WeatherRegistry.WeatherManager.Weathers)
                {
                    if (string.Equals(w.Name, WeatherName, StringComparison.OrdinalIgnoreCase))
                    {
                        weather = w;
                        break;
                    }
                }

                if (weather == null)
                {
                    Log.LogError($"Weather '{WeatherName}' not found.");
                    return;
                }

                WeatherRegistry.WeatherController.AddWeatherEffect(weather);
            }
            catch (Exception ex)
            {
                Log.LogError($"Error adding '{WeatherName}' weather: {ex.Message}");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void RegisterWeathers()
        {
            if (!Compatibility.CombinedWeatherToolKitPresent) return;
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
    }
}