using System;
using System.Collections.Generic;
using System.Text;
using WeatherRegistry;
using System.Linq;

namespace BrutalCompanyMinus.Minus.Handlers.Modded
{
    public class CustomWeather
    {
        public static void SetCustomWeather(string WeatherName)
        {
            if (!Compatibility.WeatherRegistryPresent)
            {
                return;
            }

            // Find the weather by name
            try
            {
                WeatherRegistry.Weather weather = WeatherManager.Weathers.First(w => w.Name.Equals(WeatherName, StringComparison.OrdinalIgnoreCase));
                if (weather == null)
                {
                    Log.LogError($"Weather '{WeatherName}' not found.");
                    return;
                }
                //WeatherController.ChangeCurrentWeather(weather);


                WeatherManager.CurrentWeathers.SetWeather(StartOfRound.Instance.currentLevel, weather);
            }
            catch (Exception ex)
            {
                Log.LogError($"Error setting '{WeatherName}' weather: {ex.Message}");
            }
        }

        public static bool isWeatherPresent(string WeatherName)
        {
            if (!Compatibility.WeatherRegistryPresent)
            {
                return false;
            }

            try
            {
                WeatherRegistry.Weather weather = WeatherManager.Weathers.First(w => w.Name.Equals(WeatherName, StringComparison.OrdinalIgnoreCase));
                if (weather == null)
                {
                    Log.LogError($"Weather '{WeatherName}' not found.");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error adding '{WeatherName}' weather: {ex.Message}");
                return false;
            }
        }

        public static void AddCustomWeather(string WeatherName)
        {
            if (!Compatibility.WeatherRegistryPresent)
            {
                return;
            }

            try
            {
                WeatherRegistry.Weather weather = WeatherManager.Weathers.First(w => w.Name.Equals(WeatherName, StringComparison.OrdinalIgnoreCase));
                if (weather == null)
                {
                    Log.LogError($"Weather '{WeatherName}' not found.");
                    return;
                }
                WeatherController.AddWeatherEffect(weather);
            }
            catch (Exception ex)
            {
                Log.LogError($"Error adding '{WeatherName}' weather: {ex.Message}");
            }
        }

        public static void RegisterWeathers()
        {
            if (!Compatibility.CombinedWeatherToolKitPresent)
            {
                return;
            }
        }

        public static string GetCustomWeather()
        {
            if (!Compatibility.WeatherRegistryPresent)
            {
                return "";
            }

            return WeatherManager.GetCurrentWeather(StartOfRound.Instance.currentLevel).ToString();
        }
    }
}
