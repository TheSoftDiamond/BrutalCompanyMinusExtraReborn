using BepInEx;
using BepInEx.Configuration;
using BrutalCompanyMinus.Minus.Handlers;
using Discord;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace BrutalCompanyMinus.Minus
{
    [HarmonyPatch]
    public class EventManager
    {

        #region VanillaEvents

        internal static List<MEvent> vanillaEvents = new List<MEvent>() {
            //Rare
            new Events.SID(), // Requires Beta Events
            // Very Good
            new Events.BigBonus(),
            new Events.ScrapGalore(),
            new Events.GoldenBars(),
            new Events.BigDelivery(),
            new Events.PlentyOutsideScrap(),
            new Events.BlackFriday(),
            new Events.SafeOutside(),
            new Events.VeryEarlyShip(),
            new Events.TrapsFailure(),
            // Good
            new Events.Bounty(),
            new Events.Bonus(),
            new Events.SmallerMap(),
            new Events.MoreScrap(),
            new Events.HigherScrapValue(),
            new Events.GoldenFacility(),
            new Events.Dentures(),
            new Events.Pickles(),
            new Events.Honk(),
            new Events.TransmuteScrapSmall(),
            new Events.SmallDelivery(),
            new Events.ScarceOutsideScrap(),
            new Events.FragileEnemies(),
            new Events.FullAccess(),
            new Events.EarlyShip(),
            new Events.MoreExits(),
            new Events.DoorOverdriveEv(),
            new Events.ZedDog(),
            //new Events.AllOneHanded(),
            new Events.NotMetal(),
            // Neutral
            new Events.Nothing(),
            new Events.MetalSwitch(),
            //new Events.HandsSwitch(),
            new Events.Locusts(),
            new Events.Birds(),
            new Events.Trees(),
            new Events.LeaflessBrownTrees(),
            new Events.LeaflessTrees(),
            new Events.Raining(),
            new Events.Gloomy(),
            new Events.HeavyRain(),
            new Events.MaskItem(),
            new Events.EasterEggs(),
            new Events.Clock(),
            new Events.SussyPaintings(),
            new Events.Train(),
            new Events.ControlPad(),
            new Events.PlasticCup(),
            new Events.ToiletPaper(),
            new Events.FootballScrap(),
            new Events.GarbageLid(),
            new Events.SeveredBits(),
            // Bad
            new Events.HoardingBugs(),
            new Events.Dustpans(),
            new Events.Bees(),
            new Events.ShipLightsFailure(),
            new Events.Landmines(),
            new Events.Lizard(),
            new Events.Slimes(),
            new Events.Thumpers(),
            new Events.Turrets(),
            new Events.Spiders(),
            new Events.SnareFleas(),
            new Events.FacilityGhost(),
            new Events.OutsideTurrets(),
            new Events.OutsideLandmines(),
            new Events.ShipmentFees(),
            new Events.GrabbableLandmines(),
            new Events.GrabbableTurrets(),
            new Events.StrongEnemies(),
            new Events.KamikazieBugs(),
            new Events.RealityShift(),
            new Events.Masked(),
            new Events.Butlers(),
            new Events.SpikeTraps(),
            new Events.FlowerSnake(),
            new Events.LateShip(),
            new Events.HolidaySeason(),
            new Events.TurretsEverywhere(),
            new Events.ItemChargerFailure(),
            new Events.CruiserFailure(),
            new Events.TeleporterFailure(),
            new Events.TerminalFailure(),
            new Events.DoorFailure(),
            new Events.WalkieFailure(),
            new Events.ManualCameraFailure(),
            new Events.LeverFailure(),
            new Events.WelcomeToTheFactory(),
            new Events.FlashLightsFailure(),
            new Events.JetpackFailure(),
            new Events.DoorCircuitFailure(),
            new Events.Meteors(),
            new Events.AntiBounty(),
            new Events.TargetingFailureEvent(),
            new Events.TeleporterTraps(),
            new Events.IsMetal(),
            new Events.Stingray(), // Requires Beta Events (Temporary)
            new Events.Puma(), // Requires Beta Events (Temporary)
            new Events.KidnapperFox(),
            // Very Bad
            new Events.Cadaver(),
            new Events.CatsAndDogs(), // Requires Beta Events (Temporary)
            //new Events.AllTwoHanded(),
            //new Events.UncertainHands(),
            new Events.Nutcracker(),
            new Events.KiwiBird(),// Requires Special Events
            new Events.LockedEntrance(),// Requires Special Events
            new Events.Arachnophobia(),
            new Events.Bracken(),
            new Events.Coilhead(),
            new Events.BaboonHorde(),
            new Events.Dogs(),
            new Events.VeryLateShip(),
            new Events.TeleportIn(),
            new Events.GiantsOutside(),
            new Events.Jester(),
            new Events.LittleGirl(),
            new Events.AntiCoilhead(),
            new Events.BadProduce(),
            new Events.TransmuteScrapBig(),
            new Events.Warzone(),
            new Events.BugHorde(),
            new Events.ForestGiant(),
            new Events.InsideBees(),
            new Events.NutSlayer(),
            new Events.AllWeather(),
            new Events.Worms(),
            new Events.OldBirds(),
            new Events.ShipCoreFailure(),
            new Events.Dweller(),
            new Events.BerserkTurrets(),
            new Events.ExplodingItems(),
            //Insane
            new Events.Hell(),
            new Events.TimeChaos(),// Requires Special Events
            new Events.NutSlayersMore(),
            new Events.SlimeInside(),
            // No Enemy
            new Events.NoBaboons(),
            new Events.NoBracken(),
            new Events.NoCoilhead(),
            new Events.NoDogs(),
            new Events.NoGiants(),
            new Events.NoHoardingBugs(),
            new Events.NoJester(),
            new Events.NoGhosts(),
            new Events.NoLizards(),
            new Events.NoNutcracker(),
            new Events.NoSpiders(),
            new Events.NoThumpers(),
            new Events.NoSnareFleas(),
            new Events.NoWorm(),
            new Events.NoSlimes(),
            new Events.NoMasks(),
            new Events.NoTurrets(),
            new Events.NoLandmines(),
            new Events.NoOldBird(),
            new Events.NoButlers(),
            new Events.NoBirds(),
            new Events.NoSpikeTraps(),

        };

        #endregion

        #region ModdedEvents

        // Code revision => config entries fix...
        /// <summary>
        /// Registry for modded events
        /// </summary>
        public static List<MEvent> moddedEvents = new List<MEvent>()
        {
            //Rare

            //Very Good
            new Events.CityOfGold(),

            //Good
            new Events.Dice(),
            new Events.Nemo(),
            new Events.HotBarMania(),

            // Neutral
            new Events.NeedyCats(),

            //Bad
            new Events.Lockers(),
            new Events.Hallowed(),
            new Events.TakeyGokuBracken(),
            new Events.Cleaners(),
            new Events.ImmortalSnail(),
            new Events.Peepers(),
            new Events.RollingGiants(),
            new Events.Roomba(),
            new Events.ShockwaveDrones(),
            new Events.Shrimp(),
            new Events.Football(),
            new Events.Baldi(),
            new Events.Shiba(),
            new Events.Seamine(),
            new Events.YeetBomb(),
            new Events.ManStalker(),
            new Events.Foxy(),
            new Events.HotBarHassle(),
            new Events.Bellcrab(),
            new Events.LightEaterEnemy(),
            new Events.LeafBoys(),
            new Events.WelcomeToOoblterra(),
            new Events.PhonesOut(),
            new Events.Barbers(),
            new Events.SCP939(),
            new Events.SCP682(),
            //new Events.Mimics(),
            new Events.SolarFlare(),
            new Events.Heatwave(),
            new Events.Windy(),
            new Events.MeteorShower(),
            new Events.Forsaken(),
            new Events.MobileTurrets(),

            //Very Bad
            new Events.Mantitoil(),
            new Events.ToilSlayer(),
            new Events.MantiToilSlayer(),
            new Events.AllSlayers(),
            new Events.BadDice(),
            new Events.Herobrine(),
            new Events.ShyGuy(),
            new Events.SirenHead(),
            new Events.SlenderMan(),
            new Events.TheFiend(),
            new Events.ToilHead(),
            new Events.GiantShowdown(),
            new Events.MoaiEnemy(),
            new Events.Meltdown(),
            new Events.SkullEnemy(),
            new Events.Critters(),
            new Events.PlaytimeBig(),
            new Events.ItsPlaytime(),
            new Events.Walkers(),
            new Events.SoulDev(),
            new Events.MajoraMoon(),
            new Events.BloodMoon(),
            new Events.Hurricane(),
            new Events.Bertha(),
           
            //Insane

            //No Enemy
            new Events.NoMantitoil(),
            new Events.NoToilSlayer(),
            new Events.NoMantiToilSlayer(),
            new Events.NoSlayers(),
            new Events.NoFiend(),
            new Events.NoImmortalSnails(),
            new Events.NoLockers(),
            new Events.NoShyGuy(),
            //new Events.NoMimics(),
            new Events.NoPeepers()

        };
        #endregion

        #region Other Lists

        //public static List<MEvent> ExternalEvents = new List<MEvent>() { };

        internal static List<MEvent> customEvents = new List<MEvent>();

        internal static List<MEvent> events = new List<MEvent>();

        internal static List<MEvent> disabledEvents = new List<MEvent>();

        internal static List<MEvent> currentEvents = new List<MEvent>();

        internal static List<MEvent> forcedEvents = new List<MEvent>();

        internal static List<MEvent> allVeryGood = new List<MEvent>(), allGood = new List<MEvent>(), allNeutral = new List<MEvent>(), allBad = new List<MEvent>(), allVeryBad = new List<MEvent>(), allRemove = new List<MEvent>(), allInsane = new List<MEvent>(), allRare = new List<MEvent>(), allSpecial = new List<MEvent>(), allBeta = new List<MEvent>();

        internal static List<MEvent> sideEvents = new List<MEvent>();

        private static List<MEvent> tipEventsToDo = new List<MEvent>();

        //internal static List<CustomEvents> customEventsList = new List<CustomEvents>();

        internal static List<string> currentEventDescriptions = new List<string>();

        internal static float eventTypeSum = 0;
        internal static float[] eventTypeCount = new float[] { };

        internal static float[] eventTypeRarities = new float[] { };

        public static List<IndoorMapHazard> hazards = new List<IndoorMapHazard>();

        #endregion

        /// <summary>
        /// This must be called before save load, will generate the config in Custom_Events.cfg
        /// </summary>
        /// <param name="mEvents">MEvents.</param>
        //public static void AddEvents(params MEvent[] mEvents) => customEventsList.Add(new CustomEvents(Configuration.customEventConfig, mEvents.ToList()));

        /// <summary>
        /// This must be called before save load, will generate the config in specified config file.
        /// </summary>
        /// <param name="toConfig">Config to generate to.</param>
        /// <param name="mEvents">MEvents.</param>
        //public static void AddEvents(ConfigFile toConfig, params MEvent[] mEvents) => customEventsList.Add(new CustomEvents(toConfig, mEvents.ToList()));

        internal static MEvent RandomWeightedEvent(List<MEvent> _events, System.Random rng)
        {
            if (_events.Count == 0) return new Events.Nothing();

            int WeightedSum = 0;
            foreach (MEvent e in _events) WeightedSum += e.Weight;

            foreach (MEvent e in _events)
            {
                if (rng.Next(0, WeightedSum) < e.Weight)
                {
                    return e;
                }
                WeightedSum -= e.Weight;
            }

            return _events[_events.Count - 1];
        }

        /// <summary>
        /// Choose events for the round
        /// </summary>
        /// <param name="additionalEvents"></param>
        /// <returns></returns>
        internal static List<MEvent> ChooseEvents(out List<MEvent> additionalEvents)
        {
            currentEvents.Clear();
            sideEvents.Clear();

            List<MEvent> chosenEvents = new List<MEvent>();
            List<MEvent> eventsToChooseForm = new List<MEvent>();
            foreach (MEvent e in events) eventsToChooseForm.Add(e);

            // Decide how many events to spawn
            System.Random rng = new System.Random(StartOfRound.Instance.randomMapSeed + 32345 + Environment.TickCount);
            int eventsToSpawn = (int)MEvent.Scale.Compute(Configuration.eventsToSpawn, MEvent.EventType.Neutral) + RoundManager.Instance.GetRandomWeightedIndex(Configuration.weightsForExtraEvents.IntArray(), rng);

            if (Configuration.scaleHeat.Value)
            {
                float currentHeat = currentHeatDifficulty();

                if ((currentHeat == Configuration.heatMaxCap.Value) && Configuration.heatForceEventAtMax.Value)
                {
                    Log.LogInfo("Current heat has hit max cap, now forcing events");
                    Log.LogInfo("Attempting to force events: " + Configuration.heatEventsToForce.Value);

                    string[] eventNames = Configuration.heatEventsToForce.Value
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToArray();

                    forcedEvents.AddRange(GetEventsByName(eventNames));
                }
            }

            //Forced events
            foreach (MEvent forcedEvent in forcedEvents)
            {
                eventsToChooseForm.RemoveAll(x => x.Name() == forcedEvent.Name());
                foreach (string eventToRemove in forcedEvent.EventsToRemove)
                {
                    eventsToChooseForm.RemoveAll(x => x.Name() == forcedEvent.Name());
                }
            }

            // Spawn events
            for (int i = 0; i < eventsToSpawn; i++)
            {
                MEvent newEvent = RandomWeightedEvent(eventsToChooseForm, rng);

                if (!newEvent.AddEventIfOnly()) // If event condition is false, remove event from eventsToChoosefrom and iterate again
                {
                    i--;
                    eventsToChooseForm.RemoveAll(x => x.Name() == newEvent.Name());
                    continue;
                }

                // Check moon whitelist/blacklist
                bool eventValid = newEvent.MoonMode
                                   ? IsEventOnMoonWhitelist(newEvent)
                                   : !IsIgnoredEventByMoonBlacklist(newEvent);

                Log.LogInfo($"Checking {(newEvent.MoonMode ? "whitelist" : "blacklist")} for event {newEvent.Name()}");
                if (!eventValid)
                {
                    Log.LogInfo($"Event {newEvent.Name()} is {(newEvent.MoonMode ? "not whitelisted" : "blacklisted")} on moon {Manager.currentLevel.PlanetName}, skipping.");
                }

                // To prevent overwriting eventValid from previous checks where it declared invalid, check if eventValid is still true here.
                if (eventValid)
                {
                    if (newEvent.isSpecialEvent || newEvent.isBetaEvent)
                    {
                        bool specialFailed = newEvent.isSpecialEvent && !Configuration.enableSpecialEvents.Value;

                        bool betaFailed = newEvent.isBetaEvent && !Configuration.enableBetaEvents.Value;

                        if (specialFailed || betaFailed)
                        {
                            eventValid = false;
                        }
                        else
                        {
                            eventValid = true;
                        }
                    }
                }

                // Remove event and iterate again if condition is not met for event
                if (!eventValid)
                {
                    //Log.LogInfo($"Event {newEvent.Name()} is {(newEvent.MoonMode ? "not whitelisted" : "blacklisted")} on moon {Manager.currentLevel.PlanetName}, skipping.");
                    i--;
                    eventsToChooseForm.RemoveAll(x => x.Name() == newEvent.Name());
                    continue;
                }



                // Add event and remove incompatible events
                if (eventValid)
                {
                    chosenEvents.Add(newEvent);

                    // Remove so no further accurrences
                    eventsToChooseForm.RemoveAll(x => x.Name() == newEvent.Name());

                    // Remove incompatible events from toChooseList
                    int AmountRemoved = 0;

                    foreach (string eventToRemove in newEvent.EventsToRemove)
                    {
                        eventsToChooseForm.RemoveAll(x => x.Name() == eventToRemove);
                        AmountRemoved += chosenEvents.RemoveAll(x => x.Name() == eventToRemove);
                    }

                    foreach (string eventToSpawnWith in newEvent.EventsToSpawnWith)
                    {
                        eventsToChooseForm.RemoveAll(x => x.Name() == eventToSpawnWith);
                        AmountRemoved += chosenEvents.RemoveAll(x => x.Name() == eventToSpawnWith);
                    }

                    i -= AmountRemoved; // Decrement each time an event is removed from chosenEvents list
                }
                else
                {
                    i--;
                }
            }

            // Generate eventsToSpawnWith list with no copies
            List<MEvent> eventsToSpawnWith = new List<MEvent>();
            for (int i = 0; i < chosenEvents.Count; i++)
            {
                foreach (string eventToSpawnWith in chosenEvents[i].EventsToSpawnWith)
                {
                    int index = eventsToSpawnWith.FindIndex(x => x.Name() == eventToSpawnWith);
                    if (index == -1) eventsToSpawnWith.Add(MEvent.GetEvent(eventToSpawnWith)); // If dosen't exist in list, add.
                    index = sideEvents.FindIndex(x => x.Name() == eventToSpawnWith);
                    if (index == -1) sideEvents.Add(MEvent.GetEvent(eventToSpawnWith)); // If dosen't exist in list, add.

                }
            }

            // Remove disabledEvents from EventsToSpawnWith List
            foreach (MEvent e in disabledEvents)
            {
                int index = eventsToSpawnWith.FindIndex(x => x.Name() == e.Name());
                if (index != -1) eventsToSpawnWith.RemoveAt(index);
            }

            additionalEvents = eventsToSpawnWith;
            currentEvents = chosenEvents;
            return chosenEvents;
        }

        internal static void ApplyEvents(List<MEvent> currentEvents)
        {

            foreach (MEvent e in currentEvents)
            {
                if (!e.Executed)
                {
                    e.Executed = true;
                    e.Execute();
                }
            }
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.ShipLeave))]
        public static void FixHazardsOnLeave()
        {
            //Check if host otherwise return
            if (!RoundManager.Instance.IsHost) return;

            if (RoundManager.Instance?.currentLevel?.indoorMapHazards == null) return;

            var currentHazards = RoundManager.Instance.currentLevel.indoorMapHazards.ToList();

            currentHazards.RemoveAll(hazard => hazards.Contains(hazard));

            RoundManager.Instance.currentLevel.indoorMapHazards = currentHazards.ToArray();

            hazards.Clear();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.OnLocalDisconnect))]
        public static void FixHazardsOnLeaveLocalDisconnect()
        {
            FixHazardsOnLeave();
        }


        /// <summary>
        /// Execute on ship leave
        /// </summary>
        internal static void ExecuteOnShipLeave()
        {
            Log.LogInfo("Executing OnShipLeave for all events()");
            foreach (MEvent e in events)
            {
                e.OnShipLeave();
            }

            foreach (MEvent e in vanillaEvents)
            {
                e.OnShipLeave();
            }

            foreach (MEvent e in moddedEvents)
            {
                e.OnShipLeave();
            }

            foreach (MEvent e in customEvents)
            {
                e.OnShipLeave();
            }

            //foreach(MEvent e in ExternalEvents)
            //{
            //    e.OnShipLeave();
            //}
        }

        /// <summary>
        /// Execute on game start
        /// </summary>
        internal static void ExecuteOnGameStart()
        {
            Log.LogInfo("Executing OnGameStart for all events()");
            if (currentEvents != null)
            {
                currentEvents.Clear(); // sanity check
            }
            if (sideEvents != null)
            {
                sideEvents.Clear(); // sanity check
            }
            foreach (MEvent e in events)
            {
                e.OnGameStart();
            }

            foreach (MEvent e in vanillaEvents)
            {
                e.OnGameStart();
            }

            foreach (MEvent e in moddedEvents)
            {
                e.OnGameStart();
            }

            foreach (MEvent e in customEvents)
            {
                e.OnGameStart();
            }

            //foreach (MEvent e in ExternalEvents)
            //{
            //    e.OnGameStart();
            //}
        }

        /// <summary>
        /// Execute on local disconnect
        /// </summary>
        internal static void ExecuteOnLocalDisconnect()
        {
            Log.LogInfo("Executing OnLocalDisconnect for all events()");
            foreach (MEvent e in events)
            {
                e.OnLocalDisconnect();
            }

            foreach (MEvent e in vanillaEvents)
            {
                e.OnLocalDisconnect();
            }

            foreach (MEvent e in moddedEvents)
            {
                e.OnLocalDisconnect();
            }

            //foreach (MEvent e in ExternalEvents)
            //{
            //    e.OnLocalDisconnect();
            //}
        }

        internal static void UpdateAllEventWeights()
        {
            if (Configuration.useCustomWeights.Value) return;

            if (Configuration.EnableRandomizer.Value && !Configuration.speedrunMode.Value)
            {
                if (RoundManager.Instance.IsServer)
                {
                        // If the game is day 0, then we want to randomize the weights.
                        if (StartOfRound.Instance.gameStats.daysSpent == 0 && (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.Start) || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.All))))
                        {
                            if (Configuration.RandomizeWeight.Value && Configuration.RandomizeWeight != null)
                            {
                                Log.LogInfo("Randomizing event weights for day 0");
                                RandomizeWeight();
                            }
                        }

                        if (StartOfRound.Instance.gameStats.daysSpent != 0)
                        {
                            if (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.LeverPull) || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.All)))
                            {
                                if (Configuration.RandomizeWeight.Value && Configuration.RandomizeWeight != null)
                                {
                                    Log.LogInfo("Randomizing on lever pull");
                                    RandomizeWeight();
                                }
                            }
                        }
                }
            }

            float fix(float value) // This is to avoid division by zero
            {
                if (value < 1) return 1;
                return value;
            }

            int eventTypeAmount = Configuration.eventTypeScales.Length;

            float[] computedScales = new float[eventTypeAmount];
            for (int i = 0; i < eventTypeAmount; i++) computedScales[i] = MEvent.Scale.Compute(Configuration.eventTypeScales[i]);

            float eventTypeWeightSum = 0;
            for (int i = 0; i < eventTypeAmount; i++) eventTypeWeightSum += computedScales[i];
            eventTypeWeightSum = fix(eventTypeWeightSum);

            float[] eventTypeProbabilities = new float[eventTypeAmount];
            for (int i = 0; i < eventTypeAmount; i++) eventTypeProbabilities[i] = computedScales[i] / eventTypeWeightSum;
            eventTypeRarities = eventTypeProbabilities;

            int[] newEventWeights = new int[eventTypeAmount];
            for (int i = 0; i < eventTypeAmount; i++)
            {
                newEventWeights[i] = (int)((eventTypeSum / fix(eventTypeCount[i])) * eventTypeProbabilities[i] * 1000.0f);
                Log.LogInfo($"Set eventType weight for {((MEvent.EventType)Enum.ToObject(typeof(MEvent.EventType), i)).ToString()} to {newEventWeights[i]}");
            }

            foreach (MEvent e in events) e.Weight = newEventWeights[(int)e.Type];
        }

        internal static void UpdateEventTypeCounts()
        {
            int eventTypeAmount = Configuration.eventTypeScales.Length;

            eventTypeCount = new float[eventTypeAmount];
            for (int i = 0; i < eventTypeAmount; i++) eventTypeCount[i] = 0.0f;
            foreach (MEvent e in events) eventTypeCount[(int)e.Type]++;

            eventTypeSum = 0.0f;
            for (int i = 0; i < eventTypeAmount; i++) eventTypeSum += eventTypeCount[i];
        }

        internal static void UpdateEventDescriptions(List<MEvent> events)
        {
            if (!Configuration.displayEvents.Value) return;
            currentEventDescriptions.Clear();
            foreach (MEvent e in events)
            {
                currentEventDescriptions.Add($"<color={e.ColorHex}>{e.Descriptions[UnityEngine.Random.Range(0, e.Descriptions.Count)]}</color>");
            }
        }

        /// <summary>
        /// This is used to describe difficulty name and color transitioning in the UI.
        /// </summary>
        public struct DifficultyTransition : IComparable<DifficultyTransition>
        {
            internal const uint byteMask = 0b_00000000_00000000_00000000_11111111;

            public string name;
            public string hex;
            public float above;

            public uint[] rgb;

            public DifficultyTransition(string name, string hex, float above)
            {
                this.name = name;
                this.hex = hex;
                this.above = above;

                rgb = new uint[3];
                uint parsedValue = 0;
                try
                {
                    parsedValue = uint.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                } catch
                {
                    Log.LogError("Failed to parse hex.");
                }

                rgb[0] = (parsedValue >> 16) & byteMask; // r
                rgb[1] = (parsedValue >> 8) & byteMask;  // g
                rgb[2] = parsedValue & byteMask;         // b
            }

            public string GetTransitionHex(DifficultyTransition next)
            {
                float at = Mathf.Clamp((next.above - Manager.difficulty) / (next.above - above), 0.0f, 1.0f);

                uint newR = InBetween(rgb[0], next.rgb[0], at);
                uint newG = InBetween(rgb[1], next.rgb[1], at);
                uint newB = InBetween(rgb[2], next.rgb[2], at);

                return newR.ToString("X2") + newG.ToString("X2") + newB.ToString("X2");
            }

            private uint InBetween(uint min, uint max, float at) => (uint)Mathf.Clamp((at * (max - min)) + min, 0.0f, 255.0f);

            public int CompareTo(DifficultyTransition other)
            {
                return above.CompareTo(other.above);
            }
        }

        internal class CustomEvents
        {
            public ConfigFile configFile;

            public List<MEvent> events;

            public CustomEvents(ConfigFile configFile, List<MEvent> events)
            {
                this.configFile = configFile;
                this.events = events;
            }
        }

        /// <summary>
        /// Modify level on load
        /// </summary>
        /// <param name="newLevel"></param>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.LoadNewLevel))]
        public static void ModifyLevel(ref SelectableLevel newLevel)
        {
            ExecuteOnShipLeave();
            UI.canClearText = false;
            Manager.ComputeDifficultyValues();

            Manager.currentLevel = newLevel;
            Manager.currentTerminal = GameObject.FindObjectOfType<Terminal>();

            Assets.generateOriginalValuesLists();
            Net.Instance.ClearGameObjectsServerRpc(); // Clear all previously placed objects on all clients
            if (!RoundManager.Instance.IsHost || newLevel.levelID == 3) return;

            LevelModifications.ResetValues(StartOfRound.Instance);

            // Apply weather multipliers
            foreach (Weather e in Net.Instance.currentWeatherMultipliers)
            {
                if (newLevel.currentWeather == e.weatherType)
                {
                    if (Configuration.AffectPropertiesOutOfEvents.Value)
                    {
                        Manager.scrapValueMultiplier *= e.scrapValueMultiplier;
                        Manager.scrapAmountMultiplier *= e.scrapAmountMultiplier;
                    }
                }
            }


            // Apply level properties
            LevelProperties properties = Configuration.levelProperties.GetValueOrDefault(newLevel.levelID);
            if (properties != null)
            {
                if (Configuration.AffectPropertiesOutOfEvents.Value)
                {
                    Manager.scrapValueMultiplier *= properties.GetScrapValueMultiplier();
                    Manager.scrapAmountMultiplier *= properties.GetScrapAmountMultiplier();
                }
            }

            // Do heat things here
            if (Configuration.scaleHeat.Value)
            {
                // Get current PlanetName
                Manager.levelNameOnLand = RoundManager.Instance.currentLevel.levelID;
                if (!Manager.heatDifficulty.ContainsKey(Manager.levelNameOnLand))
                { // Add the planet to the list if it does not exist.
                    Log.LogInfo("Adding an entry to the list with the ID " + Manager.levelNameOnLand + " and heat difficulty " + Configuration.startingHeat.Value);
                    Manager.heatDifficulty.Add(Manager.levelNameOnLand, Configuration.startingHeat.Value); //Declare 0 by default, or whatever the starting heat is set to if non zero.
                }

                //Decrement every other planet too, assuming the list is not null.
                if (Manager.heatDifficulty != null)
                {
                    foreach (var planet in Manager.heatDifficulty.Keys.ToList())
                    {
                        if (planet != Manager.levelNameOnLand)
                        {
                            Log.LogInfo(planet + " is not equal to " + Manager.levelNameOnLand + ". Current heat difficulty was " + Manager.heatDifficulty[planet] + ", decrementing by " + Configuration.heatDecrementAmount.Value);
                            Manager.heatDifficulty[planet] = Math.Max(Manager.heatDifficulty[planet] - Math.Abs(Configuration.heatDecrementAmount.Value), 0);
                            Log.LogInfo("New decremented heat recieved for " + planet + " is " + Manager.heatDifficulty[planet]);
                        }
                    }
                }
            }

            // Difficulty modifications
            if (Configuration.AffectPropertiesOutOfEvents.Value)
            {
                //Manager.AddEnemyHp((int)MEvent.Scale.Compute(Configuration.enemyBonusHpScaling));
                //Manager.AddInsideSpawnChance(newLevel, MEvent.Scale.Compute(Configuration.insideSpawnChanceAdditive));
                //Manager.AddOutsideSpawnChance(newLevel, MEvent.Scale.Compute(Configuration.outsideSpawnChanceAdditive));
                //Manager.MultiplySpawnChance(newLevel, MEvent.Scale.Compute(Configuration.spawnChanceMultiplierScaling));
                //Manager.MultiplySpawnCap(MEvent.Scale.Compute(Configuration.spawnCapMultiplier));
                //Manager.AddInsidePower((int)MEvent.Scale.Compute(Configuration.insideEnemyMaxPowerCountScaling));
                //Manager.AddOutsidePower((int)MEvent.Scale.Compute(Configuration.outsideEnemyPowerCountScaling));
                //Manager.scrapValueMultiplier *= MEvent.Scale.Compute(Configuration.scrapValueMultiplier);
                //Manager.scrapAmountMultiplier *= MEvent.Scale.Compute(Configuration.scrapAmountMultiplier);
                //Manager.factorySizeMultiplier = MEvent.Scale.Compute(Configuration.factorySizeMultiplier);

                /*Developer Note for anyone working with ModifyLevel, the following is invoked for difficulty selections:
                 * 1. Check if the randomizer is enabled and exists, if not, use the config value.
                 * 2. If the randomizer is enabled, check if the current day is 0 OR set to run every every lever pull, if so, randomize the value.
                 * 3. Otherwise, use the randomizer value stored from the save.
                 */

                if ((Configuration.EnableRandomizer != null && !Configuration.EnableRandomizer.Value) ||
                    (Configuration.RandomizeEnemyHP != null && !Configuration.RandomizeEnemyHP.Value) ||
                    Configuration.speedrunMode.Value)
                {
                    Manager.AddEnemyHp((int)MEvent.Scale.Compute(Configuration.enemyBonusHpScaling));
                }
                else
                {
                    if (StartOfRound.Instance.gameStats.daysSpent == 0 || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.LeverPull) || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.All))))
                    {
                        RandomizeBonusEnemyHP();
                    }
                    Manager.AddEnemyHp((int)(Manager.randomizerbonusenemyhp));
                }

                if ((Configuration.EnableRandomizer != null && !Configuration.EnableRandomizer.Value) ||
                (Configuration.RandomizeSpawnChanceInside != null && !Configuration.RandomizeSpawnChanceInside.Value) ||
                Configuration.speedrunMode.Value)
                {
                    Manager.AddInsideSpawnChance(newLevel, MEvent.Scale.Compute(Configuration.insideSpawnChanceAdditive));
                }
                else
                {
                    if (StartOfRound.Instance.gameStats.daysSpent == 0 || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.LeverPull) || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.All))))
                    {
                        RandomizeSpawnChanceInside();
                    }
                    Manager.AddInsideSpawnChance(newLevel, Manager.randomizerspawnchanceinside);
                }

                if ((Configuration.EnableRandomizer != null && !Configuration.EnableRandomizer.Value) ||
                (Configuration.RandomizeSpawnChanceOutside != null && !Configuration.RandomizeSpawnChanceOutside.Value) ||
                Configuration.speedrunMode.Value)
                {
                    Manager.AddOutsideSpawnChance(newLevel, MEvent.Scale.Compute(Configuration.outsideSpawnChanceAdditive));
                }
                else
                {
                    if (StartOfRound.Instance.gameStats.daysSpent == 0 || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.LeverPull) || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.All))))
                    {
                        RandomizeSpawnChanceOutside();
                    }
                    Manager.AddOutsideSpawnChance(newLevel, Manager.randomizerspawnchanceoutside);
                }

                if ((Configuration.EnableRandomizer != null && !Configuration.EnableRandomizer.Value) ||
                (Configuration.RandomizeSpawnChance != null && !Configuration.RandomizeSpawnChance.Value) ||
                Configuration.speedrunMode.Value)
                {
                    Manager.MultiplySpawnChance(newLevel, MEvent.Scale.Compute(Configuration.insideSpawnChanceAdditive));
                }
                else
                {
                    if (StartOfRound.Instance.gameStats.daysSpent == 0 || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.LeverPull) || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.All))))
                    {
                        RandomizeSpawnChance();
                    }
                    Manager.MultiplySpawnChance(newLevel, Manager.randomizerspawnchance);
                }

                if ((Configuration.EnableRandomizer != null && !Configuration.EnableRandomizer.Value) ||
                (Configuration.RandomizeSpawnCap != null && !Configuration.RandomizeSpawnCap.Value) ||
                Configuration.speedrunMode.Value)
                {
                    Manager.MultiplySpawnCap(MEvent.Scale.Compute(Configuration.spawnCapMultiplier));
                }
                else
                {
                    if (StartOfRound.Instance.gameStats.daysSpent == 0 || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.LeverPull) || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.All))))
                    {
                        RandomizeSpawnCap();
                    }
                    Manager.MultiplySpawnCap(Manager.randomizerspawncap);
                }

                if ((Configuration.EnableRandomizer != null && !Configuration.EnableRandomizer.Value) ||
                (Configuration.RandomizeInsidePower != null && !Configuration.RandomizeInsidePower.Value) ||
                Configuration.speedrunMode.Value)
                {
                    Manager.AddInsidePower((int)MEvent.Scale.Compute(Configuration.insideEnemyMaxPowerCountScaling));
                }
                else
                {
                    if (StartOfRound.Instance.gameStats.daysSpent == 0 || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.LeverPull) || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.All))))
                    {
                        RandomizeInsidePower();
                    }
                    Manager.AddInsidePower((int)(Manager.randomizerinsidepower));
                }

                if ((Configuration.EnableRandomizer != null && !Configuration.EnableRandomizer.Value) ||
                (Configuration.RandomizeOutsidePower != null && !Configuration.RandomizeOutsidePower.Value) ||
                Configuration.speedrunMode.Value)
                {
                    Manager.AddOutsidePower((int)MEvent.Scale.Compute(Configuration.outsideEnemyPowerCountScaling));
                }
                else
                {
                    if (StartOfRound.Instance.gameStats.daysSpent == 0 || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.LeverPull) || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.All))))
                    {
                        RandomizeOutsidePower();
                    }
                    Manager.AddOutsidePower((int)(Manager.randomizeroutsidepower));
                }


                if ((Configuration.EnableRandomizer != null && !Configuration.EnableRandomizer.Value) ||
                (Configuration.RandomizeScrapValue != null && !Configuration.RandomizeScrapValue.Value) ||
                Configuration.speedrunMode.Value)
                {
                    Manager.scrapValueMultiplier *= MEvent.Scale.Compute(Configuration.scrapValueMultiplier);
                }
                else
                {
                    if (StartOfRound.Instance.gameStats.daysSpent == 0 || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.LeverPull) || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.All))))
                    {
                        RandomizeScrapValue();
                    }
                    Manager.scrapValueMultiplier *= Manager.randomizerscrapvalue;
                }

                if ((Configuration.EnableRandomizer != null && !Configuration.EnableRandomizer.Value) ||
                (Configuration.RandomizeScrapAmount != null && !Configuration.RandomizeScrapAmount.Value) ||
                Configuration.speedrunMode.Value)
                {
                    Manager.scrapAmountMultiplier *= MEvent.Scale.Compute(Configuration.scrapAmountMultiplier);
                }
                else
                {
                    if (StartOfRound.Instance.gameStats.daysSpent == 0 || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.LeverPull) || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.All))))
                    {
                        RandomizeScrapAmount();
                    }
                    Manager.scrapAmountMultiplier *= Manager.randomizerscrapamount;
                }


                if ((Configuration.EnableRandomizer != null && !Configuration.EnableRandomizer.Value) ||
                (Configuration.RandomizeFactory != null && !Configuration.RandomizeFactory.Value) ||
                Configuration.speedrunMode.Value)
                {
                    Manager.factorySizeMultiplier *= MEvent.Scale.Compute(Configuration.factorySizeMultiplier);
                }
                else
                {
                    if (StartOfRound.Instance.gameStats.daysSpent == 0 || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.LeverPull) || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.All))))
                    {
                        RandomizeFactory();
                    }
                    Manager.factorySizeMultiplier *= Manager.randomizerfactory;
                }
            }

            List<MEvent> additionalEvents = new List<MEvent>();
            List<MEvent> currentEvents = new List<MEvent>();

            if (Configuration.ExtraLogging.Value)
            {
                if (newLevel.PlanetName != null)
                {
                    Log.LogInfo("Moon name is " + newLevel.PlanetName);
                }
            }

            bool skipEventActivation = IsIgnoredMoon(newLevel.PlanetName) || IsIgnoredMoon(newLevel.name);

            //It looks like this is a mistake to have a second check, but its here for the Event Chance feature.
            skipEventActivation |= !DoesEventsRunByChance();

            if (Configuration.DisableAllEvents.Value)
            {
                Log.LogInfo("All events are disabled. Skipping event activation.");
                skipEventActivation = true;
            }

            if (!skipEventActivation)
            {
                // Choose any apply events
                if (!Configuration.useCustomWeights.Value) UpdateAllEventWeights();

                currentEvents = ChooseEvents(out additionalEvents);

                foreach (MEvent e in currentEvents)
                {
                    Log.LogInfo("Event chosen: " + e.Name()); // Log Chosen events
                    Manager.scrapValueMultiplier *= MEvent.Scale.Compute(Configuration.scrapValueByEventTypeScale[e.Type]);
                    Manager.scrapAmountMultiplier *= MEvent.Scale.Compute(Configuration.scrapAmountByEventTypeScale[e.Type]);
                }
                foreach (MEvent e in additionalEvents) Log.LogInfo("Additional events: " + e.Name());

                ApplyEvents(currentEvents);
                ApplyEvents(additionalEvents);
            }

            foreach (MEvent forcedEvent in forcedEvents)
            {
                forcedEvent.Execute();

                foreach (string additionalEvent in forcedEvent.EventsToSpawnWith)
                {
                    MEvent.GetEvent(additionalEvent).Execute();
                }
            }

            currentEvents.AddRange(forcedEvents);
            forcedEvents.Clear();

            UpdateEventDescriptions(currentEvents);

            if (Configuration.showEventsInChat.Value && !Configuration.DisplayUIAfterShipLeaves.Value)
            {
                HUDManager.Instance.AddTextToChatOnServer("<color=#FFFFFF>Events:</color>");
                foreach (string eventDescription in currentEventDescriptions)
                {
                    HUDManager.Instance.AddTextToChatOnServer(eventDescription);
                }
            }

            if (tipEventsToDo.Count > 0)
            {
                tipEventsToDo.Clear();
            }
            tipEventsToDo.AddRange(currentEvents);

            // Apply maxPower counts Inside
            RoundManager.Instance.currentLevel.maxEnemyPowerCount = (int)((RoundManager.Instance.currentLevel.maxEnemyPowerCount + Manager.bonusMaxInsidePowerCount) * Manager.spawncapMultipler);
            if (Configuration.scaleHeat.Value && (Configuration.heatSettingsToAffect.Value.HasFlag(Configuration.HeatSettingsFlags.InsidePower) || (Configuration.heatSettingsToAffect.Value.HasFlag(Configuration.HeatSettingsFlags.All))))
            {
                float heatIndex = EventManager.currentHeatDifficulty();
                if (heatIndex > 0)
                {
                    if (Configuration.AffectPropertiesOutOfEvents.Value)
                    {
                        RoundManager.Instance.currentLevel.maxEnemyPowerCount *= (int)((Configuration.heatMultiplierOtherCalculations.Value * RoundManager.Instance.currentLevel.maxEnemyPowerCount / Configuration.heatDampening.Value) * Math.Pow(1 + Configuration.heatDampening.Value, heatIndex) + 1);
                    }
                }
            }

            RoundManager.Instance.currentLevel.maxOutsideEnemyPowerCount = (int)((RoundManager.Instance.currentLevel.maxOutsideEnemyPowerCount + Manager.bonusMaxOutsidePowerCount) * Manager.spawncapMultipler);
            if (Configuration.scaleHeat.Value && (Configuration.heatSettingsToAffect.Value.HasFlag(Configuration.HeatSettingsFlags.OutsidePower) || (Configuration.heatSettingsToAffect.Value.HasFlag(Configuration.HeatSettingsFlags.All))))
            {
                float heatIndex = EventManager.currentHeatDifficulty();
                if (heatIndex > 0)
                {
                    if (Configuration.AffectPropertiesOutOfEvents.Value)
                    {
                        RoundManager.Instance.currentLevel.maxOutsideEnemyPowerCount *= (int)((Configuration.heatMultiplierOtherCalculations.Value * RoundManager.Instance.currentLevel.maxOutsideEnemyPowerCount / Configuration.heatDampening.Value) * Math.Pow(1 + Configuration.heatDampening.Value, heatIndex) + 1);
                    }
                }
            }

            //Make sure scrap amount/value stays below the cap
            Manager.scrapValueMultiplier = Mathf.Clamp(Manager.scrapValueMultiplier, 0.0f, Configuration.scrapValueMax.Value);
            Manager.scrapAmountMultiplier = Mathf.Clamp(Manager.scrapAmountMultiplier, 0.0f, Configuration.scrapAmountMax.Value);

            //Apply factory size by the multiplier and clamp it
            RoundManager.Instance.currentLevel.factorySizeMultiplier *= Math.Clamp(Math.Abs(Manager.factorySizeMultiplier), Math.Abs(Configuration.FactorySizeMin.Value), Math.Abs(Configuration.FactorySizeMax.Value));

            // Check time adjustments
            if (Configuration.enableCustomTimeAdjustments.Value)
            {
                Net.Instance.MoveTimeServerRpc(Mathf.Min(MEvent.Scale.Compute(Configuration.startingTime), 2147483647.0f), Mathf.Clamp(MEvent.Scale.Compute(Configuration.timeScaling), 0.001f, 2147483647));
            }

            // Sync values to all clients
            Net.Instance.SyncValuesClientRpc(Manager.currentLevel.factorySizeMultiplier, Manager.scrapValueMultiplier, Manager.scrapAmountMultiplier, Manager.bonusEnemyHp);

            // Apply UI
            if (!Configuration.DisplayUIAfterShipLeaves.Value)
            {
                UI.GenerateText(currentEvents);
            }
            else
            {
                UI.ClearText();
            }

            // Logging
            Log.LogInfo($"MapMultipliers = [" +
                $"scrapValueMultiplier: {Manager.scrapValueMultiplier}, " +
                $"scrapAmountMultiplier: {Manager.scrapAmountMultiplier}, " +
                $"factorySizeMultiplier: {Manager.factorySizeMultiplier}, " +
                $"EnemyHp: {(int)MEvent.Scale.Compute(Configuration.enemyBonusHpScaling)}, " +
                $"insideSpawnChanceAdditive: {MEvent.Scale.Compute(Configuration.insideSpawnChanceAdditive)}, " +
                $"outsideSpawnChanceAdditive: {MEvent.Scale.Compute(Configuration.outsideSpawnChanceAdditive)}, " +
                $"SpawnChanceMultiplier: {MEvent.Scale.Compute(Configuration.spawnChanceMultiplierScaling)}, " +
                $"SpawnCapMultiplier: {MEvent.Scale.Compute(Configuration.spawnCapMultiplier)}, " +
                $"InsidePower: {(int)MEvent.Scale.Compute(Configuration.insideEnemyMaxPowerCountScaling)}, " +
                $"OutsidePower: {(int)MEvent.Scale.Compute(Configuration.outsideEnemyPowerCountScaling)}]");

            Log.LogInfo("Inside Spawn Curve");
            foreach (Keyframe key in newLevel.enemySpawnChanceThroughoutDay.keys) Log.LogInfo($"Time:{key.time} + $Value:{key.value}");
            Log.LogInfo("Outside Spawn Curve");
            foreach (Keyframe key in newLevel.outsideEnemySpawnChanceThroughDay.keys) Log.LogInfo($"Time:{key.time} + $Value:{key.value}");
            Log.LogInfo("Daytime Spawn Curve");
            foreach (Keyframe key in newLevel.daytimeEnemySpawnChanceThroughDay.keys) Log.LogInfo($"Time:{key.time} + $Value:{key.value}");

        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(RoundManager), "RefreshEnemiesList")]
        private static void OnRefreshEnemiesList()
        {
            if (RoundManager.Instance.IsServer)
            {
                HUDManager.Instance.StartCoroutine(EventTips(tipEventsToDo));
            }
        }

        internal static IEnumerator EventTips(List<MEvent> events)
        {
            yield return new WaitForSeconds(Mathf.Abs(Configuration.InitTimePopUp.Value));
            foreach (MEvent e in events)
            {
                if (e.showTip && e.TipMessages.Count > 0 && e.TipTitle.Count > 0)
                {
                    Net.Instance.SpawnTipServerRpc(e.TipTitle[UnityEngine.Random.Range(0, e.TipTitle.Count)], e.TipMessages[UnityEngine.Random.Range(0, e.TipMessages.Count)], e.isWarning);

                    yield return new WaitForSeconds(Mathf.Abs(Configuration.timeBetweenTips.Value));
                }
            }

            EventManager.tipEventsToDo.Clear();
        }

        /// <summary>
        /// Check if moon is ignored from events
        /// </summary>
        /// <param name="moonName"></param>
        /// <returns></returns>
        internal static bool IsIgnoredMoon(string moonName)
        {
            moonName = moonName.ToLower();
            string moonsToIgnore = Configuration.MoonsToIgnore.GetSerializedValue();
            string[] ignoredMoons = string.IsNullOrEmpty(moonsToIgnore)
                ? new string[0]
                : moonsToIgnore.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                              .Select(moon => moon.Trim().ToLower())
                              .ToArray();

            //Log.LogInfo(moonsToIgnore); //Current list of moons that are ignored

            bool skipEventActivation = false;

            foreach (string moon in ignoredMoons)
            {
                string moonNameNoNumbers = Regex.Replace(moonName, @"-?\d-?", string.Empty).Trim();
                string ignoredMoonNoNumbers = Regex.Replace(moon, @"-?\d-?", string.Empty).Trim();
                if (moonName == moon || moonNameNoNumbers == ignoredMoonNoNumbers)
                {
                    skipEventActivation = true;
                    Log.LogInfo("Moon is on list of moons to ignore events. Skipping Events");
                    break;
                }
            }

            return skipEventActivation;
        }

        internal static bool DoesEventsRunByChance()
        {
            float chanceByConfig = Mathf.Clamp(MEvent.Scale.Compute(Configuration.EventChanceGlobal), 0f, 100f);
            float computedChance = UnityEngine.Random.Range(0f, 100f);
            return computedChance <= chanceByConfig;
        }

        #region Randomizer

        [HarmonyPostfix]
        [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.SetNewProfitQuota))]
        public static void DoRandomOnQuotaFulfill()
        {
            if (!RoundManager.Instance.IsServer) return;
            if (Configuration.speedrunMode.Value) return;
            if (Configuration.EnableRandomizer.Value && (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.QuotaFulfill) || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.All))))
            {
                DoRandomizer();
            }
        }

        public static void ResetRandomizerData()
        {
            Log.LogInfo("Called a reset of all randomizer values");
            Manager.randomizerscrapamount = 1;
            Manager.randomizerscrapvalue = 1;
            Manager.randomizerspawnchanceinside = 0;
            Manager.randomizerspawnchanceoutside = 0;
            Manager.randomizerspawncap = 1;
            Manager.randomizerspawnchance = 1;
            Manager.randomizerfactory = 1;
            Manager.randomizerinsidepower = 0;
            Manager.randomizeroutsidepower = 0;
            Manager.randomizerbonusenemyhp = 0;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.Start))]
        public static void DoRandomOnStart()
        {
            if (!RoundManager.Instance.IsServer) return;
            if (Configuration.speedrunMode.Value) return;

            //Try loading any randomizer weight data if it exists.
            LoadRandomizerData();

            if (Configuration.EnableRandomizer.Value && (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.Start) || (Configuration.WhenRandomize.Value.HasFlag(Configuration.RandomizeFlags.All))))
            {
                DoRandomizer();
            }
        }

        internal static void DoRandomizer()
        {
            Log.LogInfo("Randomizer Called");

            if (Configuration.EnableRandomizer.Value && Configuration.RandomizeWeight.Value)
            {
                RandomizeWeight();
            }

            if (Configuration.EnableRandomizer.Value && Configuration.RandomizeScrapValue.Value)
            {
                RandomizeScrapValue();
            }

            if (Configuration.EnableRandomizer.Value && Configuration.RandomizeScrapAmount.Value)
            {
                RandomizeScrapAmount();
            }

            if (Configuration.EnableRandomizer.Value && Configuration.RandomizeFactory.Value)
            {
                RandomizeFactory();
            }

            if (Configuration.EnableRandomizer.Value && Configuration.RandomizeSpawnChanceInside.Value)
            {
                RandomizeSpawnChanceInside();
            }

            if (Configuration.EnableRandomizer.Value && Configuration.RandomizeSpawnChanceOutside.Value)
            {
                RandomizeSpawnChanceOutside();
            }

            if (Configuration.EnableRandomizer.Value && Configuration.RandomizeSpawnChance.Value)
            {
                RandomizeSpawnCap();
            }

            if (Configuration.EnableRandomizer.Value && Configuration.RandomizeSpawnCap.Value)
            {
                RandomizeSpawnChance();
            }

            if (Configuration.EnableRandomizer.Value && Configuration.RandomizeEnemyHP.Value)
            {
                RandomizeBonusEnemyHP();
            }

            if (Configuration.EnableRandomizer.Value && Configuration.RandomizeInsidePower.Value)
            {
                RandomizeInsidePower();
            }

            if (Configuration.EnableRandomizer.Value && Configuration.RandomizeOutsidePower.Value)
            {
                RandomizeOutsidePower();
            }
            SaveRandomizerData();
        }
        

        internal static void SaveRandomizerData()
        {
            string gameSaveName = GameNetworkManager.Instance.currentSaveFileName;

            try
            {
                var randomizerweight = events.ToDictionary(e => e.Name(), e => (float)e.Weight);
                ES3.Save("randomizerweights", randomizerweight, $"{gameSaveName}_Brutal");
            }
            catch (Exception ex)
            {
                Log.LogError($"Error saving randomizer weights to save file: {ex.Message}");
            }

            try
            {
                ES3.Save("randomizerscrapvalue", Manager.randomizerscrapvalue, $"{gameSaveName}_Brutal");
            }
            catch (Exception ex)
            {
                Log.LogError($"Error saving randomizer scrap value to save file: {ex.Message}");
            }

            try
            {
                ES3.Save("randomizerscrapamount", Manager.randomizerscrapamount, $"{gameSaveName}_Brutal");
            }
            catch (Exception ex)
            {
                Log.LogError($"Error saving randomizer scrap amount to save file: {ex.Message}");
            }

            try
            {
                ES3.Save("randomizerfactory", Manager.randomizerfactory, $"{gameSaveName}_Brutal");
            }
            catch (Exception ex)
            {
                Log.LogError($"Error saving randomizer factory size to save file: {ex.Message}");
            }

            try
            {
                ES3.Save("randomizerspawnchanceinside", Manager.randomizerspawnchanceinside, $"{gameSaveName}_Brutal");
            }
            catch (Exception ex)
            {
                Log.LogError($"Error saving randomizer inside power to save file: {ex.Message}");
            }

            try
            {
                ES3.Save("randomizerspawnchanceoutside", Manager.randomizerspawnchanceoutside, $"{gameSaveName}_Brutal");
            }
            catch (Exception ex)
            {
                Log.LogError($"Error saving randomizer outside power to save file: {ex.Message}");
            }

            try
            {
                ES3.Save("randomizerspawncap", Manager.randomizerspawncap, $"{gameSaveName}_Brutal");
            }
            catch (Exception ex)
            {
                Log.LogError($"Error saving randomizer spawn cap to save file: {ex.Message}");
            }

            try
            {
                ES3.Save("randomizerspawnchance", Manager.randomizerspawnchance, $"{gameSaveName}_Brutal");
            }
            catch (Exception ex)
            {
                Log.LogError($"Error saving randomizer spawn chance to save file: {ex.Message}");
            }

            try
            {
                ES3.Save("randomizerbonusenemyhp", Manager.randomizerbonusenemyhp, $"{gameSaveName}_Brutal");
            }
            catch (Exception ex)
            {
                Log.LogError($"Error saving randomizer bonus enemy HP to save file: {ex.Message}");
            }

            try
            {
                ES3.Save("randomizerinsidepower", Manager.randomizerinsidepower, $"{gameSaveName}_Brutal");
            }
            catch (Exception ex)
            {
                Log.LogError($"Error saving randomizer inside power to save file: {ex.Message}");
            }

            try
            {
                ES3.Save("randomizeroutsidepower", Manager.randomizeroutsidepower, $"{gameSaveName}_Brutal");
            }
            catch (Exception ex)
            {
                Log.LogError($"Error saving randomizer outside power to save file: {ex.Message}");
            }
        }

        internal static void LoadRandomizerData()
        {
            string gameSaveName = GameNetworkManager.Instance.currentSaveFileName;

            Log.LogInfo("Attempting to load randomizer data");

            try
            {
                if (ES3.KeyExists("randomizerweights", $"{gameSaveName}_Brutal"))
                {
                    var randomizerweight = ES3.Load<Dictionary<string, float>>("randomizerweights", $"{gameSaveName}_Brutal");
                    foreach (MEvent e in events)
                    {
                        e.Weight = randomizerweight.ContainsKey(e.Name()) ? (int)randomizerweight[e.Name()] : e.Weight;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Error checking for randomizer weight in save file: {ex.Message}");
            }

            try
            {
                if (ES3.KeyExists("randomizerscrapvalue", $"{gameSaveName}_Brutal"))
                {
                    Manager.randomizerscrapvalue = ES3.Load<float>("randomizerscrapvalue");
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Error checking for randomizer scrap value in save file: {ex.Message}");
            }

            try
            {
                if (ES3.KeyExists("randomizerscrapamount", $"{gameSaveName}_Brutal"))
                {
                    Manager.randomizerscrapamount = ES3.Load<float>("randomizerscrapamount");
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Error checking for randomizer scrap amount in save file: {ex.Message}");
            }

            try
            {
                if (ES3.KeyExists("randomizerfactory", $"{gameSaveName}_Brutal"))
                {
                    Manager.randomizerfactory = ES3.Load<float>("randomizerfactory");
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Error checking for randomizer factory in save file: {ex.Message}");
            }

            try
            {
                if (ES3.KeyExists("randomizerspawnchanceinside", $"{gameSaveName}_Brutal"))
                {
                    Manager.randomizerspawnchanceinside = ES3.Load<float>("randomizerspawnchanceinside");
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Error checking for randomizer inside chance in save file: {ex.Message}");
            }

            try
            {
                if (ES3.KeyExists("randomizerspawnchanceoutside", $"{gameSaveName}_Brutal"))
                {
                    Manager.randomizerspawnchanceoutside = ES3.Load<float>("randomizerspawnchanceoutside");
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Error checking for randomizer outside chance in save file: {ex.Message}");
            }

            try
            {
                if (ES3.KeyExists("randomizerspawncap", $"{gameSaveName}_Brutal"))
                {
                    Manager.randomizerspawncap = ES3.Load<float>("randomizerspawncap");
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Error checking for randomizer spawn cap in save file: {ex.Message}");
            }

            try
            {
                if (ES3.KeyExists("randomizerspawnchance", $"{gameSaveName}_Brutal"))
                {
                    Manager.randomizerspawnchance = ES3.Load<float>("randomizerspawnchance");
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Error checking for randomizer spawn chance in save file: {ex.Message}");
            }

            try
            {
                if (ES3.KeyExists("randomizerbonusenemyhp", $"{gameSaveName}_Brutal"))
                {
                    Manager.randomizerbonusenemyhp = ES3.Load<int>("randomizerbonusenemyhp");
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Error checking for randomizer bonus enemy hp in save file: {ex.Message}");
            }

            try
            {
                if (ES3.KeyExists("randomizerinsidepower", $"{gameSaveName}_Brutal"))
                {
                    Manager.randomizerinsidepower = ES3.Load<int>("randomizerinsidepower");
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Error checking for randomizer inside power in save file: {ex.Message}");
            }

            try
            {
                if (ES3.KeyExists("randomizeroutsidepower", $"{gameSaveName}_Brutal"))
                {
                    Manager.randomizeroutsidepower = ES3.Load<int>("randomizeroutsidepower");
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Error checking for randomizer outside  power in save file: {ex.Message}");
            }

        }

        public static void RandomizeWeight()
        {
            Log.LogInfo("Randomizing all event weights");

            float minAmount = MEvent.Scale.Compute(Configuration.RandomizeWeightMin);
            float maxAmount = MEvent.Scale.Compute(Configuration.RandomizeWeightMax);

            foreach (MEvent e in events)
            {
                int newWeight = Mathf.RoundToInt(UnityEngine.Random.Range(minAmount, Mathf.Ceil(maxAmount) + 1));

                Log.LogInfo($"Randomized weight for {e.Name()} from {e.Weight} to {newWeight}");
                e.Weight = newWeight;
            }
        }

        public static void RandomizeFactory()
        {
            Log.LogInfo("Randomizing all event weights");
            float minAmount = MEvent.Scale.Compute(Configuration.RandomizeFactoryMin);
            float maxAmount = MEvent.Scale.Compute(Configuration.RandomizeFactoryMax);
            Manager.randomizerfactory = UnityEngine.Random.Range(minAmount, maxAmount);
        }

        public static void RandomizeScrapValue()
        {
            Log.LogInfo("Randomizing scrap value multiplier");
            float minAmount = MEvent.Scale.Compute(Configuration.RandomizeScrapValueMin);
            float maxAmount = MEvent.Scale.Compute(Configuration.RandomizeScrapValueMax);
            Manager.randomizerscrapvalue = UnityEngine.Random.Range(minAmount, maxAmount);
        }

        public static void RandomizeScrapAmount()
        {
            Log.LogInfo("Randomizing scrap amount multiplier");
            float minAmount = MEvent.Scale.Compute(Configuration.RandomizeScrapAmountMin);
            float maxAmount = MEvent.Scale.Compute(Configuration.RandomizeScrapAmountMax);
            Manager.randomizerscrapamount = UnityEngine.Random.Range(minAmount, maxAmount);
        }

        public static void RandomizeSpawnChanceInside()
        {
            Log.LogInfo("Randomizing inside spawn chance");
            float minAmount = MEvent.Scale.Compute(Configuration.RandomizeSpawnChanceInsideMin);
            float maxAmount = MEvent.Scale.Compute(Configuration.RandomizeSpawnChanceInsideMax);
            Manager.randomizerspawnchanceinside = UnityEngine.Random.Range(minAmount, maxAmount);
        }

        public static void RandomizeSpawnChanceOutside()
        {
            Log.LogInfo("Randomizing outside spawn chance");
            float minAmount = MEvent.Scale.Compute(Configuration.RandomizeSpawnChanceOutsideMin);
            float maxAmount = MEvent.Scale.Compute(Configuration.RandomizeSpawnChanceOutsideMax);
            Manager.randomizerspawnchanceoutside = UnityEngine.Random.Range(minAmount, maxAmount);
        }

        public static void RandomizeSpawnCap()
        {
            Log.LogInfo("Randomizing spawn cap");
            float minAmount = MEvent.Scale.Compute(Configuration.RandomizeSpawnCapMin);
            float maxAmount = MEvent.Scale.Compute(Configuration.RandomizeSpawnCapMax);
            Manager.randomizerspawncap = UnityEngine.Random.Range(minAmount, maxAmount);
        }

        public static void RandomizeSpawnChance()
        {
            Log.LogInfo("Randomizing spawn chance");
            float minAmount = MEvent.Scale.Compute(Configuration.RandomizeSpawnChanceMin);
            float maxAmount = MEvent.Scale.Compute(Configuration.RandomizeSpawnChanceMax);
            Manager.randomizerspawnchance = UnityEngine.Random.Range(minAmount, maxAmount);
        }

        public static void RandomizeBonusEnemyHP()
        {
            Log.LogInfo("Randomizing bonus enemy HP");
            float minAmount = MEvent.Scale.Compute(Configuration.RandomizeEnemyHPMin);
            float maxAmount = MEvent.Scale.Compute(Configuration.RandomizeEnemyHPMax);
            Manager.randomizerbonusenemyhp = Mathf.RoundToInt(UnityEngine.Random.Range(minAmount, maxAmount));
        }

        public static void RandomizeInsidePower()
        {
            Log.LogInfo("Randomizing inside power");
            float minAmount = MEvent.Scale.Compute(Configuration.RandomizeInsidePowerMin);
            float maxAmount = MEvent.Scale.Compute(Configuration.RandomizeInsidePowerMax);
            Manager.randomizerinsidepower = Mathf.RoundToInt(UnityEngine.Random.Range(minAmount, maxAmount));
        }

        public static void RandomizeOutsidePower()
        {
            Log.LogInfo("Randomizing outside power");
            float minAmount = MEvent.Scale.Compute(Configuration.RandomizeOutsidePowerMin);
            float maxAmount = MEvent.Scale.Compute(Configuration.RandomizeOutsidePowerMax);
            Manager.randomizeroutsidepower = Mathf.RoundToInt(UnityEngine.Random.Range(minAmount, maxAmount));
        }
    


        #endregion

        /// <summary>
        /// Check if event is ignored by moon blacklist
        /// </summary>
        /// <param name="mEvent"></param>
        /// <returns></returns>
        internal static bool IsIgnoredEventByMoonBlacklist(MEvent mEvent)
        {
            string currentMoonPlanetName = Manager.currentLevel.PlanetName;
            string currentMoonPlanetNameNoNumbers = Regex.Replace(currentMoonPlanetName, @"-?\d-?", string.Empty).Trim();

            string currentMoonName = Manager.currentLevel.name;
            string currentMoonNameNoNumbers = Regex.Replace(currentMoonName, @"-?\d-?", string.Empty).Trim();


            //Log.LogWarning($"{mEvent.Name()} Moon Blacklist: {string.Join(", ", mEvent.MoonBlacklist)}");
            //Log.LogWarning("Current Moon: " + currentMoon);

            // Check is current moon in blacklist

            if (mEvent.Blacklist.Contains(currentMoonPlanetName, StringComparer.OrdinalIgnoreCase) || mEvent.Blacklist.Contains(currentMoonPlanetName, StringComparer.OrdinalIgnoreCase) || mEvent.Blacklist.Contains(currentMoonName, StringComparer.OrdinalIgnoreCase) || mEvent.Blacklist.Contains(currentMoonNameNoNumbers, StringComparer.OrdinalIgnoreCase))
            {
                Log.LogInfo($"Event {mEvent.Name()} is ignored due to moon blacklist.");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if event is on moon whitelist
        /// </summary>
        /// <param name="mEvent"></param>
        /// <returns></returns>
        internal static bool IsEventOnMoonWhitelist(MEvent mEvent)
        {
            string currentMoonPlanetName = Manager.currentLevel.PlanetName;
            string currentMoonPlanetNameNoNumbers = Regex.Replace(currentMoonPlanetName, @"-?\d-?", string.Empty).Trim();

            string currentMoonName = Manager.currentLevel.name;
            string currentMoonNameNoNumbers = Regex.Replace(currentMoonName, @"-?\d-?", string.Empty).Trim();


            //Log.LogWarning($"{mEvent.Name()} Moon Whitelist: {string.Join(", ", mEvent.Whitelist)}");
            //Log.LogWarning("Current Moon: " + currentMoon);

            // Check is current moon in whitelist
            if (mEvent.Whitelist.Count == 0)
            {
                Log.LogInfo($"Event {mEvent.Name()} has an empty moon whitelist, but whitelist mode is on. Please consider either entering entries for the list or turn off the whitelist mode");
                return false; // Whitelist is empty, but whitelist mode is on, so no moons are valid
            }
            if (mEvent.Whitelist.Contains(currentMoonPlanetName, StringComparer.OrdinalIgnoreCase) || mEvent.Blacklist.Contains(currentMoonPlanetName, StringComparer.OrdinalIgnoreCase) || mEvent.Blacklist.Contains(currentMoonName, StringComparer.OrdinalIgnoreCase) || mEvent.Blacklist.Contains(currentMoonNameNoNumbers, StringComparer.OrdinalIgnoreCase))
            {
                Log.LogInfo($"Event {mEvent.Name()} is chosen due to moon whitelist.");
                return true; // Event is on whitelist, and valid moon
            }
            return false; // Fallback
        }

        /// <summary>
        /// Compute the given heat for a moon by its level ID.
        /// </summary>
        /// <returns></returns>
        public static float currentHeatDifficulty()
        {
            float heatValue = 0f;
            if (RoundManager.Instance?.currentLevel?.levelID != null)
            {
                int levelID = RoundManager.Instance.currentLevel.levelID;
                Log.LogInfo("Computing heat for level ID" +  levelID);
                heatValue = Manager.heatDifficulty.TryGetValue(levelID, out float heat) ? heat : Configuration.startingHeat.Value;
                Log.LogInfo("Computed Heat is " + heatValue + " for level ID " + levelID);
            }
            return heatValue;
        }

        
        internal static List<MEvent> GetEventsByName(params string[] names)
        {
            HashSet<string> nameSet = new HashSet<string>(names);
            return events.Where(n => nameSet.Contains(n.Name())).ToList();
        }
    }
}