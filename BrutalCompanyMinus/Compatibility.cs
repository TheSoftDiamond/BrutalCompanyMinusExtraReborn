using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using BepInEx.Bootstrap;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;
using static BrutalCompanyMinus.Minus.EventManager;
using BrutalCompanyMinus.Minus.Events;
using BrutalCompanyMinus.Minus;
using BepInEx;
using Mono.Cecil.Cil;

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
            lcOfficePresent = false,
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
            // monsterPlushiesPresent = false,
            // blahajPlushPresent = false,
            zombiesPlushPresent = false,
            takeyGokuPresent = false,
            takeyGokuEssentialPresent = false,
            crowdControlPresent = false,
            officialExternalModulePresent = false,
            // hallucinoceptsPresent = false,
            facelessStalekerPresent = false,
            gokuBrackenPresent = false,
            moaiEnemyPresent = false,
            //yesFoxPresent = false,
            BaldiPresent = false,
            ShibaPresent = false,
            SurfacedPresent = false,
            VarietyPresent = false,
            CodeRebirthPresent = false,
            ShipInventoryPresent = false,
            SuperEclipsePresent = false,
            SkullEnemyPresent = false,
            ManStalkerPresent = false,
            FacilityMeltdownPresent = false;


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

       /*     Assembly hallucinocepsAssembly = GetAssembly("Hallucinoceps");
            if (hallucinocepsAssembly != null)
            {
                Log.LogInfo("Found CullFactory mod, Will attempt to grab OnTeleportOtherPlayerThroughEntrance() and OnTeleportLocalPlayerThroughEntrance() methodInfos");

                Type DeafMakerShroom = hallucinocepsAssembly.GetType("Hallucinoceps.DeafMakerShroom");
                if (DeafMakerShroom != null)
                {
                    DeafMakerShroom1 = DeafMakerShroom.GetMethod("DeafMakerShroom", BindingFlags.Static | BindingFlags.NonPublic);
                  //  cullOnTeleportOtherPlayer = teleporterExtender.GetMethod("OnTeleportOtherPlayerThroughEntrance", BindingFlags.Static | BindingFlags.NonPublic);

                    if (DeafMakerShroom1 != null)
                    {
                        Log.LogInfo("Found OnTeleportOtherPlayerThroughEntrance() and OnTeleportLocalPlayerThroughEntrance() methodInfos, passages are now compatible with cullfactory");
                        cullFactoryPresent = true;
                    }
                }
            }*/

            lethalEscapePresent = IsModPresent("xCeezy.LethalEscape", "Will prevent SafeOutside event from occuring.") || IsModPresent("AudioKnight.StarlancerEnemyEscape", "Will prevent SafeOutside event from occuring");

            lethalThingsPresent = IsModPresent("evaisa.lethalthings", "Roomba and TeleporterTraps event will now occur.", new Roomba(), new TeleporterTraps());
            diversityPresent = IsModPresent("Chaos.Diversity", "Walker event will now occur.", new Walkers());
            scopophobiaPresent = IsModPresent("Scopophobia", "Shy Guy and NoShyGuy event will now occur.", new ShyGuy(), new NoShyGuy());
            lcOfficePresent = IsModPresent("Piggy.LCOffice", "Shrimp event will now occur.", new Shrimp());
            herobrinePresent = IsModPresent("Kittenji.HerobrineMod", "Herobrine event will now occur.", new Herobrine());
            sirenheadPresent = IsModPresent("Ccode.SirenHead", "SirenHead event will now occur.", new SirenHead());
            rollinggiantPresent = IsModPresent("nomnomab.rollinggiant", "RollingGiant and NoRollingGiant event will now occur.", new RollingGiants());
            theFiendPresent = IsModPresent("com.RuthlessCompany", "TheFiend and NoFiend event will now occur.", new TheFiend(), new NoFiend());
            immortalSnailPresent = IsModPresent("ImmortalSnail", "ImmortalSnail and NoImmortalSnail event will now occur.", new ImmortalSnail(), new NoImmortalSnails());
            lockerPresent = IsModPresent("com.zealsprince.locker", "Locker and NoLocker event will now occur.", new Lockers(), new NoLockers());
            theGiantSpecimensPresent = IsModPresent("TheGiantSpecimens", "GiantShowdown event will now occur.", new GiantShowdown());
            footballPresent = IsModPresent("Kittenji.FootballEntity", "Football event will now occur.", new Football());
            toilheadPresent = IsModPresent("com.github.zehsteam.ToilHead", "Toilhead event will now occur", new ToilHead(), new Mantitoil(), new ToilSlayer(), new MantiToilSlayer(), new NoMantiToilSlayer(), new NoToilSlayer(), new NoMantitoil(), new AllSlayers(), new NoSlayers(), new TurretsEverywhere());
            emergencyDicePresent = IsModPresent("Theronguard.EmergencyDice", "BadDice and Dice events will now occur.", new BadDice(), new Dice());
            goldScrapPresent = IsModPresent("LCGoldScrapMod", "CityOfGold event will now occur", new CityOfGold());
            moonsweptPresent = IsModPresent("MoonsweptTeam.Moonswept", "Cleaner and MobileTurrets events will now occur", new Cleaners(), new MobileTurrets());
            shockwaveDronesPresent = IsModPresent("droneenemy", "Shockwave Drones event will now occur", new ShockwaveDrones());
            facelessStalekerPresent = IsModPresent("sparble.slendermanmod", "SlenderMan event will now occur", new SlenderMan());
           // takeyPlushPresent = IsModPresent("com.github.zehsteam.TakeyPlush", "Takey has joined the game", new TakeyPlush()/*, new TakeyGokuPlush(), new TakeyGokuPlushBig()*/);
            // zombiesPlushPresent = IsModPresent("com.github.zehsteam.ZombiesPlush", "Zombies Plush event activated", new ZombiesPlush());
            NonShippingAuthorisationPresent = IsModPresent("NonShippingAuthorisation", "Authorisation accepted"); 
            // blahajPlushPresent = IsModPresent("com.github.zehsteam.BlahajPlush", "BlahajPlush dettected", new BlahajPlush());
            // monsterPlushiesPresent = IsModPresent("scin.monsterplushies", "Monster Plushies events will now occur", new SpiderPlush());
            takeyGokuPresent = IsModPresent("com.github.zehsteam.TakeyGokuBracken", "Takey Goku dettected", new TakeyGokuBracken());
            //Vulf.GokuBracken hallucinoceptsPresent = IsModPresent("Hallucinoceps", "Damn u Minzy!, Integration not in working state, event will NEVER occur!", new MushroomEv());
            takeyGokuEssentialPresent = IsModPresent("Vulf.GokuBracken", "GokuBracken detected, proceeding with TakeyGoku check");
            officialExternalModulePresent = IsModPresent("BCME-ExternalModule", "ExternalModule dettected...");
            crowdControlPresent = IsModPresent("WarpWorld.CrowdControl", "Warning! CrowdControl dettected, shutting down incompatible events");
            moaiEnemyPresent = IsModPresent("MoaiEnemy", "Moai Enemies were detected", new MoaiEnemy());
            //yesFoxPresent = IsModPresent("uk.1a3.yesfox"), "YesFox detected", new KidnapperFox());
            BaldiPresent = IsModPresent("com.riskivr.BaldiEnemy", "Baldi Mod Detected", new Baldi());
            FacilityMeltdownPresent = IsModPresent("me.loaforc.facilitymeltdown", "Facility Meltdown Detected");
            ShibaPresent = IsModPresent("ReavsStuff.ShibaEnemy", "Shiba Bat Detected");
            SurfacedPresent = IsModPresent("Surfaced", "Surfaced Detected");
            VarietyPresent = IsModPresent("TestAccount666.TestAccountVariety", "Test Account Variety Detected");
            CodeRebirthPresent = IsModPresent("CodeRebirth", "CodeRebirth Detected");
            SuperEclipsePresent = IsModPresent("Millie.SuperEclipse", "Super Eclipse Detected");
            ManStalkerPresent = IsModPresent("menstalker_yaboiduckisnickerbar", "Man Stalker Detected");
            SkullEnemyPresent = IsModPresent("SkullEnemy", "Skull Enemy Detected");
            ShipInventoryPresent = IsModPresent("ShipInventory", "Ship Inventory Detected. Including in inventory checks");

        }

        private static Assembly GetAssembly(string name)
        {
            if(Chainloader.PluginInfos.ContainsKey(name))
            {
                return Chainloader.PluginInfos[name].Instance.GetType().Assembly;
            }
            return null;
        }

        private static bool IsModPresent(string name, string logMessage, params MEvent[] associatedEvents)
        {
            bool isPresent = Chainloader.PluginInfos.ContainsKey(name);
            if (isPresent)
            {
                moddedEvents.AddRange(associatedEvents);
                Log.LogInfo($"{name} is present. {logMessage}");
            }
            return isPresent;
        }
    }
}
