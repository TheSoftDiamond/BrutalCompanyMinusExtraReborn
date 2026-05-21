using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using com.github.zehsteam.TakeyPlush;
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

                EventManager.FixHazardsOnLeave();

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

        public static bool DoesEventExist(string name)
        {
            try
            {
                bool exists = EventManager.events.Any(x => string.Equals(x.Name(), name, StringComparison.OrdinalIgnoreCase));
                if (!exists)
                {
                    Log.LogWarning($"The event with the name '{name}' does not exist.");
                }

                return exists;
            }
            catch (Exception e)
            {
                Log.LogError($"Error occurred while checking if event {name} exists: {e}");
                return false;
            }
        }

        #endregion

        #region Getters (Difficulty)

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
        /// This method returns the current heat difficulty. LevelID is optional.
        /// </summary>
        /// <returns></returns>
        public static float GetHeatDifficulty(int levelID = -1)
        {
            try
            {
                if (levelID == -1)
                {
                    levelID = RoundManager.Instance.currentLevel.levelID;
                }
                return Manager.heatDifficulty.TryGetValue(levelID, out float heat) ? heat : Configuration.startingHeat.Value;
            }
            catch (Exception e)
            {
                Log.LogError($"Error occurred while getting current heat difficulty: {e}");
                return 0f;
            }
        }

        #endregion

        #region Getters (Events)

        public static MEvent GetEventsByName(string names)
        {
            return EventManager.events.FirstOrDefault(x => string.Equals(x.Name(), names, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// This method returns the descriptions for a given event.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static List<string> GetEventDescriptions(MEvent e)
        {
            try
            {
                return e.Descriptions;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event descriptions for {e?.Name()}: {ex.Message}");
                return new List<string>();
            }
        }

        public static List<string> GetEventDescriptions(string thisEvent)
        {
            try
            {
                MEvent e = GetEventsByName(thisEvent);
                return e?.Descriptions ?? new List<string>();
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event descriptions for {thisEvent}: {ex.Message}");
                return new List<string>();
            }
        }

        /// <summary>
        /// This method returns the hex color of an event.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetEventColorHex(MEvent e)
        {
            try
            {
                return e.ColorHex;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event color hex for {e?.Name()}: {ex.Message}");
                return "";
            }
        }

        public static string GetEventColorHex(string thisEvent)
        {
            try
            {
                MEvent e = GetEventsByName(thisEvent);
                return e.ColorHex;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event color hex for {thisEvent}: {ex.Message}");
                return "";
            }
        }

        /// <summary>
        /// This method eturns the event's weight
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static int GetEventWeight(MEvent e)
        {
            try
            {
                return e.Weight;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event weight for {e?.Name()}: {ex.Message}");
                return 0;
            }
        }

        public static int GetEventWeight(string thisEvent)
        {
            try
            {
                MEvent e = GetEventsByName(thisEvent);
                return e.Weight;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event weight for {thisEvent}: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// This method returns the event's event type.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetEventType(MEvent e)
        {
            try
            {
                return e.Type.ToString();
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event type for {e?.Name()}: {ex.Message}");
                return "";
            }
        }

        public static string GetEventType(string thisEvent)
        {
            try
            {
                MEvent e = GetEventsByName(thisEvent);
                return e.Type.ToString();
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event type for {thisEvent}: {ex.Message}");
                return "";
            }
        }

        /// <summary>
        /// This method checks if an event is enabled or not.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool IsEventEnabled(MEvent e)
        {
            try
            {
                return e.Enabled;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event enabled status for {e?.Name()}: {ex.Message}");
                return false;
            }
        }

        public static bool IsEventEnabled(string thisEvent)
        {
            try
            {
                MEvent e = GetEventsByName(thisEvent);
                return e.Enabled;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event enabled status for {thisEvent}: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// This method checks if an event is a special event.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool IsEventSpecial(MEvent e)
        {
            try
            {
                return e.isSpecialEvent;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event special status for {e?.Name()}: {ex.Message}");
                return false;
            }
        }

        public static bool IsEventSpecial(string thisEvent)
        {
            try
            {
                MEvent e = GetEventsByName(thisEvent);
                return e.isSpecialEvent;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event special status for {thisEvent}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// This method checks if an event is a beta event.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool IsEventBeta(MEvent e)
        {
            try
            {
                return e.isBetaEvent;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event beta status for {e?.Name()}: {ex.Message}");
                return false;
            }
        }

        public static bool IsEventBeta(string thisEvent)
        {
            try
            {
                MEvent e = GetEventsByName(thisEvent);
                return e.isBetaEvent;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event beta status for {thisEvent}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// This method checks if an event is currently active. Used for monobehavior behaviors.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool IsEventActive(MEvent e)
        {
            try
            {
                return e.Active; //Where applicable
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event active status for {e?.Name()}: {ex.Message}");
                return false;
            }
        }

        public static bool IsEventActive(string thisEvent)
        {
            try
            {
                MEvent e = GetEventsByName(thisEvent);
                return e.Active; //Where applicable
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event active status for {thisEvent}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// This method checks if an event has been executed. 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool IsEventExecuted(MEvent e)
        {
            try
            {
                return e.Executed; //Where applicable
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event executed status for {e?.Name()}: {ex.Message}");
                return false;
            }
        }

        public static bool IsEventExecuted(string thisEvent)
        {
            try
            {
                MEvent e = GetEventsByName(thisEvent);
                return e.Executed; //Where applicable
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event executed status for {thisEvent}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// This method returns the list of events that will be removed when the event in question is spawned.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static List<string> GetEventsToRemove(MEvent e)
        {
            try
            {
                return e.EventsToRemove;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting events to remove for {e?.Name()}: {ex.Message}");
                return new List<string>();
            }
        }

        public static List<string> GetEventsToRemove(string thisEvent)
        {
            try
            {
                MEvent e = GetEventsByName(thisEvent);
                return e?.EventsToRemove ?? new List<string>();
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting events to remove for {thisEvent}: {ex.Message}");
                return new List<string>();
            }
        }

        /// <summary>
        /// This method returns the list of events that will be spawned alongside the event in question.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static List<string> GetEventsToAdd(MEvent e)
        {
            try
            {
                return e.EventsToSpawnWith;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting events to remove for {e?.Name()}: {ex.Message}");
                return new List<string>();
            }
        }

        public static List<string> GetEventsToAdd(string thisEvent)
        {
            try
            {
                MEvent e = GetEventsByName(thisEvent);
                return e?.EventsToSpawnWith ?? new List<string>();
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting events to remove for {thisEvent}: {ex.Message}");
                return new List<string>();
            }
        }

        /// <summary>
        /// This method checks if an event is safe for speedrunning.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool IsEventSpeedRunSafe(MEvent e)
        {
            try
            {
                return e.SpeedRunSafe;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event speedrun safe status for {e?.Name()}: {ex.Message}");
                return false;
            }
        }

        public static bool IsEventSpeedRunSafe(string thisEvent)
        {
            try
            {
                MEvent e = GetEventsByName(thisEvent);
                return e.SpeedRunSafe;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event speedrun safe status for {thisEvent}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Grab the aliases of an event.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static List<string> GetEventAliases(MEvent e)
        {
            try
            {
                return e.Aliases;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event aliases for {e?.Name()}: {ex.Message}");
                return new List<string>();
            }
        }

        public static List<string> GetEventAliases(string thisEvent)
        {
            try
            {
                MEvent e = GetEventsByName(thisEvent);
                return e?.Aliases ?? new List<string>();
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while getting event aliases for {thisEvent}: {ex.Message}");
                return new List<string>();
            }
        }

        public static bool isEventOnWhitelist(MEvent e)
        {
            try
            {
                return EventManager.IsEventOnMoonWhitelist(e);
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while checking if event is on whitelist for {e?.Name()}: {ex.Message}");
                return false;
            }
        }

        public static bool isEventOnWhitelist(string thisEvent)
        {
            try
            {
                MEvent e = GetEventsByName(thisEvent);
                return EventManager.IsEventOnMoonWhitelist(e);
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while checking if event is on whitelist for {thisEvent}: {ex.Message}");
                return false;
            }
        }

        public static bool isEventOnBlacklist(MEvent e)
        {
            try
            {
                return EventManager.IsIgnoredEventByMoonBlacklist(e);
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while checking if event is on blacklist for {e?.Name()}: {ex.Message}");
                return false;
            }
        }

        public static bool IsEventOnBlacklist(string thisEvent)
        {
            try
            {
                MEvent e = GetEventsByName(thisEvent);
                return EventManager.IsIgnoredEventByMoonBlacklist(e);
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while checking if event is on blacklist for {thisEvent}: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Setters

        //public static void SetHeatDifficulty(float heat, int levelID = -1)
        //{ 
        //
        //}

        #endregion
    }
}
