using BepInEx.Bootstrap;
using BrutalCompanyMinus.Minus.Events;
using HarmonyLib;
using System;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;
using static BrutalCompanyMinus.Minus.EventManager;

namespace BrutalCompanyMinus
{
    [HarmonyPatch]
    internal class Compatibility
    {
        internal static bool yippeeModCompatibilityMode = false;
        internal static AudioClip[] yippeeNewSFX = null;

        internal static bool
            lethalEscapePresent = false,
            lethalThingsPresent = false,
            diversityPresent = false,
            scopophobiaPresent = false,
            shrimpPresent = false,
            herobrinePresent = false,
            peepersPresent = false,
            sirenheadPresent = false,
            rollinggiantPresent = false,
            theFiendPresent = false,
            immortalSnailPresent = false,
            takeyPlushPresent = false,
            lockerPresent = false,
            theGiantSpecimensPresent = false,
            mimicsPresent = false,
            footballPresent = false,
            emergencyDicePresent = false,
            toilheadPresent = false,
            goldScrapPresent = false,
            cullFactoryPresent = false,
            NonShippingAuthorisationPresent = false,
            moonsweptPresent = false,
            shockwaveDronesPresent = false,
            zombiesPlushPresent = false,
            takeyGokuPresent = false,
            takeyGokuEssentialPresent = false,
            crowdControlPresent = false,
            officialExternalModulePresent = false,
            facelessStalekerPresent = false,
            gokuBrackenPresent = false,
            moaiEnemyPresent = false,
            BaldiPresent = false,
            ShibaPresent = false,
            SurfacedPresent = false,
            VarietyPresent = false,
            CodeRebirthPresent = false,
            ShipInventoryPresent = false,
            SelfSortingStorage = false,
            SuperEclipsePresent = false,
            SkullEnemyPresent = false,
            ManStalkerPresent = false,
            FoxyPresent = false,
            PlaytimePresent = false,
            CrittersPresent = false,
            FacilityMeltdownPresent = false,
            NeedyCatsPresent = false,
            furniturePackPresent = false,
            yesFoxPresent = false,
            soulDevPresent = false,
            lighteaterPresent = false,
            BiodiversityPresent = false,
            OoblterraPresent = false,
            LethalPhonesPresent = false,
            HotBarPlusPresent = false,
            KidnapperFoxPresent = false,
            LethalElementsPresent = false;



        internal static FieldInfo peeperSpawnChance = null;
        internal static NetworkVariable<int>[] mimicNetworkSpawnChances = null;

        internal static MethodInfo cullOnTeleportLocalPlayer = null;
        internal static MethodInfo cullOnTeleportOtherPlayer = null;

