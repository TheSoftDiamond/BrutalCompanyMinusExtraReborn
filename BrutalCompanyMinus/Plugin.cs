using System;
using HarmonyLib;
using BepInEx;
using UnityEngine;
using System.Reflection;
using System.Collections;
using BrutalCompanyMinus.Minus;
using System.Collections.Generic;
using BrutalCompanyMinus.Minus.Handlers;
using UnityEngine.Diagnostics;
using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine.InputSystem.HID;
using Discord;
using System.Diagnostics;
using BepInEx.Configuration;
using System.Globalization;
using static BrutalCompanyMinus.Configuration;

namespace BrutalCompanyMinus
{
    [HarmonyPatch]
    [BepInPlugin(GUID, NAME, VERSION)]
    internal class Plugin : BaseUnityPlugin
    {
        private const string GUID = "SoftDiamond.BrutalCompanyMinusExtraReborn";
        private const string NAME = "BrutalCompanyMinusExtraReborn";
        private const string VERSION = "0.21.6";
        private static readonly Harmony harmony = new Harmony(GUID);

        void Awake()
        {
            // Logger
            Log.Initalize(Logger);

            // Required for netweaving
            var EventTypes = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var EventType in EventTypes)
            {
                var methods = EventType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }
            
            // Load assets
            Assets.Load();

            // Create config files
            uiConfig = new ConfigFile(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\UI_Settings.cfg", true);
            difficultyConfig = new ConfigFile(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\Difficulty_Settings.cfg", true);
            eventConfig = new ConfigFile(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\VanillaEvents.cfg", true);
            weatherConfig = new ConfigFile(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\Weather_Settings.cfg", true);
            customAssetsConfig = new ConfigFile(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\Enemy_Scrap_Weights_Settings.cfg", true);
            moddedEventConfig = new ConfigFile(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\ModdedEvents.cfg", true);
            customEventConfig = new ConfigFile(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CustomEvents.cfg", true);
            allEnemiesConfig = new ConfigFile(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\AllEnemies.cfg", true);
            levelPropertiesConfig = new ConfigFile(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\LevelProperties.cfg", true);
            //  extensiveSettingsConfig = new ConfigFile(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\ExtensiveOptions.cfg", true);

            uiConfig.SaveOnConfigSet = false;
            difficultyConfig.SaveOnConfigSet = false;
            eventConfig.SaveOnConfigSet = false;
            weatherConfig.SaveOnConfigSet = false;
            customAssetsConfig.SaveOnConfigSet = false;
            moddedEventConfig.SaveOnConfigSet = false;
            customEventConfig.SaveOnConfigSet = false;
            allEnemiesConfig.SaveOnConfigSet = false;
            levelPropertiesConfig.SaveOnConfigSet = false;
         //   extensiveSettingsConfig.SaveOnConfigSet = false;

            // Patch all
            harmony.PatchAll();
            harmony.PatchAll(typeof(GrabObjectTranspiler));

            Log.LogInfo(NAME + " " + VERSION + " " + "is done patching.");
        }
    }
}
