﻿using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using BepInEx;

namespace BrutalCompanyMinus
{
    [HarmonyPatch]
    public class Assets
    {
        internal static AssetBundle bundle, customAssetBundle;

        public enum EnemyName
        {
            Bracken, HoardingBug, CoilHead, Thumper, BunkerSpider, Jester, SnareFlea, Hygrodere, GhostGirl, SporeLizard, NutCracker, Masked, EyelessDog, ForestKeeper, EarthLeviathan, BaboonHawk, RoamingLocust, Manticoil, CircuitBee, Lasso, Butler, OldBird, FlowerSnake, Maneater, Barber, GiantKiwi
        }
        public static Dictionary<EnemyName, string> EnemyNameList = new Dictionary<EnemyName, string>() {
            { EnemyName.SnareFlea, "Centipede" }, { EnemyName.BunkerSpider, "SandSpider" }, { EnemyName.HoardingBug, "HoarderBug" }, { EnemyName.Bracken, "Flowerman" }, { EnemyName.Thumper, "Crawler" },
            { EnemyName.Hygrodere, "Blob" }, { EnemyName.GhostGirl, "DressGirl" }, { EnemyName.SporeLizard, "Puffer" }, { EnemyName.NutCracker, "Nutcracker" }, { EnemyName.EyelessDog, "MouthDog" },
            { EnemyName.ForestKeeper, "ForestGiant" }, { EnemyName.EarthLeviathan, "SandWorm" }, { EnemyName.CircuitBee, "RedLocustBees" }, { EnemyName.BaboonHawk, "BaboonHawk" }, { EnemyName.CoilHead, "SpringMan" },
            { EnemyName.Jester, "Jester" }, { EnemyName.Lasso, "LassoMan" }, { EnemyName.Masked, "MaskedPlayerEnemy" }, { EnemyName.Manticoil, "Doublewing" }, { EnemyName.RoamingLocust, "DocileLocustBees" },
            { EnemyName.Butler, "Butler" }, { EnemyName.OldBird, "RadMech" }, { EnemyName.FlowerSnake, "FlowerSnake" }, { EnemyName.Maneater, "CaveDweller" }, { EnemyName.Barber, "ClaySurgeon" }, {EnemyName.GiantKiwi, "GiantKiwi" }
        };

        public enum ItemName
        {
            LargeAxle, V_TypeEngine, PlasticFish, MetalSheet, LaserPointer, BigBolt, Bottles, Ring, SteeringWheel, CookieMoldPan, EggBeater, JarOfPickles, DustPan, AirHorn, ClownHorn, CashRegister, Candy, GoldBar, YieldSign, HomemadeFlashbang, Gift, Flask, ToyCube, Remote, ToyRobot, MagnifyingGlass, StopSign, TeaKettle, Mug, RedSoda, OldPhone, HairDryer, Brush, Bell, WhoopieCushion, Comedy, Tragedy, RubberDucky, ChemicalJug, FancyLamp, GoldenCup, Painting, Toothpaste, PillBottle, PerfumeBottle, Teeth, Magic7Ball, EasterEgg, ToyTrain, ToiletPaper, SoccerBall, PlasticCup, GarabgeLid, ControlPad, Clock, ZedDog, BabyKiwiEgg
        }
        public static Dictionary<ItemName, string> ItemNameList = new Dictionary<ItemName, string>() {
            { ItemName.LargeAxle, "Cog1" }, { ItemName.V_TypeEngine, "EnginePart1"}, { ItemName.PlasticFish, "FishTestProp" }, { ItemName.MetalSheet, "MetalSheet" }, { ItemName.LaserPointer, "FlashLaserPointer" },
            { ItemName.BigBolt, "BigBolt" }, { ItemName.Bottles, "BottleBin" }, { ItemName.Ring, "Ring" }, { ItemName.SteeringWheel, "SteeringWheel" }, { ItemName.CookieMoldPan, "MoldPan" },
            { ItemName.EggBeater, "EggBeater" }, { ItemName.JarOfPickles, "PickleJar" }, { ItemName.DustPan, "DustPan" }, { ItemName.AirHorn, "Airhorn" }, { ItemName.ClownHorn, "ClownHorn" },
            { ItemName.CashRegister, "CashRegister" }, { ItemName.Candy, "Candy" }, { ItemName.GoldBar, "GoldBar" }, { ItemName.YieldSign, "YieldSign" }, { ItemName.HomemadeFlashbang, "DiyFlashbang" },
            { ItemName.Gift, "GiftBox" }, { ItemName.Flask, "Flask" }, { ItemName.ToyCube, "ToyCube" }, { ItemName.Remote, "Remote" }, { ItemName.ToyRobot, "RobotToy" },
            { ItemName.MagnifyingGlass, "MagnifyingGlass" }, { ItemName.StopSign, "StopSign" }, { ItemName.TeaKettle, "TeaKettle" }, { ItemName.Mug, "Mug" }, { ItemName.RedSoda, "SodaCanRed" },
            { ItemName.OldPhone, "Phone" }, { ItemName.HairDryer, "Hairdryer" }, { ItemName.Brush, "Brush" }, { ItemName.Bell, "Bell" }, { ItemName.WhoopieCushion, "WhoopieCushion" },
            { ItemName.Comedy, "ComedyMask" }, { ItemName.Tragedy, "TragedyMask" }, { ItemName.RubberDucky, "RubberDuck" }, { ItemName.ChemicalJug, "ChemicalJug" }, { ItemName.FancyLamp, "FancyLamp" },
            { ItemName.Painting, "FancyPainting" }, { ItemName.GoldenCup, "FancyCup" }, { ItemName.Toothpaste, "Toothpaste" }, { ItemName.PillBottle, "PillBottle" }, { ItemName.PerfumeBottle, "PerfumeBottle" },
            { ItemName.Teeth, "Dentures" }, { ItemName.Magic7Ball, "7Ball" }, { ItemName.EasterEgg, "EasterEgg" }, { ItemName.ToyTrain, "ToyTrain" }, { ItemName.ToiletPaper, "ToiletPaperRolls" },
            { ItemName.SoccerBall, "SoccerBall" }, { ItemName.PlasticCup, "PlasticCup" }, { ItemName.GarabgeLid, "GarbageLid" }, { ItemName.ControlPad, "ControlPad" }, { ItemName.Clock, "Clock" },
            { ItemName.ZedDog, "ZedDog" }, { ItemName.BabyKiwiEgg, "BabyKiwiEgg" }
        };
        