        internal static MethodInfo DeafMakerShroom1 = null;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PreInitSceneScript), "Awake")]
        private static void OnGameLoad()
        {
            Assembly yippeeAssembly = GetAssembly("sunnobunno.YippeeMod");
            if (yippeeAssembly != null)
            {
                Log.LogInfo("Found YippeeMod, Will attempt to replace kamikazie bug SFX");

                Type type = yippeeAssembly.GetType("YippeeMod.YippeeModBase");
                if (type != null)
                {
                    FieldInfo localField = type.GetField("newSFX", BindingFlags.Static | BindingFlags.NonPublic);
                    if (localField != null)
                    {
                        yippeeNewSFX = (AudioClip[])localField.GetValue(null);
                        if (yippeeNewSFX != null)
                        {
                            Log.LogInfo("YippeeMod compatibility succeeded.");
                            yippeeModCompatibilityMode = true;
                        }
                    }
                }
            }

            Assembly peepersAssembly = GetAssembly("x753.Peepers");
            if (peepersAssembly != null)
            {
                Log.LogInfo("Found PeepersMod, Will attempt to get spawnChance field.");

                Type type = peepersAssembly.GetType("LCPeeper.Peeper");
                if (type != null)
                {
                    peeperSpawnChance = type.GetField("PeeperSpawnChance", BindingFlags.Static | BindingFlags.Public);
                    if (peeperSpawnChance != null)
                    {
                        Log.LogInfo("Found spawnChance Field, Peepers and NoPeepers event's will now occur");
                        NoPeepers.oldSpawnChance = (float)peeperSpawnChance.GetValue(null);
                        peepersPresent = true;
                        moddedEvents.Add(new Peepers());
                        moddedEvents.Add(new NoPeepers());
                    }
                }
            }

            Assembly mimicsAssembly = GetAssembly("x753.Mimics");
            if (mimicsAssembly != null)
            {
                Log.LogInfo("Found mimicsMod, Will attempt to grab spawn rate network variables");

                Type mimicNetworker = mimicsAssembly.GetType("Mimics.MimicNetworker");
                Type mimic = mimicsAssembly.GetType("Mimics.Mimics");
                if (mimicNetworker != null && mimic != null)
                {
                    mimicNetworkSpawnChances = new NetworkVariable<int>[6];

                    for (int i = 0; i < 5; i++)
                    {
                        mimicNetworkSpawnChances[i] = (NetworkVariable<int>)mimicNetworker.GetField("SpawnWeight" + i, BindingFlags.Static | BindingFlags.Public).GetValue(null);
                    }
                    mimicNetworkSpawnChances[5] = (NetworkVariable<int>)mimicNetworker.GetField("SpawnWeightMax", BindingFlags.Static | BindingFlags.Public).GetValue(null);

                    bool isNull = false;
                    foreach (NetworkVariable<int> variable in mimicNetworkSpawnChances)
                    {
                        if (variable == null)
                        {
                            isNull = true;
                            break;
                        }
                    }
                    FieldInfo spawnRates = mimic.GetField("SpawnRates", BindingFlags.Static | BindingFlags.Public);

                    if (spawnRates != null && !isNull)
                    {
                        Log.LogInfo("Found spawn rate network variables, Mimics and noMimics events will now appear.");
                        Minus.Handlers.Mimics.originalSpawnRateValues = (int[])spawnRates.GetValue(null);
                        mimicsPresent = true;
                        moddedEvents.Add(new Mimics());
                        moddedEvents.Add(new NoMimics());
                    }
                }
            }

            Assembly cullFactoryAssembly = GetAssembly("com.fumiko.CullFactory");
            if(cullFactoryAssembly != null )
            {
                Log.LogInfo("Found CullFactory mod, Will attempt to grab OnTeleportOtherPlayerThroughEntrance() and OnTeleportLocalPlayerThroughEntrance() methodInfos");

                Type teleporterExtender = cullFactoryAssembly.GetType("CullFactory.Extenders.TeleportExtender");
                if(teleporterExtender != null)
                {
                    cullOnTeleportLocalPlayer = teleporterExtender.GetMethod("OnTeleportLocalPlayerThroughEntrance", BindingFlags.Static | BindingFlags.NonPublic);
                    cullOnTeleportOtherPlayer = teleporterExtender.GetMethod("OnTeleportOtherPlayerThroughEntrance", BindingFlags.Static | BindingFlags.NonPublic);

                    if(cullOnTeleportLocalPlayer != null && cullOnTeleportOtherPlayer != null)
                    {
                        Log.LogInfo("Found OnTeleportOtherPlayerThroughEntrance() and OnTeleportLocalPlayerThroughEntrance() methodInfos, passages are now compatible with cullfactory");
                        cullFactoryPresent = true;
                    }
                }
            }

            lethalEscapePresent = IsModPresent("xCeezy.LethalEscape", "Will prevent SafeOutside event from occuring.") || IsModPresent("AudioKnight.StarlancerEnemyEscape", "Will prevent SafeOutside event from occuring");
            lethalThingsPresent = IsModPresent("evaisa.lethalthings", "Roomba and TeleporterTraps event will now occur.");
            diversityPresent = IsModPresent("Chaos.Diversity", "Walker event will now occur.");
            scopophobiaPresent = IsModPresent("Scopophobia", "Shy Guy and NoShyGuy event will now occur.");
            shrimpPresent = IsModPresent("Piggy.Shrimp", "Shrimp event will now occur.");
            herobrinePresent = IsModPresent("Kittenji.HerobrineMod", "Herobrine event will now occur.");
            sirenheadPresent = IsModPresent("Ccode.SirenHead", "SirenHead event will now occur.");
            rollinggiantPresent = IsModPresent("nomnomab.rollinggiant", "RollingGiant and NoRollingGiant event will now occur.");
            theFiendPresent = IsModPresent("com.RuthlessCompany", "TheFiend and NoFiend event will now occur.");
            immortalSnailPresent = IsModPresent("ImmortalSnail", "ImmortalSnail and NoImmortalSnail event will now occur.");
            lockerPresent = IsModPresent("com.zealsprince.locker", "Locker and NoLocker event will now occur.");
            theGiantSpecimensPresent = IsModPresent("TheGiantSpecimens", "GiantShowdown event will now occur.");
            footballPresent = IsModPresent("Kittenji.FootballEntity", "Football event will now occur.");
            toilheadPresent = IsModPresent("com.github.zehsteam.ToilHead", "Toilhead event will now occur");
            emergencyDicePresent = IsModPresent("Theronguard.EmergencyDice", "BadDice and Dice events will now occur.");
            goldScrapPresent = IsModPresent("LCGoldScrapMod", "CityOfGold event will now occur");
            moonsweptPresent = IsModPresent("MoonsweptTeam.Moonswept", "Cleaner and MobileTurrets events will now occur");
            shockwaveDronesPresent = IsModPresent("droneenemy", "Shockwave Drones event will now occur");
            facelessStalekerPresent = IsModPresent("sparble.slendermanmod", "SlenderMan event will now occur");
            NonShippingAuthorisationPresent = IsModPresent("NonShippingAuthorisation", "Authorisation accepted"); 
            takeyGokuPresent = IsModPresent("com.github.zehsteam.TakeyGokuBracken", "Takey Goku dettected");
            takeyGokuEssentialPresent = IsModPresent("Vulf.GokuBracken", "GokuBracken detected, proceeding with TakeyGoku check");
            officialExternalModulePresent = IsModPresent("BCME-ExternalModule", "ExternalModule dettected...");
            crowdControlPresent = IsModPresent("WarpWorld.CrowdControl", "Warning! CrowdControl dettected, shutting down incompatible events");
            moaiEnemyPresent = IsModPresent("MoaiEnemy", "Moai Enemies were detected");
            BaldiPresent = IsModPresent("com.riskivr.BaldiEnemy", "Baldi Mod Detected");
            FacilityMeltdownPresent = IsModPresent("me.loaforc.facilitymeltdown", "Facility Meltdown Detected");
            ShibaPresent = IsModPresent("ReavsStuff.ShibaEnemy", "Shiba Bat Detected");
            SurfacedPresent = IsModPresent("Surfaced", "Surfaced Detected");
            VarietyPresent = IsModPresent("TestAccount666.TestAccountVariety", "Test Account Variety Detected");
            CodeRebirthPresent = IsModPresent("CodeRebirth", "CodeRebirth Detected");
            SuperEclipsePresent = IsModPresent("Millie.SuperEclipse", "Super Eclipse Detected");
            ManStalkerPresent = IsModPresent("menstalker_yaboiduckisnickerbar", "Man Stalker Detected");
            SkullEnemyPresent = IsModPresent("SkullEnemy", "Skull Enemy Detected");
            FoxyPresent = IsModPresent("NightmareFoxy", "NightmareFoxy Detected");
            PlaytimePresent = IsModPresent("Nuclear_Fox.LethalPlaytime", "Lethal Playtime Detected");
            CrittersPresent = IsModPresent("Nuclear_Fox.SmilingCritters", "Critters Present");
            NeedyCatsPresent = IsModPresent("Jordo.NeedyCats", "Needy Cats Detected");
            ShipInventoryPresent = IsModPresent("ShipInventory", "Ship Inventory Detected. Including in inventory checks");
            HotBarPlusPresent = IsModPresent("FlipMods.HotbarPlus", "HotBarPlus Detected.");
            yesFoxPresent = IsModPresent("uk.1a3.yesfox", "Yes Fox Detected");
            soulDevPresent = IsModPresent("SoulDev", "Soul Dev Detected");
            BiodiversityPresent = IsModPresent("com.github.biodiversitylc.Biodiversity", "Biodiversity Detected");
            furniturePackPresent = IsModPresent("MelanieMelicious.furniturePack0", "Furniture Pack Detected");
            OoblterraPresent = IsModPresent("SkullCrusher.WTO", "Ooblterra Detected");
            lighteaterPresent = IsModPresent("Lega.LightEater", "Light Eater Detected");
            LethalPhonesPresent = IsModPresent("LethalPhones", "Lethal Phones Detected");
            SelfSortingStorage = IsModPresent("zigzag.SelfSortingStorage", "Self Sorting Storage Detected");
            KidnapperFoxPresent = IsModPresent("uk.1a3.yesfox", "Kidnapper Fox Detected");
            LethalElementsPresent = IsModPresent("voxx.LethalElementsPlugin", "Lethal Elements Detected");
        }

        private static Assembly GetAssembly(string name)
        {
            if(Chainloader.PluginInfos.ContainsKey(name))
            {
                return Chainloader.PluginInfos[name].Instance.GetType().Assembly;
            }
            return null;
        }

        public static bool IsModPresent(string name, string logMessage = "")
        {
            bool isPresent = Chainloader.PluginInfos.ContainsKey(name);
            if (isPresent)
            {
                Log.LogInfo($"{name} is present. {logMessage}");
            }
            return isPresent;
        }
    }
}
