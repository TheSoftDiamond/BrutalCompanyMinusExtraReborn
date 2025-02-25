﻿using BepInEx;
using BepInEx.Configuration;
using BrutalCompanyMinus.Minus;
using System.Collections.Generic;
using static BrutalCompanyMinus.Minus.MEvent;
using HarmonyLib;
using UnityEngine;
using System.Globalization;
using BrutalCompanyMinus.Minus.CustomEvents;
using BrutalCompanyMinus.Minus.Events;
using static BrutalCompanyMinus.Minus.MonoBehaviours.EnemySpawnCycle;
using static BrutalCompanyMinus.Assets;
using static BrutalCompanyMinus.Helper;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using System;
using EventType = BrutalCompanyMinus.Minus.MEvent.EventType;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace BrutalCompanyMinus
{
    [HarmonyPatch]
    public class Configuration
    {
        // Config files
        public static ConfigFile uiConfig, eventConfig, weatherConfig, customAssetsConfig, difficultyConfig, moddedEventConfig, customEventConfig, allEnemiesConfig, levelPropertiesConfig, CorePropertiesConfig /*, extensiveSettingsConfig*/;

        // Event settings
        public static List<ConfigEntry<int>> eventWeights = new List<ConfigEntry<int>>();
        public static List<List<string>> eventDescriptions = new List<List<string>>();
        public static List<ConfigEntry<string>> eventColorHexes = new List<ConfigEntry<string>>();
        public static List<ConfigEntry<MEvent.EventType>> eventTypes = new List<ConfigEntry<MEvent.EventType>>();
        public static List<Dictionary<ScaleType, Scale>> eventScales = new List<Dictionary<ScaleType, Scale>>();
        public static List<ConfigEntry<bool>> eventEnables = new List<ConfigEntry<bool>>();
        public static List<List<string>> eventsToRemove = new List<List<string>>(), eventsToSpawnWith = new List<List<string>>();
        public static List<List<MonsterEvent>> monsterEvents = new List<List<MonsterEvent>>();
        public static List<ScrapTransmutationEvent> transmutationEvents = new List<ScrapTransmutationEvent>();

        // Difficulty Settings
        public static ConfigEntry<bool> useCustomWeights, showEventsInChat;
        public static Scale eventsToSpawn;
        public static ConfigEntry<float> goodEventIncrementMultiplier, badEventIncrementMultiplier;
        public static float[] weightsForExtraEvents;
        public static Scale[] eventTypeScales = new Scale[6];
        public static Dictionary<EventType, Scale> scrapValueByEventTypeScale = new Dictionary<EventType, Scale>();
        public static Dictionary<EventType, Scale> scrapAmountByEventTypeScale = new Dictionary<EventType, Scale>();
        public static ConfigEntry<string> MoonsToIgnore;
        public static Scale FactorySize = new Scale();

        public static EventManager.DifficultyTransition[] difficultyTransitions;
        public static ConfigEntry<bool> enableQuotaChanges;
        public static ConfigEntry<int> deadLineDaysAmount, startingCredits, startingQuota, baseIncrease, increaseSteepness;
        public static Scale
            spawnChanceMultiplierScaling = new Scale(), insideEnemyMaxPowerCountScaling = new Scale(), outsideEnemyPowerCountScaling = new Scale(), enemyBonusHpScaling = new Scale(), spawnCapMultiplier = new Scale(),
            scrapAmountMultiplier = new Scale(), scrapValueMultiplier = new Scale(), insideSpawnChanceAdditive = new Scale(), outsideSpawnChanceAdditive = new Scale();
        public static ConfigEntry<bool> ignoreMaxCap;
        public static ConfigEntry<float> difficultyMaxCap;
        public static ConfigEntry<bool> scaleByDaysPassed, scaleByScrapInShip, scaleByMoonGrade, scaleByWeather, scaleByQuota;
        public static ConfigEntry<float> daysPassedDifficultyMultiplier, daysPassedDifficultyCap, scrapInShipDifficultyMultiplier, scrapInShipDifficultyCap, quotaDifficultyMultiplier, quotaDifficultyCap;
        public static Dictionary<string, float> gradeAdditives = new Dictionary<string, float>();
        public static Dictionary<LevelWeatherType, float> weatherAdditives = new Dictionary<LevelWeatherType, float>();
        // Player Scaling Settings
        public static ConfigEntry<bool> enablePlayerScaling;
        public static ConfigEntry<string> playerScalingType;
        public static ConfigEntry<float> playerScalingMultiplier;
        public static ConfigEntry<int> basePlayerAmount;

        // Weather settings
        public static ConfigEntry<bool> useWeatherMultipliers, randomizeWeatherMultipliers, enableTerminalText;
        public static ConfigEntry<float> weatherRandomRandomMinInclusive, weatherRandomRandomMaxInclusive;
        public static Weather noneMultiplier, dustCloudMultiplier, rainyMultiplier, stormyMultiplier, foggyMultiplier, floodedMultiplier, eclipsedMultiplier;

        // UI settings
        public static ConfigEntry<string> UIKey;
        public static ConfigEntry<bool> NormaliseScrapValueDisplay, EnableUI, ShowUILetterBox, ShowExtraProperties, PopUpUI, DisplayUIAfterShipLeaves, DisplayExtraPropertiesAfterShipLeaves, displayEvents;
        public static ConfigEntry<float> UITime, scrollSpeed;

        // Custom assets settings
        public static ConfigEntry<int> nutSlayerLives, nutSlayerHp;
        public static ConfigEntry<float> nutSlayerMovementSpeed;
        public static ConfigEntry<bool> nutSlayerImmortal;
        public static ConfigEntry<int> slayerShotgunMinValue, slayerShotgunMaxValue;

        // All enemies settings
        public static ConfigEntry<bool> enableAllEnemies, enableAllAllEnemies;

        // Level properties settings
        public static Dictionary<int, LevelProperties> levelProperties = new Dictionary<int, LevelProperties>();

        // Other
        public static CultureInfo en = new CultureInfo("en-US"); // This is important, no touchy
        public static string scaleDescription = "Format: BaseScale, IncrementScale, MinCap, MaxCap,   Forumla: BaseScale + (IncrementScale * Difficulty),   By default difficulty goes between 0 to 100 depending on certain factors";

        // Custom Events and Settings
        public static string customEventsFolder = Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CustomEvents";
        
        // Core Properties
        public static ConfigEntry<bool> enableCustomEvents;
        public static ConfigEntry<bool> ExtraLogging;
        public static ConfigEntry<bool> HellTimeAdjustment;
        public static ConfigEntry<bool> VeryLateShipAdjustment;

        /*   public static ConfigEntry<bool> EnableStreamerEvents;*/

        private static void Initalize()
        {
            // Difficulty Settings
            useCustomWeights = difficultyConfig.Bind("_Event Settings", "Use custom weights?", false, "'false'= Use eventType weights to set all the weights     'true'= Use custom set weights");
            eventsToSpawn = getScale(difficultyConfig.Bind("_Event Settings", "Event scale amount", "2, 0.03, 2.0, 5.0", "The base amount of events   Format: BaseScale, IncrementScale, MinCap, MaxCap,   " + scaleDescription).Value);
            weightsForExtraEvents = ParseValuesFromString(difficultyConfig.Bind("_Event Settings", "Weights for bonus events", "40, 39, 15, 5, 1", "Weights for bonus events, can be expanded. (40, 39, 15, 5, 1) is equivalent to (+0, +1, +2, +3, +4) events").Value);
            showEventsInChat = difficultyConfig.Bind("_Event Settings", "Will Minus display events in chat?", false);

            eventTypeScales = new Scale[6]
            {
                getScale(difficultyConfig.Bind("_EventType Weights", "VeryBad event scale", "5, 0.25, 5, 30", scaleDescription).Value),
                getScale(difficultyConfig.Bind("_EventType Weights", "Bad event scale", "40, -0.15, 25, 40", scaleDescription).Value),
                getScale(difficultyConfig.Bind("_EventType Weights", "Neutral event scale", "10, -0.05, 5, 10", scaleDescription).Value),
                getScale(difficultyConfig.Bind("_EventType Weights", "Good event scale", "23, -0.1, 13, 23", scaleDescription).Value),
                getScale(difficultyConfig.Bind("_EventType Weights", "VeryGood event scale", "3, 0.14, 3, 17", scaleDescription).Value),
                getScale(difficultyConfig.Bind("_EventType Weights", "Remove event scale", "15, -0.05, 10, 15", "These events remove something   " + scaleDescription).Value)
            };

            difficultyTransitions = GetDifficultyTransitionsFromString(difficultyConfig.Bind("Difficulty Scaling", "Difficulty Transitions", "Easy,00FF00,0|Medium,008000,15|Hard,FF0000,30|Very Hard,800000,50|Insane,140000,75", "Format: NAME,HEX,ABOVE, above is the value the name will be shown at.").Value);
            ignoreMaxCap = difficultyConfig.Bind("Difficulty Scaling", "Ignore max cap?", false, "Will ignore max cap if true, upperlimit is dictated by difficulty max cap setting as well.");
            difficultyMaxCap = difficultyConfig.Bind("Difficulty Scaling", "Difficulty max cap", 100.0f, "The difficulty value wont go beyond this.");
            scaleByDaysPassed = difficultyConfig.Bind("Difficulty Scaling", "Scale by days passed?", true, "Will add to difficulty depending on how many days have passed.");
            daysPassedDifficultyMultiplier = difficultyConfig.Bind("Difficulty Scaling", "Difficulty per days passed?", 1.0f, "");
            daysPassedDifficultyCap = difficultyConfig.Bind("Difficulty Scaling", "Days passed difficulty cap", 60.0f, "Days passed difficulty scaling wont add beyond this.");
            scaleByScrapInShip = difficultyConfig.Bind("Difficulty Scaling", "Scale by scrap in ship?", true, "Will add to difficulty depending on how much scrap is inside the ship.");
            scrapInShipDifficultyMultiplier = difficultyConfig.Bind("Difficulty Scaling", "Difficulty per scrap value in ship?", 0.0025f, "By default +1.0 per 400 scrap in ship");
            scrapInShipDifficultyCap = difficultyConfig.Bind("Difficulty Scaling", "Scrap in ship difficulty cap", 30.0f, "Scrap in ship difficulty scaling wont add beyond this.");
            scaleByQuota = difficultyConfig.Bind("Difficulty Scaling", "Scale by quota?", false, "Will add to difficulty depending on how high the quota is.");
            quotaDifficultyMultiplier = difficultyConfig.Bind("Difficulty Scaling", "Difficulty per quota value?", 0.005f, "By default +1.0 per 200 quota");
            quotaDifficultyCap = difficultyConfig.Bind("Difficulty Scaling", "Quota difficulty cap", 100.0f, "Quota scaling wont add difficulty beyond this");
            scaleByMoonGrade = difficultyConfig.Bind("Difficulty Scaling", "Scale by moon grade?", true, "Will add to difficulty depending on grade of moon you land on.");
            gradeAdditives = GetMoonRiskFromString(difficultyConfig.Bind("Difficulty Scaling", "Grade difficulty scaling", "D,-8|C,-8|B,-4|A,5|S,10|S+,15|S++,20|S+++,30|Other,10", "Format: GRADE,DIFFICULTY, Do not remove 'Other'").Value);
            scaleByWeather = difficultyConfig.Bind("Difficulty Scaling", "Scale by weather type?", false, "Will add to difficulty depending on weather of moon you land on.");
            weatherAdditives = new Dictionary<LevelWeatherType, float>()
            {
                { LevelWeatherType.None, difficultyConfig.Bind("Difficulty Scaling", "None weather difficulty", 0.0f, "Difficulty added for playing on None weather").Value },
                { LevelWeatherType.Rainy, difficultyConfig.Bind("Difficulty Scaling", "Rainy weather difficulty", 2.0f, "Difficulty added for playing on Rainy weather").Value },
                { LevelWeatherType.DustClouds, difficultyConfig.Bind("Difficulty Scaling", "DustClouds weather difficulty", 2.0f, "Difficulty added for playing on DustClouds weather").Value },
                { LevelWeatherType.Flooded, difficultyConfig.Bind("Difficulty Scaling", "Flooded weather difficulty", 4.0f, "Difficulty added for playing on Flooded weather").Value },
                { LevelWeatherType.Foggy, difficultyConfig.Bind("Difficulty Scaling", "Foggy weather difficulty", 4.0f, "Difficulty added for playing on Foggy weather").Value },
                { LevelWeatherType.Stormy, difficultyConfig.Bind("Difficulty Scaling", "Stormy weather difficulty", 7.0f, "Difficulty added for playing on Stormy weather").Value },
                { LevelWeatherType.Eclipsed, difficultyConfig.Bind("Difficulty Scaling", "Eclipsed weather difficulty", 7.0f, "Difficulty added for playing on Eclipsed weather").Value },
            };

            spawnChanceMultiplierScaling = getScale(difficultyConfig.Bind("Difficulty", "Spawn chance multiplier scale", "1.0, 0.017, 1.0, 2.0", "This will multiply the spawn chance by this,   " + scaleDescription).Value);
            insideSpawnChanceAdditive = getScale(difficultyConfig.Bind("Difficulty", "Inside spawn chance additive", "0.0, 0.0, 0.0, 0.0", "This will add to all keyframes for insideSpawns on the animationCurve,   " + scaleDescription).Value);
            outsideSpawnChanceAdditive = getScale(difficultyConfig.Bind("Difficulty", "Outside spawn chance additive", "0.0, 0.0, 0.0, 0.0", "This will add to all keyframes for outsideSpawns on the animationCurve,   " + scaleDescription).Value);
            spawnCapMultiplier = getScale(difficultyConfig.Bind("Difficulty", "Spawn cap multipler scale", "1.0, 0.017, 1.0, 2.0", "This will multiply outside and inside power counts by this,   " + scaleDescription).Value);
            insideEnemyMaxPowerCountScaling = getScale(difficultyConfig.Bind("Difficulty", "Additional Inside Max Enemy Power Count", "0, 0, 0, 0", "Added max enemy power count for inside enemies.,   " + scaleDescription).Value);
            outsideEnemyPowerCountScaling = getScale(difficultyConfig.Bind("Difficulty", "Additional Outside Max Enemy Power Count", "0, 0, 0, 0", "Added max enemy power count for outside enemies.,   " + scaleDescription).Value);
            enemyBonusHpScaling = getScale(difficultyConfig.Bind("Difficulty", "Additional hp scale", "0, 0, 0, 0", "Added hp to all enemies,   " + scaleDescription).Value);
            scrapValueMultiplier = getScale(difficultyConfig.Bind("Difficulty", "Scrap value multiplier scale", "1.0, 0.003, 1.0, 1.3", "Global scrap value multiplier,   " + scaleDescription).Value);
            scrapAmountMultiplier = getScale(difficultyConfig.Bind("Difficulty", "Scrap amount multiplier scale", "1.0, 0.003, 1.0, 1.3", "Global scrap amount multiplier,   " + scaleDescription).Value);
            goodEventIncrementMultiplier = difficultyConfig.Bind("Difficulty", "Global multiplier for increment value on good and veryGood eventTypes.", 1.0f);
            badEventIncrementMultiplier = difficultyConfig.Bind("Difficulty", "Global multiplier for increment value on bad and veryBad eventTypes.", 1.0f);

            enablePlayerScaling = difficultyConfig.Bind("Player Scaling", "Enable player scaling?", false, "Enable player scaling");
            playerScalingType = difficultyConfig.Bind("Player Scaling", "Player scaling type", "Linear", "Type of scaling for player amount. Options: Linear, Exponential");
            playerScalingMultiplier = difficultyConfig.Bind("Player Scaling", "Player scaling multiplier", 1.0f, "Multiplier for player scaling");
            basePlayerAmount = difficultyConfig.Bind("Player Scaling", "Base player amount", 4, "Base player amount");


            Scale bindEventTypeScrapAmountMultiplier(EventType difficulty)
                => getScale(difficultyConfig.Bind("_EventType Scrap Multipliers", difficulty + " scrap amount scale", "1, 0.0, 1, 1", scaleDescription).Value);
            Scale bindEventTypeScrapValueMultiplier(EventType difficulty)
                => getScale(difficultyConfig.Bind("_EventType Scrap Multipliers", difficulty + " scrap value scale", "1, 0.0, 1, 1", scaleDescription).Value);
            foreach (EventType difficulty in (EventType[])Enum.GetValues(typeof(EventType)))
            {
                scrapAmountByEventTypeScale.Add(difficulty, bindEventTypeScrapAmountMultiplier(difficulty));
                scrapValueByEventTypeScale.Add(difficulty, bindEventTypeScrapValueMultiplier(difficulty));
            }

            MoonsToIgnore = difficultyConfig.Bind("Moons Settings", "Moons to Not Spawn Events On", "", "Events will not spawn on these moons. Seperate by comma for each moon name.");
            //FactorySize = getScale(difficultyConfig.Bind("Moons Settings", "Factory Size multiplier scale", "1.0, 0.012, 1.0, 2.0", "This will multiply the factory size by this. Avoid negatives, zero or any numbers too big.   " + scaleDescription).Value);


            // Custom scrap settings
            nutSlayerLives = customAssetsConfig.Bind("NutSlayer", "Lives", 5, "If hp reaches zero or below, decrement lives and reset hp until 0 lives.");
            nutSlayerHp = customAssetsConfig.Bind("NutSlayer", "Hp", 6);
            nutSlayerMovementSpeed = customAssetsConfig.Bind("NutSlayer", "Speed?", 9.5f);
            nutSlayerImmortal = customAssetsConfig.Bind("NutSlayer", "Immortal?", true);
            grabbableTurret.minValue = customAssetsConfig.Bind("Grabbable Landmine", "Min value", 50).Value;
            grabbableTurret.maxValue = customAssetsConfig.Bind("Grabbable Landmine", "Max value", 75).Value;
            grabbableLandmine.minValue = customAssetsConfig.Bind("Grabbable Turret", "Min value", 100).Value;
            grabbableLandmine.maxValue = customAssetsConfig.Bind("Grabbable Turret", "Max value", 150).Value;
            slayerShotgunMinValue = customAssetsConfig.Bind("Slayer Shotgun", "Min value", 200);
            slayerShotgunMaxValue = customAssetsConfig.Bind("Slayer Shotgun", "Max value", 300);

            // Weather settings
            useWeatherMultipliers = weatherConfig.Bind("_Weather Settings", "Enable weather multipliers?", true, "'false'= Disable all weather multipliers     'true'= Enable weather multipliers");
            randomizeWeatherMultipliers = weatherConfig.Bind("_Weather Settings", "Weather multiplier randomness?", false, "'false'= disable     'true'= enable");
            enableTerminalText = weatherConfig.Bind("_Weather Settings", "Enable terminal text?", true);

            weatherRandomRandomMinInclusive = weatherConfig.Bind("_Weather Random Multipliers", "Min Inclusive", 0.9f, "Lower bound of random value");
            weatherRandomRandomMaxInclusive = weatherConfig.Bind("_Weather Random Multipliers", "Max Inclusive", 1.2f, "Upper bound of random value");

            Weather createWeatherSettings(Weather weather)
            {
                string configHeader = "(" + weather.weatherType.ToString() + ") Weather multipliers";

                float valueMultiplierSetting = weatherConfig.Bind(configHeader, "Scrap Value Multiplier", weather.scrapValueMultiplier, "Multiply Scrap value for " + weather.weatherType.ToString()).Value;
                float amountMultiplierSetting = weatherConfig.Bind(configHeader, "Scrap Amount Multiplier", weather.scrapAmountMultiplier, "Multiply Scrap amount for " + weather.weatherType.ToString()).Value;

                return new Weather(weather.weatherType, valueMultiplierSetting, amountMultiplierSetting);
            }

            noneMultiplier = createWeatherSettings(new Weather(LevelWeatherType.None, 1.00f, 1.00f));
            dustCloudMultiplier = createWeatherSettings(new Weather(LevelWeatherType.DustClouds, 1.05f, 1.00f));
            rainyMultiplier = createWeatherSettings(new Weather(LevelWeatherType.Rainy, 1.05f, 1.00f));
            stormyMultiplier = createWeatherSettings(new Weather(LevelWeatherType.Stormy, 1.35f, 1.20f));
            foggyMultiplier = createWeatherSettings(new Weather(LevelWeatherType.Foggy, 1.15f, 1.10f));
            floodedMultiplier = createWeatherSettings(new Weather(LevelWeatherType.Flooded, 1.25f, 1.15f));
            eclipsedMultiplier = createWeatherSettings(new Weather(LevelWeatherType.Eclipsed, 1.35f, 1.20f));

            // UI Settings
            UIKey = uiConfig.Bind("UI Options", "Toggle UI Key", "K");
            NormaliseScrapValueDisplay = uiConfig.Bind("UI Options", "Normlize scrap value display number?", true, "In game default value is 0.4, having this set to true will multiply the 'displayed value' by 2.5 so it looks normal.");
            EnableUI = uiConfig.Bind("UI Options", "Enable UI?", true);
            ShowUILetterBox = uiConfig.Bind("UI Options", "Display UI Letter Box?", true);
            ShowExtraProperties = uiConfig.Bind("UI Options", "Display extra properties", true, "Display extra properties on UI such as scrap value and amount multipliers.");
            PopUpUI = uiConfig.Bind("UI Options", "PopUp UI?", true, "Will the UI popup whenever you start the day?");
            UITime = uiConfig.Bind("UI Options", "PopUp UI time.", 45.0f, "UI popup time");
            scrollSpeed = uiConfig.Bind("UI Options", "Scroll speed", 1.0f, "Multiplier speed on scrolling with arrows.");
            DisplayUIAfterShipLeaves = uiConfig.Bind("UI Options", "Display UI after ship leaves?", false, "Will only display event's after ship has left.");
            DisplayExtraPropertiesAfterShipLeaves = uiConfig.Bind("UI Options", "Display extra properties on UI after ship Leaves?", true, "This will show Event Type raritys for next day and difficulty info.");
            displayEvents = uiConfig.Bind("UI Options", "Display events?", true, "Having this set to false wont show events in the UI.");

            //Core Properties
            enableCustomEvents = CorePropertiesConfig.Bind("Custom Events", "Enable Custom Events?", true, "Enables custom events to be loaded from the custom events folder.");
            ExtraLogging = CorePropertiesConfig.Bind("General", "Enable Extra Logging?", false, "Enables extra logging for debugging purposes.");
            //todo - add settings for events and modded events that mess with features like time
            HellTimeAdjustment = CorePropertiesConfig.Bind("Events Features", "Enable Hell Time Adjustment?", true, "When the Hell event occurs, should the time be adjusted to be normal? Disable if you wish to suffer.");
            VeryLateShipAdjustment = CorePropertiesConfig.Bind("Events Features", "Enable Very Late Ship Time Adjustment?", true, "When the VeryLateShip event occurs, should the time be adjusted to be normal? Disable if you wish to suffer.");



            //Custom Events Folder
            try
            {
                if (!System.IO.Directory.Exists(customEventsFolder))
                {
                    System.IO.Directory.CreateDirectory(customEventsFolder);
                }
            }
            catch (Exception e)
            {
                Log.LogWarning("Failed to create custom events folder: " + e.Message);
            }

            /*   EnableStreamerEvents = extensiveSettingsConfig.Bind("Extra Options", "Enable Streamer events?", true, "Enables streamer specific events");*/


            // Event settings
            void RegisterEvents(ConfigFile toConfig, List<MEvent> events)
            {

                // Event settings
                foreach (MEvent e in events)
                {
                    eventWeights.Add(toConfig.Bind(e.Name(), "Custom Weight", e.Weight, "If you want to use custom weights change 'Use custom weights'? setting in '__Event Settings' to true."));
                    eventDescriptions.Add(ListToDescriptions(toConfig.Bind(e.Name(), "Descriptions", StringsToList(e.Descriptions, "|"), "Seperated by |").Value));
                    eventColorHexes.Add(toConfig.Bind(e.Name(), "Color Hex", e.ColorHex));
                    eventTypes.Add(toConfig.Bind(e.Name(), "Event Type", e.Type));
                    eventEnables.Add(toConfig.Bind(e.Name(), "Event Enabled?", e.Enabled, "Setting this to false will stop the event from occuring.")); // Normal event

                    // Make scale list
                    Dictionary<ScaleType, Scale> scales = new Dictionary<ScaleType, Scale>();
                    foreach (KeyValuePair<ScaleType, Scale> obj in e.ScaleList)
                    {
                        scales.Add(obj.Key, getScale(toConfig.Bind(e.Name(), obj.Key.ToString(), GetStringFromScale(obj.Value), ScaleInfoList[obj.Key] + "   " + scaleDescription).Value));
                    }
                    eventScales.Add(scales);

                    // EventsToRemove and EventsToSpawnWith
                    eventsToRemove.Add(ListToStrings(toConfig.Bind(e.Name(), "Events To Remove", StringsToList(e.EventsToRemove, ", "), "Will prevent said event(s) from occuring.").Value));
                    eventsToSpawnWith.Add(ListToStrings(toConfig.Bind(e.Name(), "Events To Spawn With", StringsToList(e.EventsToSpawnWith, ", "), "Will spawn said events(s).").Value));

                    // Monster events
                    List<MonsterEvent> newMonsterEvents = new List<MonsterEvent>();
                    for (int i = 0; i < e.monsterEvents.Count; i++)
                    {
                        newMonsterEvents.Add(new MonsterEvent(
                            toConfig.Bind(e.Name(), $"Enemy {i} Name", e.monsterEvents[i].enemy.name, "Inputting an invalid enemy name will cause it to return an empty enemyType").Value,
                            getScale(toConfig.Bind(e.Name(), $"{e.monsterEvents[i].enemy.name} {ScaleType.InsideEnemyRarity}", GetStringFromScale(e.monsterEvents[i].insideSpawnRarity), $"{ScaleInfoList[ScaleType.InsideEnemyRarity]}   {scaleDescription}").Value),
                            getScale(toConfig.Bind(e.Name(), $"{e.monsterEvents[i].enemy.name} {ScaleType.OutsideEnemyRarity}", GetStringFromScale(e.monsterEvents[i].outsideSpawnRarity), $"{ScaleInfoList[ScaleType.OutsideEnemyRarity]}   {scaleDescription}").Value),
                            getScale(toConfig.Bind(e.Name(), $"{e.monsterEvents[i].enemy.name} {ScaleType.MinInsideEnemy}", GetStringFromScale(e.monsterEvents[i].minInside), $"{ScaleInfoList[ScaleType.MinInsideEnemy]}   {scaleDescription}").Value),
                            getScale(toConfig.Bind(e.Name(), $"{e.monsterEvents[i].enemy.name} {ScaleType.MaxInsideEnemy}", GetStringFromScale(e.monsterEvents[i].maxInside), $"{ScaleInfoList[ScaleType.MaxInsideEnemy]}   {scaleDescription}").Value),
                            getScale(toConfig.Bind(e.Name(), $"{e.monsterEvents[i].enemy.name} {ScaleType.MinOutsideEnemy}", GetStringFromScale(e.monsterEvents[i].minOutside), $"{ScaleInfoList[ScaleType.MinOutsideEnemy]}   {scaleDescription}").Value),
                            getScale(toConfig.Bind(e.Name(), $"{e.monsterEvents[i].enemy.name} {ScaleType.MaxOutsideEnemy}", GetStringFromScale(e.monsterEvents[i].maxOutside), $"{ScaleInfoList[ScaleType.MaxOutsideEnemy]}   {scaleDescription}").Value)
                        ));
                    }
                    monsterEvents.Add(newMonsterEvents);

                    // Scrap transmutation events
                    Scale amount = new Scale(0.0f, 0.0f, 0.0f, 0.0f);
                    if (e.scrapTransmutationEvent.items.Length > 0) amount = getScale(toConfig.Bind(e.Name(), "Percentage", GetStringFromScale(e.scrapTransmutationEvent.amount), $"{ScaleInfoList[ScaleType.Percentage]}   {scaleDescription}").Value);
                    SpawnableItemWithRarity[] newScrapTransmuations = new SpawnableItemWithRarity[e.scrapTransmutationEvent.items.Length];
                    for (int i = 0; i < e.scrapTransmutationEvent.items.Length; i++)
                    {
                        newScrapTransmuations[i] = new SpawnableItemWithRarity()
                        {
                            spawnableItem = GetItem(toConfig.Bind(e.Name(), $"Scrap {i} name", e.scrapTransmutationEvent.items[i].spawnableItem.name, "Inputting an invalid scrap name will cause it to return an empty item").Value),
                            rarity = toConfig.Bind(e.Name(), $"{e.scrapTransmutationEvent.items[i].spawnableItem.name} Rarity", e.scrapTransmutationEvent.items[i].rarity).Value
                        };
                    }
                    transmutationEvents.Add(new ScrapTransmutationEvent(amount, newScrapTransmuations));
                }

            }

            RegisterEvents(eventConfig, EventManager.vanillaEvents);
            RegisterEvents(moddedEventConfig, EventManager.moddedEvents);

            if (enableCustomEvents.Value)
            {
                foreach (string eventFile in System.IO.Directory.GetFiles(customEventsFolder))
                {
                    if (eventFile.EndsWith(".json"))
                    {
                        MEvent cEvent = new GeneralCustomEvent(eventFile);
                        cEvent.Initalize();

                        if (cEvent.Enabled) EventManager.customEvents.Add(cEvent);
                    }
                }
                RegisterEvents(customEventConfig, EventManager.customEvents);
            }

            /*foreach (EventManager.CustomEvents customevent in EventManager.customEventsList)
            {
                foreach (MEvent e in customevent.events)
                {
                    e.Initalize();
                }

                RegisterEvents(customevent.configFile, customevent.events);
                EventManager.customEvents.AddRange(customevent.events);
            }
            EventManager.customEventsList.Clear();*/

            EventManager.events.AddRange(EventManager.vanillaEvents);
            EventManager.events.AddRange(EventManager.moddedEvents);
            EventManager.events.AddRange(EventManager.customEvents);

            // Specific event settings
            Minus.Handlers.FacilityGhost.actionTimeCooldown = eventConfig.Bind(nameof(FacilityGhost), "Normal Action Time Interval", 15.0f, "How often does it take for the ghost to make a decision?").Value;
            Minus.Handlers.FacilityGhost.ghostCrazyActionInterval = eventConfig.Bind(nameof(FacilityGhost), "Crazy Action Time Interval", 0.1f, "How often does it take for the ghost to make a decision while going crazy?").Value;
            Minus.Handlers.FacilityGhost.ghostCrazyPeriod = eventConfig.Bind(nameof(FacilityGhost), "Crazy Period", 5.0f, "How long will the ghost go crazy for?").Value;
            Minus.Handlers.FacilityGhost.crazyGhostChance = eventConfig.Bind(nameof(FacilityGhost), "Crazy Chance", 0.1f, "Whenever the ghost makes a decision, what is the chance that it will go crazy?").Value;
            Minus.Handlers.FacilityGhost.DoNothingWeight = eventConfig.Bind(nameof(FacilityGhost), "Do Nothing Weight?", 25, "Whenever the ghost makes a decision, what is the weight to do nothing?").Value;
            Minus.Handlers.FacilityGhost.OpenCloseBigDoorsWeight = eventConfig.Bind(nameof(FacilityGhost), "Open and close big doors weight", 20, "Whenever the ghost makes a decision, what is the weight for ghost to open and close big doors?").Value;
            Minus.Handlers.FacilityGhost.MessWithLightsWeight = eventConfig.Bind(nameof(FacilityGhost), "Mess with lights weight", 16, "Whenever the ghost makes a decision, what is the weight to mess with lights?").Value;
            Minus.Handlers.FacilityGhost.MessWithBreakerWeight = eventConfig.Bind(nameof(FacilityGhost), "Mess with breaker weight", 4, "Whenever the ghost makes a decision, what is the weight to mess with the breaker?").Value;
            Minus.Handlers.FacilityGhost.disableTurretsWeight = eventConfig.Bind(nameof(FacilityGhost), "Disable turrets weight?", 5, "Whenever the ghost makes a decision, what is the weight to attempt to disable the turrets?").Value;
            Minus.Handlers.FacilityGhost.disableLandminesWeight = eventConfig.Bind(nameof(FacilityGhost), "Disable landmines weight?", 5, "Whenever the ghost makes a decision, what is the weight to attempt to disable the landmines?").Value;
            Minus.Handlers.FacilityGhost.turretRageWeight = eventConfig.Bind(nameof(FacilityGhost), "Turret rage weight?", 5, "Whenever the ghost makes a decision, what is the weight to attempt to make turrets rage?").Value;
            Minus.Handlers.FacilityGhost.OpenCloseDoorsWeight = eventConfig.Bind(nameof(FacilityGhost), "Open and close normal doors weight", 9, "Whenever the ghost makes a decision, what is the weight to attempt to open and close normal doors.").Value;
            Minus.Handlers.FacilityGhost.lockUnlockDoorsWeight = eventConfig.Bind(nameof(FacilityGhost), "Lock and unlock normal doors weight", 3, "Whenever the ghost makes a decision, what is the weight to attempt to lock and unlock normal doors.").Value;
            Minus.Handlers.FacilityGhost.chanceToOpenCloseDoor = eventConfig.Bind(nameof(FacilityGhost), "Chance to open and close normal doors", 0.3f, "Whenever the ghosts decides to open and close doors, what is the chance for each individual door that it will do that.").Value;
            Minus.Handlers.FacilityGhost.rageTurretsChance = eventConfig.Bind(nameof(FacilityGhost), "Chance to rage a turret", 0.3f, "Whenever the ghosts decides to rage a turret, what is the chance for each individual turret that it will do that.").Value;

            Minus.Handlers.RealityShift.normalScrapWeight = eventConfig.Bind(nameof(RealityShift), "Normal shift weight", 85, "Weight for transforming scrap into some other scrap?").Value;
            Minus.Handlers.RealityShift.grabbableLandmineWeight = eventConfig.Bind(nameof(RealityShift), "Grabbable landmine shift weight", 15, "Weight for transforming scrap into a grabbable landmine?").Value;
            Minus.Handlers.RealityShift.transmuteChance = eventConfig.Bind(nameof(RealityShift), "Chance to transmute", 0.5f, "Chance for transmutation to occur.").Value;
            Minus.Handlers.RealityShift.enemyTeleportChance = eventConfig.Bind(nameof(RealityShift), "Enemy teleport chance", 0.1f, "Chance enemy teleportation to occur when hit.").Value;


            DDay.bombardmentInterval = eventConfig.Bind(nameof(Warzone), "Bombardment interval", 100, "The time it takes before each bombardment event.").Value;
            DDay.bombardmentTime = eventConfig.Bind(nameof(Warzone), "Bombardment time", 15, "When a bombardment event occurs, how long will it last?").Value;
            DDay.fireInterval = eventConfig.Bind(nameof(Warzone), "Fire interval", 1, "During a bombardment event how often will it fire?").Value;
            DDay.fireAmount = eventConfig.Bind(nameof(Warzone), "Fire amount", 8, "For every fire interval, how many shot's will it take? This will get scaled higher on bigger maps.").Value;
            DDay.displayWarning = eventConfig.Bind(nameof(Warzone), "Display warning?", true, "Display warning message before bombardment?").Value;
            DDay.volume = eventConfig.Bind(nameof(Warzone), "Siren Volume?", 0.3f, "Volume of the siren? between 0.0 and 1.0").Value;
            ArtilleryShell.speed = eventConfig.Bind(nameof(Warzone), "Artillery shell speed", 100.0f, "How fast does the artillery shell travel?").Value;

            Minus.Handlers.Mimics.spawnRateScales[0] = getScale(moddedEventConfig.Bind(nameof(Mimics), "Zero Mimics Scale", "0, 0, 0, 0", "Weight Scale of zero mimics spawning   " + scaleDescription).Value);
            Minus.Handlers.Mimics.spawnRateScales[1] = getScale(moddedEventConfig.Bind(nameof(Mimics), "One Mimic Scale", "0, 0, 0, 0", "Weight Scale of one mimic spawning   " + scaleDescription).Value);
            Minus.Handlers.Mimics.spawnRateScales[2] = getScale(moddedEventConfig.Bind(nameof(Mimics), "Two Mimics Scale", "0, 0, 0, 0", "Weight Scale of two mimics spawning   " + scaleDescription).Value);
            Minus.Handlers.Mimics.spawnRateScales[3] = getScale(moddedEventConfig.Bind(nameof(Mimics), "Three Mimics Scale", "80.0, -1.25, 5.0, 80.0", "Weight Scale of three mimics spawning   " + scaleDescription).Value);
            Minus.Handlers.Mimics.spawnRateScales[4] = getScale(moddedEventConfig.Bind(nameof(Mimics), "Four Mimics Scale", "40.0, -0.5, 10.0, 40.0", "Weight Scale of four mimics spawning   " + scaleDescription).Value);
            Minus.Handlers.Mimics.spawnRateScales[5] = getScale(moddedEventConfig.Bind(nameof(Mimics), "Maximum Mimics Scale", "10.0, 0.84, 10.0, 60.0", "Weight Scale of maximum mimics spawning   " + scaleDescription).Value);

            // Level properties config
            foreach (SelectableLevel level in StartOfRound.Instance.levels)
            {
                if (level == null) continue;

                Scale minScrapAmount = getScale(levelPropertiesConfig.Bind($"{level.levelID}:{level.name}", "Min scrap amount scale", "1.0, 0.0, 1.0, 1.0", scaleDescription).Value);
                Scale maxScrapAmount = getScale(levelPropertiesConfig.Bind($"{level.levelID}:{level.name}", "Max scrap amount scale", "1.0, 0.0, 1.0, 1.0", scaleDescription).Value);
                Scale minScrapValue = getScale(levelPropertiesConfig.Bind($"{level.levelID}:{level.name}", "Min scrap value scale", "1.0, 0.0, 1.0, 1.0", scaleDescription).Value);
                Scale maxScrapValue = getScale(levelPropertiesConfig.Bind($"{level.levelID}:{level.name}", "Max scrap value scale", "1.0, 0.0, 1.0, 1.0", scaleDescription).Value);

                levelProperties.TryAdd(level.levelID, new LevelProperties(level.levelID, minScrapAmount, maxScrapAmount, minScrapValue, maxScrapValue));
            }

        }

        internal static bool Initalized = false;
        public static void CreateConfig()
        {
            if (Initalized) return;

            // Initalize Events
            foreach (MEvent e in EventManager.vanillaEvents) e.Initalize();
            foreach (MEvent e in EventManager.moddedEvents) e.Initalize();

            // Config
            Initalize();

            // Use config settings
            for (int i = 0; i != EventManager.events.Count; i++)
            {
                EventManager.events[i].Weight = eventWeights[i].Value;
                EventManager.events[i].Descriptions = eventDescriptions[i];
                EventManager.events[i].ColorHex = eventColorHexes[i].Value;
                EventManager.events[i].Type = eventTypes[i].Value;
                EventManager.events[i].ScaleList = eventScales[i];
                EventManager.events[i].Enabled = eventEnables[i].Value;
                EventManager.events[i].EventsToRemove = eventsToRemove[i];
                EventManager.events[i].EventsToSpawnWith = eventsToSpawnWith[i];
                EventManager.events[i].monsterEvents = monsterEvents[i];
                EventManager.events[i].scrapTransmutationEvent = transmutationEvents[i];
            }

            // Create disabled events list and update
            List<MEvent> newEvents = new List<MEvent>();
            foreach (MEvent e in EventManager.events)
            {
                if (!e.Enabled)
                {
                    EventManager.disabledEvents.Add(e);
                }
                else
                {
                    newEvents.Add(e);
                    switch (e.Type)
                    {
                        case EventType.VeryBad:
                            EventManager.allVeryBad.Add(e);
                            break;
                        case EventType.Bad:
                            EventManager.allBad.Add(e);
                            break;
                        case EventType.Neutral:
                            EventManager.allNeutral.Add(e);
                            break;
                        case EventType.Good:
                            EventManager.allGood.Add(e);
                            break;
                        case EventType.VeryGood:
                            EventManager.allVeryGood.Add(e);
                            break;
                        case EventType.Remove:
                            EventManager.allRemove.Add(e);
                            break;
                    }
                }
            }
            EventManager.events = newEvents;

            EventManager.UpdateEventTypeCounts();
            EventManager.UpdateAllEventWeights();

            Log.LogInfo(
                $"\n\nTotal Events:{EventManager.events.Count},   Disabled Events:{EventManager.disabledEvents.Count},   Total Events - Remove Count:{EventManager.events.Count - EventManager.eventTypeCount[5]}\n" +
                $"Very Bad:{EventManager.eventTypeCount[0]}\n" +
                $"Bad:{EventManager.eventTypeCount[1]}\n" +
                $"Neutral:{EventManager.eventTypeCount[2]}\n" +
                $"Good:{EventManager.eventTypeCount[3]}\n" +
                $"Very Good:{EventManager.eventTypeCount[4]}\n" +
                $"Remove:{EventManager.eventTypeCount[5]}\n");

            CreateAllEnemiesConfig();

            uiConfig.Save();
            difficultyConfig.Save();
            eventConfig.Save();
            weatherConfig.Save();
            customAssetsConfig.Save();
            moddedEventConfig.Save();
            //customEventConfig.Save();
            allEnemiesConfig.Save();
            levelPropertiesConfig.Save();
            CorePropertiesConfig.Save();
            //   extensiveSettingsConfig.Save();

            Initalized = true;
        }

        private static void CreateAllEnemiesConfig()
        {
            enableAllEnemies = allEnemiesConfig.Bind("_All Enemies", "Enable?", false, "This will make all enemies capable of spawning on all moons, this will make the game harder.");
            enableAllAllEnemies = allEnemiesConfig.Bind("_All All Enemies", "Enable?", false, "This will make all inside enemies spawn inside and outside and all outside enemies spawn inside and outside, so giants and worms can spawn inside, enable both 'All' and 'All All' if you are a sadist. This will make the game harder.");

            List<EnemySpawnInfo> allSpawnInfos = new List<EnemySpawnInfo>()
            {
                // Inside
                CreateEnemyEntry(EnemyName.Bracken, 8, 1, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.HoardingBug, 60, 10, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.CoilHead, 20, 5, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.Thumper, 25, 5, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.BunkerSpider, 35, 5, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.Jester, 7, 1, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.SnareFlea, 45, 5, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.Hygrodere, 10, 3, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.GhostGirl, 5, 1, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.SporeLizard, 15, 3, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.NutCracker, 15, 3, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.Masked, 10, 3, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.Butler, 20, 3, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.Lasso, 5, 1, SpawnLocation.Inside),
                CreateEnemyEntry(kamikazieBug.name, 30, 5, SpawnLocation.Inside),
                CreateEnemyEntry(antiCoilHead.name, 10, 2, SpawnLocation.Inside),
                CreateEnemyEntry(nutSlayer.name, 3, 1, SpawnLocation.Inside),
                // Outside
                CreateEnemyEntry(EnemyName.EyelessDog, 25, 5, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.ForestKeeper, 10, 3, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.EarthLeviathan, 8, 3, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.BaboonHawk, 35, 10, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.OldBird, 6, 3, SpawnLocation.Outside)
            };

            foreach (EnemyType enemy in EnemyList.Values)
            {
                if (enemy == null || enemy.enemyPrefab == null || enemy.isDaytimeEnemy || allSpawnInfos.Exists(x => x.enemy.name == enemy.name)) continue;

                if (enemy.isOutsideEnemy)
                {
                    CreateEnemyEntry(enemy.name, 5, 1, SpawnLocation.Outside);
                }
                else
                {
                    CreateEnemyEntry(enemy.name, 5, 1, SpawnLocation.Inside);
                }
            }

            allEnemiesCycle = new SpawnCycle()
            {
                enemies = allSpawnInfos,
                nothingWeight = allEnemiesConfig.Bind("_All Enemies", "All enemies nothing weight", 400.0f, "This is the weight chance for a spawn to not occur.").Value,
                spawnAttemptInterval = allEnemiesConfig.Bind("_All Enemies", "Spawn interval", 86.0f, "How often will this cycle attempt to spawn an enemy? in seconds").Value,
                spawnCycleDuration = 0.0f
            };

            header = "All All Enemies";
            List<EnemySpawnInfo> allAllSpawnInfos = new List<EnemySpawnInfo>()
            {
                // Inside
                CreateEnemyEntry(EnemyName.Bracken, 8, 1, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.HoardingBug, 60, 10, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.CoilHead, 20, 5, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.Thumper, 25, 5, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.BunkerSpider, 35, 5, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.Jester, 7, 1, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.SnareFlea, 45, 5, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.Hygrodere, 10, 3, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.GhostGirl, 5, 1, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.SporeLizard, 15, 3, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.NutCracker, 15, 3, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.Masked, 10, 3, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.Butler, 20, 3, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.Lasso, 5, 1, SpawnLocation.Inside),
                CreateEnemyEntry(kamikazieBug.name, 30, 5, SpawnLocation.Inside),
                CreateEnemyEntry(antiCoilHead.name, 10, 2, SpawnLocation.Inside),
                CreateEnemyEntry(nutSlayer.name, 3, 1, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.EyelessDog, 10, 5, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.ForestKeeper, 6, 3, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.EarthLeviathan, 8, 3, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.BaboonHawk, 20, 10, SpawnLocation.Inside),
                CreateEnemyEntry(EnemyName.OldBird, 6, 3, SpawnLocation.Inside),
                // Outside
                CreateEnemyEntry(EnemyName.Bracken, 4, 1, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.HoardingBug, 30, 10, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.CoilHead, 10, 5, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.Thumper, 13, 5, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.BunkerSpider, 18, 5, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.Jester, 3, 1, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.SnareFlea, 10, 5, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.Hygrodere, 5, 3, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.GhostGirl, 3, 1, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.SporeLizard, 7, 3, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.NutCracker, 8, 3, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.Masked, 5, 3, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.Butler, 10, 3, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.Lasso, 3, 1, SpawnLocation.Outside),
                CreateEnemyEntry(kamikazieBug.name, 15, 5, SpawnLocation.Outside),
                CreateEnemyEntry(antiCoilHead.name, 5, 2, SpawnLocation.Outside),
                CreateEnemyEntry(nutSlayer.name, 2, 1, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.EyelessDog, 15, 5, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.ForestKeeper, 10, 3, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.EarthLeviathan, 12, 3, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.BaboonHawk, 35, 10, SpawnLocation.Outside),
                CreateEnemyEntry(EnemyName.OldBird, 10, 3, SpawnLocation.Outside)
            };

            foreach (EnemyType enemy in EnemyList.Values)
            {
                if (enemy == null || enemy.enemyPrefab == null || allSpawnInfos.Exists(x => x.enemy.name == enemy.name)) continue;

                CreateEnemyEntry(enemy.name, 5, 1, SpawnLocation.Inside);
                CreateEnemyEntry(enemy.name, 5, 1, SpawnLocation.Outside);
            }

            allAllEnemiesCycle = new SpawnCycle()
            {
                enemies = allAllSpawnInfos,
                nothingWeight = allEnemiesConfig.Bind("_All All Enemies", "All enemies nothing weight", 400.0f, "This is the weight chance for a spawn to not occur.").Value,
                spawnAttemptInterval = allEnemiesConfig.Bind("_All All Enemies", "Spawn interval", 86.0f, "How often will this cycle attempt to spawn enemies? in seconds").Value,
                spawnCycleDuration = 0.0f
            };
        }

        private static string header = "All Enemies";
        private static EnemySpawnInfo CreateEnemyEntry(string enemy, float defaultWeight, int spawnCap, SpawnLocation spawnLocation)
        {
            return new EnemySpawnInfo()
            {
                enemy = GetEnemyOrDefault(enemy).enemyPrefab,
                enemyWeight = allEnemiesConfig.Bind(header, $"{spawnLocation} {enemy} Weight", defaultWeight, "weight").Value,
                spawnCap = allEnemiesConfig.Bind(header, $"{spawnLocation} {enemy} Spawn Cap", spawnCap, "weight").Value,
                spawnLocation = spawnLocation
            };
        }

        private static EnemySpawnInfo CreateEnemyEntry(EnemyName name, float defaultWeight, int spawnCap, SpawnLocation spawnLocation) => CreateEnemyEntry(EnemyNameList[name], defaultWeight, spawnCap, spawnLocation);

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TimeOfDay), "Awake")]
        private static void OnTimeDayStart(ref TimeOfDay __instance)
        {
            enableQuotaChanges = difficultyConfig.Bind("Quota Settings", "_Enable Quota Changes", false, "Once set to true, load up a save to generate the rest of this config, this will take values from the game as default.");
            if (enableQuotaChanges.Value)
            {
                __instance.quotaVariables.deadlineDaysAmount = difficultyConfig.Bind("Quota Settings", "Deadline Days Amount", __instance.quotaVariables.deadlineDaysAmount).Value;
                __instance.quotaVariables.startingCredits = difficultyConfig.Bind("Quota Settings", "Starting Credits", __instance.quotaVariables.startingCredits).Value;
                __instance.quotaVariables.startingQuota = difficultyConfig.Bind("Quota Settings", "Starting Quota", __instance.quotaVariables.startingQuota).Value;
                __instance.quotaVariables.baseIncrease = difficultyConfig.Bind("Quota Settings", "Base Increase", __instance.quotaVariables.baseIncrease).Value;
                __instance.quotaVariables.increaseSteepness = difficultyConfig.Bind("Quota Settings", "Increase Steepness", __instance.quotaVariables.increaseSteepness).Value;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Terminal), "Awake")]
        private static void OnTerminalAwake()
        {
            Manager.currentTerminal = GameObject.FindObjectOfType<Terminal>();
        }
    }
}