        public enum ObjectName
        {
            LargeRock1, LargeRock2, LargeRock3, LargeRock4, TreeLeaflessBrown1, GiantPumkin, GreyRockGrouping2, GreyRockGrouping4, Tree, TreeLeafless2, TreeLeafless3, Landmine, Turret, SpikeRoofTrap
        }
        public static Dictionary<ObjectName, string> ObjectNameList = new Dictionary<ObjectName, string>() {
            { ObjectName.LargeRock1, "LargeRock1" }, { ObjectName.LargeRock2, "LargeRock2" }, { ObjectName.LargeRock3, "LargeRock3" }, { ObjectName.LargeRock4, "LargeRock4" }, { ObjectName.GreyRockGrouping2, "GreyRockGrouping2" },
            { ObjectName.GreyRockGrouping4, "GreyRockGrouping4" }, { ObjectName.GiantPumkin, "GiantPumpkin" }, { ObjectName.Tree, "tree" }, { ObjectName.TreeLeaflessBrown1, "treeLeaflessBrown.001 Variant" }, { ObjectName.TreeLeafless2, "treeLeafless.002_LOD0" },
            { ObjectName.TreeLeafless3, "treeLeafless.003_LOD0" }, { ObjectName.Landmine, "Landmine" }, { ObjectName.Turret, "TurretContainer" }, { ObjectName.SpikeRoofTrap, "SpikeRoofTrapHazard" }
        };

        public enum AtmosphereName
        {
            RollingGroundFog, Rainy, Stormy, Foggy, Flooded, Eclipsed
        }
        public static Dictionary<AtmosphereName, string> AtmosphereNameList = new Dictionary<AtmosphereName, string>()
        {
            { AtmosphereName.RollingGroundFog, "rolling ground fog" }, { AtmosphereName.Rainy, "rainy" }, { AtmosphereName.Stormy, "stormy" }, { AtmosphereName.Foggy, "foggy" }, { AtmosphereName.Flooded, "flooded" },
            { AtmosphereName.Eclipsed, "eclipsed" }
        };
        
        internal static GameObject hangarShip
        {
            get => GameObject.Find("/Environment/HangarShip");
        }

        internal static GameObject cruiser
        {
            get => GameObject.Find("CompanyCruiser(Clone)");
        }

        internal static GameObject Microwave
        {
            get => GameObject.Find("MicrowaveContainer(Clone)");
        }

