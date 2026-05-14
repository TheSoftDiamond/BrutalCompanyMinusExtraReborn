using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.Rendering.HighDefinition.ScalableSettingLevelParameter;

namespace BrutalCompanyMinus.Minus
{
    public class API
    {

        #region Functions

        /// <summary>
        /// This method can be used to regenerate events after landing.
        /// </summary>
        public static void RegenerateEvents()
        {
            try
            {
                SelectableLevel newLevel = Manager.currentLevel;
                EventManager.ModifyLevel(ref newLevel);
            }
            catch (Exception e)
            {
                Log.LogError($"Error occurred with GenerateBrutalEvents: {e}");
            }
        }

        /// <summary>
        /// This method can be used to force events by their names.
        /// </summary>
        /// <param name="eventNames"></param>
        public static void ForceEvents(string[] eventNames)
        {
            try
            {
                EventManager.forcedEvents.AddRange(EventManager.GetEventsByName(eventNames));
            }
            catch (Exception e)
            {
                Log.LogError($"Error occurred while forcing event(s) {eventNames}: {e}");
            }
        }

        //public static bool IsEventActive(string eventName)
        //{ }

        //public static bool DoesEventExist()
        //{ }

        #endregion

        #region Getters

        /// <summary>
        /// This method returns the current difficulty of the game.
        /// </summary>
        /// <returns></returns>
        public static float GetDifficulty()
        {
            try
            {
                return Manager.difficulty;
            }
            catch (Exception e)
            {
                Log.LogError($"Error occurred while getting current difficulty: {e}");
                return 0f;
            }
        }

        /// <summary>
        /// This method returns the current days difficulty.
        /// </summary>
        /// <returns></returns>
        public static float GetDaysDifficulty()
        {
            try
            {
                return Mathf.Clamp(Manager.daysPassed * Configuration.daysPassedDifficultyMultiplier.Value, 0.0f, Configuration.daysPassedDifficultyCap.Value);
            }
            catch (Exception e)
            {
                Log.LogError($"Error occurred while getting current days difficulty: {e}");
                return 0f;
            }
        }

        /// <summary>
        /// This method returns the current scrap difficulty.
        /// </summary>
        /// <returns></returns>
        public static float GetScrapDifficulty()
        {
            try
            {
                return Mathf.Clamp(Manager.GetScrapInShip() * Configuration.scrapInShipDifficultyMultiplier.Value, 0.0f, Configuration.scrapInShipDifficultyCap.Value);
            }
            catch (Exception e)
            {
                Log.LogError($"Error occurred while getting current scrap difficulty: {e}");
                return 0f;
            }
        }

        /// <summary>
        /// This method returns the current moon difficulty.
        /// </summary>
        /// <returns></returns>
        public static float GetMoonDifficulty()
        {
            try
            {
                if (Configuration.gradeAdditives.TryGetValue(StartOfRound.Instance.currentLevel.riskLevel, out float value))
                {
                    return value;
                }
                else
                {
                    return 0f;
                }
            }
            catch (Exception e)
            {
                Log.LogError($"Error occurred while getting current moon difficulty: {e}");
                return 0f;
            }
        }

        /// <summary>
        /// This method returns the current weather difficulty.
        /// </summary>
        /// <returns></returns>
        public static float GetWeatherDifficulty()
        {
            try
            {
                return Configuration.weatherAdditives.GetValueOrDefault(StartOfRound.Instance.currentLevel.currentWeather, 0.0f);
            }
            catch (Exception e)
            {
                Log.LogError($"Error occurred while getting current weather difficulty: {e}");
                return 0f;
            }
        }

        /// <summary>
        /// This method returns the current quota difficulty.
        /// </summary>
        /// <returns></returns>
        public static float GetQuotaDifficulty()
        {
            try
            {
                return Mathf.Clamp(TimeOfDay.Instance.profitQuota * Configuration.quotaDifficultyMultiplier.Value, 0.0f, Configuration.quotaDifficultyCap.Value);
            }
            catch (Exception e)
            {
                Log.LogError($"Error occurred while getting current quota difficulty: {e}");
                return 0f;
            }
        }

        /// <summary>
        /// This method returns the current heat difficulty.
        /// </summary>
        /// <returns></returns>
        public static float GetHeatDifficulty()
        {
            try
            {
                int levelID = RoundManager.Instance.currentLevel.levelID;
                return Manager.heatDifficulty.TryGetValue(levelID, out float heat) ? heat : Configuration.startingHeat.Value;
            }
            catch (Exception e)
            {
                Log.LogError($"Error occurred while getting current heat difficulty: {e}");
                return 0f;
            }
        }

        /// <summary>
        /// This method returns the heat difficulty of a level by its ID.
        /// </summary>
        /// <param name="levelID"></param>
        /// <returns></returns>
        public static float GetHeatDifficultyByID(int levelID)
        {
            try
            {
                return Manager.heatDifficulty.TryGetValue(levelID, out float heat) ? heat : Configuration.startingHeat.Value;
            }
            catch (Exception e)
            {
                Log.LogError($"Error occurred while getting current heat difficulty: {e}");
                return 0f;
            }
        }




        #endregion
    }
}
