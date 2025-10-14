using System;
using System.Collections.Generic;
using System.Text;
using WeatherRegistry;
using System.Linq;
using static UnityEngine.Rendering.HighDefinition.ScalableSettingLevelParameter;

namespace BrutalCompanyMinus.Minus.Handlers.Modded
{
    public class CustomWeather
    {
        public static void SetCustomWeather(string WeatherName)
        {
            if (!Compatibility.IsModPresent("mrov.WeatherRegistry"))
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

        public static void AddCustomWeather(string WeatherName)
        {
            if (!Compatibility.IsModPresent("mrov.WeatherRegistry"))
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
    }
}