        public static bool ReadSettingEarly(string filePath, string settingName)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    // Log the error
                    //Log.LogWarning($"Config file not found: {filePath}");
                    return false;
                }
                else
                {
                    // Log the success
                    //Log.LogInfo($"Config file found: {filePath}");
                }
                // Read the file content
                string fileContent = File.ReadAllText(filePath);

                // Match the setting name and its value
                string pattern = $@"{Regex.Escape(settingName)}\s*=\s*(true|false)";

                // Match the setting
                Match match = Regex.Match(fileContent, pattern, RegexOptions.IgnoreCase);

                // If the setting is found, return its value
                if (match.Success)
                {
                    // Get the value
                    string value = match.Groups[1].Value;
                    //Log.LogInfo($"Setting '{settingName}' found: {value}");
                    return bool.Parse(value);
                }
                else
                {
                    // Log the error
                    //Log.LogWarning($"Setting '{settingName}' not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Log the error
                //Log.LogWarning($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public static Dictionary<string, EnemyType> EnemyList = new Dictionary<string, EnemyType>();
        public static Dictionary<string, Item> ItemList = new Dictionary<string, Item>();
        public static Dictionary<string, GameObject> ObjectList = new Dictionary<string, GameObject>();

        internal static List<float> factorySizeMultiplierList = new List<float>();
        internal static List<SpawnableMapObject[]> spawnableMapObjects = new List<SpawnableMapObject[]>();
        internal static List<float> averageScrapValueList = new List<float>();
        internal static List<AnimationCurve> insideSpawnChanceCurves = new List<AnimationCurve>(), outsideSpawnChanceCurves = new List<AnimationCurve>(), daytimeSpawnChanceCurves = new List<AnimationCurve>();
        internal static List<int> insideMaxPowerCounts = new List<int>(), outsideMaxPowerCounts = new List<int>(), daytimeMaxPowerCounts = new List<int>();

        internal static StormyWeather stormy;
        internal static FloodWeather flooded;

        // Custom Assets
        public static EnemyType antiCoilHead, nutSlayer, kamikazieBug;
        public static Item slayerShotgun, grabbableTurret, grabbableLandmine;
        public static GameObject artilleryShell, artillerySirens, bunkerEntrance, bunkerEscape, teleportAudio, bloodRain;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameNetworkManager), "Start")]
        private static void GenerateCustom()
        {
            antiCoilHead = (EnemyType)customAssetBundle.LoadAsset("AntiCoilHead");
            nutSlayer = (EnemyType)customAssetBundle.LoadAsset("NutSlayer");
            kamikazieBug = (EnemyType)customAssetBundle.LoadAsset("KamikazieBug");

            slayerShotgun = (Item)customAssetBundle.LoadAsset("SlayerShotgun");
            grabbableTurret = (Item)customAssetBundle.LoadAsset("GrabbableTurret");
            grabbableLandmine = (Item)customAssetBundle.LoadAsset("GrabbableLandmine");

            artilleryShell = (GameObject)customAssetBundle.LoadAsset("ArtilleryShell");
            artillerySirens = (GameObject)customAssetBundle.LoadAsset("DDay");
            bunkerEntrance = (GameObject)customAssetBundle.LoadAsset("BunkerEntrance");
            bunkerEscape = (GameObject)customAssetBundle.LoadAsset("BunkerEscape");
            teleportAudio = (GameObject)customAssetBundle.LoadAsset("TeleportAudioSource");
            bloodRain = (GameObject)customAssetBundle.LoadAsset("BloodRainParticleContainer");

            RegisterNetworkPrefabs(antiCoilHead.enemyPrefab, nutSlayer.enemyPrefab, kamikazieBug.enemyPrefab, slayerShotgun.spawnPrefab, grabbableTurret.spawnPrefab, grabbableLandmine.spawnPrefab, artillerySirens, bunkerEntrance, bunkerEscape);
        }

        private static void RegisterNetworkPrefabs(params GameObject[] objects)
        {
            foreach(GameObject obj in objects)
            {
                NetworkManager.Singleton.AddNetworkPrefab(obj);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(TimeOfDay), "Start")]
        private static void OnTimeOfODayStart(ref TimeOfDay __instance)
        {
            GameObject bloodyRain = GameObject.Instantiate(bloodRain);
            LocalVolumetricFog fog = bloodyRain.transform.Find("Foggy").GetComponent<LocalVolumetricFog>();
            fog.parameters.albedo = new Color(0.25f, 0.35f, 0.55f, 1f);
            fog.parameters.meanFreePath = 80.0f;
            fog.parameters.size.y = 255f;

            __instance.effects = __instance.effects.Add(new WeatherEffect() { name = "bloodyrain", effectObject = bloodyRain, effectPermanentObject = null, lerpPosition = false, effectEnabled = false });
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), "Start")]
        private static void OnStartOfRoundStart() // Do this or items will disapear on save reload
        {
            StartOfRound.Instance.allItemsList.itemsList.AddRange(new List<Item>() { slayerShotgun, grabbableTurret, grabbableLandmine });
        }

        internal static void Load()
        {
            bundle = LoadAssetBundle("bcm_assets");
            customAssetBundle = LoadAssetBundle("bcm_customassets");

            //Ensure Configuration is loaded
            //Configuration.CreateConfig();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static AssetBundle LoadAssetBundle(string fileName)
        {
            try
            {
                string dllFolderPath = Path.GetDirectoryName(Plugin.Instance.Info.Location);
                string assetBundleFilePath = Path.Combine(dllFolderPath, fileName);
                return AssetBundle.LoadFromFile(assetBundleFilePath);
            }
            catch (Exception e)
            {
                Log.LogError($"Failed to load AssetBundle \"{fileName}\". {e}");
            }

            return null;
        }

        private static bool generatedList = false;
        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(StartOfRound.Instance == null) return;

            // This is placed here intentionally, if placed after generatedList it wont work after loading Scene again.
            // Get weather scripts
            StormyWeather[] stormyWeather = Resources.FindObjectsOfTypeAll<StormyWeather>().Concat(GameObject.FindObjectsByType<StormyWeather>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)).ToArray();
            FloodWeather[] floodWeather = Resources.FindObjectsOfTypeAll<FloodWeather>().Concat(GameObject.FindObjectsByType<FloodWeather>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)).ToArray();

            if (stormyWeather.Length != 0) stormy = stormyWeather[0];
            if (floodWeather.Length != 0) flooded = floodWeather[0];

            if (generatedList) return;

            // Generate Enemy List
            Log.LogInfo("Generating 'EnemyList'");

            EnemyType[] AllEnemies = Resources.FindObjectsOfTypeAll<EnemyType>().Concat(GameObject.FindObjectsByType<EnemyType>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)).ToArray();
            Array.Reverse(AllEnemies); // Reverse(Important)
            AllEnemies = AllEnemies.GroupBy(x => x.name).Select(x => x.FirstOrDefault()).ToArray(); // Remove duplicates

            for (int i = 0; i < AllEnemies.Length; i++)
            {
                if (ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Silence Prefab Warnings?") == false)
                {
                    if (AllEnemies[i].enemyPrefab == null) Log.LogWarning(string.Format("Enemy:{0}, prefab is null, this may cause issues...", AllEnemies[i].name));
                }
                EnemyList.Add(AllEnemies[i].name, AllEnemies[i]);
            }
            EnemyList.Remove("RedPillEnemyType"); // Useless

            // Check list
            foreach (KeyValuePair<string, EnemyType> e in EnemyList)
            {
                bool existsInList = false;
                foreach (KeyValuePair<EnemyName, string> n in EnemyNameList)
                {
                    if (e.Key == n.Value)
                    {
                        existsInList = true;
                        break;
                    }
                }
                if (ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Silence Enum Warnings?") == false)
                {
                    if (!existsInList) Log.LogWarning(string.Format("Enemy:'{0}', isn't matched with enum, this may cause issues...", e.Key));
                }
            }

            Log.LogInfo(string.Format("Finished generating 'EnemyList', Count:{0}", EnemyList.Count));

            // Generate Item List
            Log.LogInfo("Generating 'ItemList'");

            Item[] AllItems = Resources.FindObjectsOfTypeAll<Item>().Concat(GameObject.FindObjectsByType<Item>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)).ToArray();
            Array.Reverse(AllItems); // Reverse(Important)
            AllItems = AllItems.GroupBy(x => x.name).Select(x => x.FirstOrDefault()).ToArray(); // Remove duplicates

            for (int i = 0; i < AllItems.Length; i++)
            {
                if (ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Silence Prefab Warnings?") == false)
                {
                    if (AllItems[i].spawnPrefab == null) Log.LogWarning(string.Format("Item:{0}, prefab is null, this may cause issues...", AllItems[i].name));
                }
                ItemList.Add(AllItems[i].name, AllItems[i]);
            }

            // Check list
            foreach (KeyValuePair<string, Item> i in ItemList)
            {
                bool existsInList = false;
                foreach (KeyValuePair<ItemName, string> n in ItemNameList)
                {
                    if (i.Key == n.Value)
                    {
                        existsInList = true;
                        break;
                    }
                }
                if (ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Silence Enum Warnings?") == false)
                {
                    if (!existsInList) Log.LogWarning(string.Format("Item:'{0}', isn't matched with enum, this may cause issues...", i.Key));
                }
            }

            Log.LogInfo(string.Format("Finished generating 'ItemList', Count:{0}", ItemList.Count));



            // Generate Object List
            Log.LogInfo("Generating 'ObjectList'");

            List<SpawnableMapObject> insideObjectList = new List<SpawnableMapObject>();
            List<SpawnableOutsideObjectWithRarity> outsideObjectList = new List<SpawnableOutsideObjectWithRarity>();

            foreach (SelectableLevel level in StartOfRound.Instance.levels)
            {
                if (level == null || level.spawnableMapObjects == null) continue;
                foreach (SpawnableMapObject obj in level.spawnableMapObjects)
                {
                    if(obj == null || obj.prefabToSpawn == null) continue;
                    if (insideObjectList.FindIndex(o => o.prefabToSpawn.name == obj.prefabToSpawn.name) < 0) // If dosent exist in list then add
                    {
                        insideObjectList.Add(obj);
                    }
                }

                foreach (SpawnableOutsideObjectWithRarity obj in level.spawnableOutsideObjects)
                {
                    if(obj == null || obj.spawnableObject == null || obj.spawnableObject.prefabToSpawn == null) continue;
                    if (outsideObjectList.FindIndex(o => o.spawnableObject.prefabToSpawn.name == obj.spawnableObject.prefabToSpawn.name) < 0) // If dosent exist in list then add
                    {
                        outsideObjectList.Add(obj);
                    }
                }
            }

            foreach (SpawnableMapObject obj in insideObjectList) ObjectList.Add(obj.prefabToSpawn.name, obj.prefabToSpawn);
            foreach (SpawnableOutsideObjectWithRarity obj in outsideObjectList) ObjectList.Add(obj.spawnableObject.prefabToSpawn.name, obj.spawnableObject.prefabToSpawn);

            // Check list
            foreach (KeyValuePair<string, GameObject> o in ObjectList)
            {
                bool existsInList = false;
                foreach (KeyValuePair<ObjectName, string> n in ObjectNameList)
                {
                    if (o.Key == n.Value)
                    {
                        existsInList = true;
                        break;
                    }
                }
                if (ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Silence Enum Warnings?") == false)
                {
                    if (!existsInList) Log.LogWarning(string.Format("Object:'{0}', isn't matched with enum, this may cause issues...", o.Key));
                }
            }

            Log.LogInfo(string.Format("Finished generating 'ObjectList', Count:{0}", ObjectList.Count));

            Log.LogInfo(string.Format("Map Count:{0}", factorySizeMultiplierList.Count));

            Log.LogInfo("Generating configuration");
            Configuration.CreateConfig();

            generatedList = true;
        }

        private static bool generatedOrignalValuesList = false;
        internal static void generateOriginalValuesLists()
        {
            if (generatedOrignalValuesList) return;
            
            // Generate FactorySize List and animation curves list
            foreach (SelectableLevel level in StartOfRound.Instance.levels)
            {
                // Factory size list and average scrap value
                factorySizeMultiplierList.Add(level.factorySizeMultiplier);
                List<SpawnableItemWithRarity> items = new List<SpawnableItemWithRarity>();
                items.AddRange(level.spawnableScrap);

                float scrapValueSum = 0.0f;
                float scrapWeightSum = 0.0f;
                foreach (SpawnableItemWithRarity item in items)
                {
                    scrapValueSum += (item.spawnableItem.minValue + item.spawnableItem.maxValue) * item.rarity;
                    scrapWeightSum += item.rarity;
                }
                if (scrapWeightSum != 0.0f)
                {
                    averageScrapValueList.Add(scrapValueSum / (scrapWeightSum * 2.0f));
                }
                else
                {
                    averageScrapValueList.Add(80);
                }

                if (ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Experimental Dont Handle Spawn Chance?") == false)
                {
                    // Animation curves list
                    AnimationCurve newInsideSpawnChanceCurve = new AnimationCurve(), newOutsideSpawnChanceCurve = new AnimationCurve(), newDaytimeSpawnChanceCurve = new AnimationCurve();

                    foreach (Keyframe key in level.enemySpawnChanceThroughoutDay.keys) newInsideSpawnChanceCurve.AddKey(key);
                    foreach (Keyframe key in level.outsideEnemySpawnChanceThroughDay.keys) newOutsideSpawnChanceCurve.AddKey(key);
                    foreach (Keyframe key in level.daytimeEnemySpawnChanceThroughDay.keys) newDaytimeSpawnChanceCurve.AddKey(key);

                    insideSpawnChanceCurves.Add(newInsideSpawnChanceCurve);
                    outsideSpawnChanceCurves.Add(newOutsideSpawnChanceCurve);
                    daytimeSpawnChanceCurves.Add(newDaytimeSpawnChanceCurve);
                }

                // enemyPowerCounts
                insideMaxPowerCounts.Add(level.maxEnemyPowerCount);
                outsideMaxPowerCounts.Add(level.maxOutsideEnemyPowerCount);
                daytimeMaxPowerCounts.Add(level.maxDaytimeEnemyPowerCount);

                spawnableMapObjects.Add(level.spawnableMapObjects);

            }
            generatedOrignalValuesList = true;
        }

        public static EnemyType GetEnemy(EnemyName name) => GetEnemy(EnemyNameList[name]);
        public static EnemyType GetEnemy(string name)
        {
            if (EnemyList.TryGetValue(name, out EnemyType enemyType)) return enemyType;

            if (ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Silence Get Method Warnings?") == false)
            {
                Log.LogWarning($"GetEnemy({name}) failed, returning an empty enemy type");
            }
            EnemyType empty = ScriptableObject.CreateInstance<EnemyType>();
            empty.enemyName = name;
            empty.name = name;
            return empty;
        }

        public static EnemyType GetEnemyOrDefault(string name)
        {
            if (EnemyList.TryGetValue(name, out EnemyType enemyType)) return enemyType;
            if (ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Silence Get Method Warnings?") == false)
            {
                Log.LogWarning($"GetEnemyOrDefault({name}) failed, returning kamikazie bug.");
            }
            return kamikazieBug;
        }

        public static Item GetItem(ItemName name) => GetItem(ItemNameList[name]);
        public static Item GetItem(string name)
        {
            if(ItemList.TryGetValue(name, out Item item)) return item;
            if (ReadSettingEarly(Paths.ConfigPath + "\\BrutalCompanyMinusExtraReborn\\CoreProperties.cfg", "Silence Get Method Warnings?") == false)
            {
                Log.LogWarning($"GetItem({name}) failed, returning an empty item");
            }
            Item empty = ScriptableObject.CreateInstance<Item>();
            empty.itemName = name;
            empty.name = name;
            return empty;
        }


        /// <summary>
        /// Improved method to spawn vanilla items, by Zehs
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="matchCase"></param>
        /// <returns></returns>
        [Obsolete("Causes the mod to fail, do not use on modded item events", false)]
        public static Item GetItemByName(string itemName, bool matchCase = true)
        {
            System.StringComparison comparisonType = matchCase ? System.StringComparison.CurrentCulture : System.StringComparison.OrdinalIgnoreCase;

            foreach (var item in StartOfRound.Instance.allItemsList.itemsList)
            {
                if (item.itemName.Equals(itemName, comparisonType))
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Moddified method to spawn modded items, by Zehs
        /// </summary>
        /// <param name="itemName">Name of the item</param>
        /// <param name="matchCase">Case sensitive?</param>
        /// <param name="isModPresent">Is mod present?</param>
        /// <returns></returns>
        [Obsolete("Causes the mod to fail, do not use on vanilla item events", true)]
        public static Item GetItemByNameModded(string itemName, bool isModPresent, bool matchCase = true)
        {
            if (isModPresent == true)
            {
                System.StringComparison comparisonType = matchCase ? System.StringComparison.CurrentCulture : System.StringComparison.OrdinalIgnoreCase;

                foreach (var item in StartOfRound.Instance.allItemsList.itemsList)
                {
                    if (item.itemName.Equals(itemName, comparisonType))
                    {
                        return item;
                    }
                }
            }
            else
            {
                return null;
            }
            return null;
        }

        public static GameObject GetObject(ObjectName name) => GetObject(ObjectNameList[name]);
        public static GameObject GetObject(string name)
        {
            if(ObjectList.TryGetValue(name, out GameObject obj)) return obj;
            Log.LogWarning($"GetObject({name} failed, returning empty gameObject");
            return new GameObject(name);
        }
    }
}