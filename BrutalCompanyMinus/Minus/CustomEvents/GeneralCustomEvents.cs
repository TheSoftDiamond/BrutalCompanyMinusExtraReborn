using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BrutalCompanyMinus.Minus.Handlers.CustomEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.CustomEvents
{
    internal class GeneralCustomEvent : MEvent
    {
        private string name = "0";
        private CustomEventHandling.EventData? eventData;

        public List<HazardEvent> hazardEvents;

        public GeneralCustomEvent(string path)
        {
            eventData = CustomEventHandling.ReadFile(path);
            hazardEvents = new List<HazardEvent>();
        }

        public override string Name() => name;

        public override void Initalize()
        {
            Log.LogInfo("Running Custom Event Initialization");
            if (eventData != null)
            {
                name = eventData.Name;
                Type = eventData.Type;
                Weight = eventData.Weight;
                ColorHex = eventData.Color;
                Descriptions = eventData.Descriptions;
                Enabled = eventData.Enabled;

                SanitizeName();

                if (Descriptions.Count == 0) Descriptions.Add($"Please add a description to {name} event");
                
                if (eventData.Items != null)
                {
                    Scale transmuteAmount = CustomEventHandling.ArrayToScale(eventData.Items.TransmuteAmount);
                    Scale scrapAmount = CustomEventHandling.ArrayToScale(eventData.Items.ScrapAmount);
                    Scale scrapValue = CustomEventHandling.ArrayToScale(eventData.Items.ScrapValue);

                    List<SpawnableItemWithRarity> items = new List<SpawnableItemWithRarity>();

                    foreach (CustomEventHandling.ItemData item in eventData.Items.Items)
                    {
                        items.Add(new SpawnableItemWithRarity() {
                            spawnableItem = Assets.GetItem(item.Name),
                            rarity = item.Rarity
                        });
                    }

                    scrapTransmutationEvent = new ScrapTransmutationEvent(transmuteAmount, items.ToArray());

                    ScaleList.Add(ScaleType.ScrapAmount, scrapAmount);
                    ScaleList.Add(ScaleType.ScrapValue, scrapValue);
                }
                
                if (eventData.Enemies != null && eventData.Enemies.Count > 0)
                {
                    foreach (CustomEventHandling.EnemyData enemy in eventData.Enemies)
                    {
                        Scale insideRarity = CustomEventHandling.ArrayToScale(enemy.InsideRarity);
                        Scale outsideRarity = CustomEventHandling.ArrayToScale(enemy.OutsideRarity);
                        Scale minInside = CustomEventHandling.ArrayToScale(enemy.MinInside);
                        Scale maxInside = CustomEventHandling.ArrayToScale(enemy.MaxInside);
                        Scale minOutside = CustomEventHandling.ArrayToScale(enemy.MinOutside);
                        Scale maxOutside = CustomEventHandling.ArrayToScale(enemy.MaxOutside);

                        monsterEvents.Add(new MonsterEvent(enemy.Name, insideRarity, outsideRarity, minInside, maxInside, minOutside, maxOutside));
                    }
                }

                if (!string.IsNullOrWhiteSpace(eventData.Weather))
                {
                    Handlers.Modded.CustomWeather.SetCustomWeather(eventData.Weather);
                }
                
                if (eventData.Hazards != null && eventData.Hazards.Count > 0)
                {
                    foreach (BaseHazardData hazard in eventData.Hazards)
                    {
                        if (hazard is OutsideHazardData)
                        {
                            OutsideHazardData outsideHazard = (OutsideHazardData) hazard;
                            Scale minDensity = CustomEventHandling.ArrayToScale(outsideHazard.MinDensity);
                            Scale maxDensity = CustomEventHandling.ArrayToScale(outsideHazard.MaxDensity);
                            hazardEvents.Add(new HazardEvent(hazard.PrefabName, minDensity, maxDensity));
                        }
                        else if (hazard is InsideHazardData)
                        {
                            InsideHazardData insideHazard = (InsideHazardData) hazard;
                            Scale minAmount = CustomEventHandling.ArrayToScale(insideHazard.MinSpawn);
                            Scale maxAmount = CustomEventHandling.ArrayToScale(insideHazard.MaxSpawn);

                            hazardEvents.Add(new HazardEvent(hazard.PrefabName, minAmount, maxAmount,
                                insideHazard.SpawnOptions.FacingAwayFromWall,
                                insideHazard.SpawnOptions.FacingWall,
                                insideHazard.SpawnOptions.BackToWall,
                                insideHazard.SpawnOptions.BackFlushWithWall,
                                insideHazard.SpawnOptions.RequireDistanceBetween,
                                insideHazard.SpawnOptions.DisallowNearEntrances
                            ));
                        }
                    }
                }
                
                if (eventData.EventsToRemove != null && eventData.EventsToRemove.Count > 0)
                {
                    EventsToRemove = eventData.EventsToRemove;
                }
                
                if (eventData.EventsToSpawnWith != null && eventData.EventsToSpawnWith.Count > 0)
                {
                    EventsToSpawnWith = eventData.EventsToSpawnWith;
                }

                Log.LogInfo($"{name} event initialized");
            }
            else
            {
                Enabled = false;
                Log.LogInfo("No event data received. Check if your event files are valid JSON");
            }
        }

        public override bool AddEventIfOnly()
        {
            if (eventData == null) return false; // Force event off if it's missing data

            // Check event's requirements
            if (eventData.AddEventIfOnly != null && eventData.AddEventIfOnly.Count > 0)
            {
                foreach (string evt in eventData.AddEventIfOnly)
                {
                    Log.LogInfo($"{name} contains {evt}: {Chainloader.PluginInfos.ContainsKey(evt)}");
                    if (!Chainloader.PluginInfos.ContainsKey(evt)) return false;
                }
            }

            // Check if other item transmutations are active if this event adds items
            if (eventData.Items != null)
            {
                if (Manager.transmuteScrap) return false;

                Manager.transmuteScrap = true;
            }

            return true;
        }


        public override void Execute()
        {
            //Enemy
            ExecuteAllMonsterEvents();

            //Items
            if (eventData.Items != null)
            {
                Manager.scrapAmountMultiplier *= Getf(ScaleType.ScrapAmount);
                scrapTransmutationEvent.Execute();
            }

            //Hazards
            foreach (HazardEvent hazard in hazardEvents)
            {
                hazard.Execute();
            }
        }

        /// <summary>
        /// Removes invalid characters from the name of the event.
        /// </summary>
        private void SanitizeName()
        {
            //This is done because BepinEx Configurations do not allow these special characters
            string origName = name;
            name = name.Replace("\n", "")
                .Replace("\t", "")
                .Replace("\\", "")
                .Replace("\"", "")
                .Replace("\'", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace(" ", "");

            if (name != origName)
            {
                Log.LogWarning($"{origName} has been renamed to {name} due to invalid characters");
            }
        }

        /// <summary>
        /// This is used to describe a basic Hazard event.
        /// </summary>
        public class HazardEvent : MEvent
        {
            private bool isInside;

            public GameObject hazardObject;

            public Scale minDensity, maxDensity;

            public bool facingAwayFromWall, facingWall, backToWall, backFlushWithWall, requireDistanceBetween, disallowNearEntrances;

            public HazardEvent(GameObject hazardObject, Scale minDensity, Scale maxDensity)
            {
                this.isInside = false;
                this.hazardObject = hazardObject;
                AssignSpawnParameters(minDensity, maxDensity);

                ScaleList.Add(ScaleType.MinDensity, minDensity);
                ScaleList.Add(ScaleType.MaxDensity, maxDensity);
            }

            public HazardEvent(Assets.ObjectName hazardName, Scale minDensity, Scale maxDensity)
            {
                this.isInside = false;
                this.hazardObject = Assets.GetObject(hazardName);
                AssignSpawnParameters(minDensity, maxDensity);

                ScaleList.Add(ScaleType.MinDensity, minDensity);
                ScaleList.Add(ScaleType.MaxDensity, maxDensity);
            }

            public HazardEvent(string hazardName, Scale minDensity, Scale maxDensity)
            {
                this.isInside = false;
                this.hazardObject = Assets.GetObject(hazardName);
                AssignSpawnParameters(minDensity, maxDensity);

                ScaleList.Add(ScaleType.MinDensity, minDensity);
                ScaleList.Add(ScaleType.MaxDensity, maxDensity);
            }

            public HazardEvent(GameObject hazardObject, Scale minAmount, Scale maxAmount, bool facingAwayFromWall, bool facingWall, bool backToWall, bool backFlushWithWall, bool requireDistanceBetween, bool disallowNearEntrances)
            {
                this.isInside = true;
                this.hazardObject = hazardObject;
                AssignSpawnParameters(minAmount, maxAmount, facingAwayFromWall, facingWall, backToWall, backFlushWithWall, requireDistanceBetween, disallowNearEntrances);

                ScaleList.Add(ScaleType.MinAmount, minAmount);
                ScaleList.Add(ScaleType.MaxAmount, maxAmount);
            }

            public HazardEvent(Assets.ObjectName hazardName, Scale minAmount, Scale maxAmount, bool facingAwayFromWall, bool facingWall, bool backToWall, bool backFlushWithWall, bool requireDistanceBetween, bool disallowNearEntrances)
            {
                this.isInside = true;
                this.hazardObject = Assets.GetObject(hazardName);
                AssignSpawnParameters(minAmount, maxAmount, facingAwayFromWall, facingWall, backToWall, backFlushWithWall, requireDistanceBetween, disallowNearEntrances);

                ScaleList.Add(ScaleType.MinAmount, minAmount);
                ScaleList.Add(ScaleType.MaxAmount, maxAmount);
            }

            public HazardEvent(string hazardName, Scale minAmount, Scale maxAmount, bool facingAwayFromWall, bool facingWall, bool backToWall, bool backFlushWithWall, bool requireDistanceBetween, bool disallowNearEntrances)
            {
                this.isInside = true;
                this.hazardObject = Assets.GetObject(hazardName);
                AssignSpawnParameters(minAmount, maxAmount, facingAwayFromWall, facingWall, backToWall, backFlushWithWall, requireDistanceBetween, disallowNearEntrances);

                ScaleList.Add(ScaleType.MinAmount, minAmount);
                ScaleList.Add(ScaleType.MaxAmount, maxAmount);
            }

            private void AssignSpawnParameters(Scale minDensity, Scale maxDensity)
            {
                this.minDensity = minDensity;
                this.maxDensity = maxDensity;
            }

            private void AssignSpawnParameters(Scale minAmount, Scale maxAmount, bool facingAwayFromWall, bool facingWall, bool backToWall, bool backFlushWithWall, bool requireDistanceBetween, bool disallowNearEntrances)
            {
                this.minDensity = minAmount;
                this.maxDensity = maxAmount;
                this.facingWall = facingWall;
                this.facingAwayFromWall = facingAwayFromWall;
                this.backToWall = backToWall;
                this.backFlushWithWall = backFlushWithWall;
                this.requireDistanceBetween = requireDistanceBetween;
                this.disallowNearEntrances = disallowNearEntrances;
            }
            
            public void Execute()
            {
                if (isInside)
                {
                    // Spawn inside
                    RoundManager.Instance.currentLevel.spawnableMapObjects = RoundManager.Instance.currentLevel.spawnableMapObjects.Add(new SpawnableMapObject()
                    {
                        prefabToSpawn = hazardObject,
                        numberToSpawn = new AnimationCurve(new Keyframe(0f, Get(ScaleType.MinAmount)), new Keyframe(1f, Get(ScaleType.MaxAmount))),
                        spawnFacingAwayFromWall = this.facingAwayFromWall,
                        spawnFacingWall = this.facingWall,
                        spawnWithBackToWall = this.backToWall,
                        spawnWithBackFlushAgainstWall = this.backFlushWithWall,
                        requireDistanceBetweenSpawns = this.requireDistanceBetween,
                        disallowSpawningNearEntrances = this.disallowNearEntrances
                    });
                }
                else
                {
                    // Spawn outside
                    Manager.insideObjectsToSpawnOutside.Add(new Manager.ObjectInfo(hazardObject, UnityEngine.Random.Range(Getf(ScaleType.MinDensity), Getf(ScaleType.MaxDensity))));
                }
            }
        }
    }
}
