using BepInEx;
using BepInEx.Configuration;
using BrutalCompanyMinus.Minus.Handlers;
using BrutalCompanyMinus.Minus.Handlers.Modded;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;
using static BrutalCompanyMinus.Configuration;

namespace BrutalCompanyMinus
{
    [BepInDependency("ShipInventory", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("FlipMods.HotbarPlus", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("mrov.WeatherRegistry", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("voxx.LethalElementsPlugin", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(GUID, NAME, VERSION)]
    internal class Plugin : BaseUnityPlugin
    {
        private const string GUID = "SoftDiamond.BrutalCompanyMinusExtraReborn";
        private const string NAME = "BrutalCompanyMinusExtraReborn";
        private const string VERSION = "0.24.0";

        internal static Plugin Instance { get; private set; }

        private static readonly Harmony harmony = new Harmony(GUID);

        #pragma warning disable IDE0051 // Remove unused private members
        private void Awake()
        #pragma warning restore IDE0051 // Remove unused private members
        {
            if (Instance == null) Instance = this;

            // Logger
            Log.Initalize(Logger);

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
            CorePropertiesConfig = new ConfigFile(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", true);
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
            CorePropertiesConfig.SaveOnConfigSet = false;
            //   extensiveSettingsConfig.SaveOnConfigSet = false;

            // Load assets
            Assets.Load();

            // Patch all
            harmony.PatchAll();
            harmony.PatchAll(typeof(GrabObjectTranspiler));

            if (Compatibility.IsModPresent("FlipMods.HotbarPlus"))
            {
                HotBarPlusCompat.PatchAll(harmony);
            }

            Log.LogInfo(NAME + " " + VERSION + " " + "is done patching.");

            // Delete the CustomEvent Config File Every time
            // This is because the config file will take over the .json file instructions.
            // A method without the need to use the config file would be better but it is suspected it may be some networking issue preventing this.
            // Will look into this in the future, but no definite date. Somehow it prevents failed RPC calls in multiplayer.
            try
            {
                System.IO.File.Delete(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CustomEvents.cfg");

                //Create and dispose
                System.IO.File.Create(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CustomEvents.cfg").Dispose();

            }
            catch (Exception e)
            {
                Log.LogWarning("Failed to delete custom event config file: " + e.Message);
            }

            NetcodePatcherAwake();
        }

        private void NetcodePatcherAwake()
        {
            try
            {
                var currentAssembly = Assembly.GetExecutingAssembly();
                var types = currentAssembly.GetTypes();

                foreach (var type in types)
                {
                    var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                    foreach (var method in methods)
                    {
                        try
                        {
                            // Safely attempt to retrieve custom attributes
                            var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);

                            if (attributes.Length > 0)
                            {
                                try
                                {
                                    // Safely attempt to invoke the method
                                    method.Invoke(null, null);
                                }
                                catch (TargetInvocationException ex)
                                {
                                    // Log and continue if method invocation fails (e.g., due to missing dependencies)
                                    Logger.LogWarning($"Failed to invoke method {method.Name}: {ex.Message}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle errors when fetching custom attributes, due to missing types or dependencies
                            Logger.LogWarning($"Error processing method {method.Name} in type {type.Name}: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Catch any general exceptions that occur in the process
                Logger.LogError($"An error occurred in NetcodePatcherAwake: {ex.Message}");
            }
        }
    }
}
