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
        }

        public override string Name() => name;

        public override void Initalize()
        {
            if (eventData != null)
            {
                name = eventData.Name;
                Type = eventData.Type;
                Weight = eventData.Weight;
                ColorHex = eventData.Color;
                Descriptions = eventData.Descriptions;
                Enabled = eventData.Enabled;
                
                if (eventData.Items != null)
                {
                    foreach (CustomEventHandling.ItemData item in eventData.Items)
                    {
                        // TODO: Need to add transmutation amount scale to item data to process correctly
                    }
                }
                
                if (eventData.Enemies != null)
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
                
                if (eventData.Hazards != null)
                {
                    foreach (BaseHazardData hazard in eventData.Hazards)
                    {
                        if (hazard is OutsideHazardData)
                        {
                            
                        }
                        else if (hazard is InsideHazardData)
                        {

                        }
                    }
                }
                
                if (eventData.EventsToRemove != null)
                {
                    EventsToRemove = eventData.EventsToRemove;
                }
                
                if (eventData.EventsToSpawnWith != null)
                {
                    EventsToSpawnWith = eventData.EventsToSpawnWith;
                }
            }
        }

        public override bool AddEventIfOnly()
        {
            if (eventData == null) return false; // Force event off if it's missing data

            foreach (string evt in eventData.AddEventIfOnly)
            {
                if (!Chainloader.PluginInfos.ContainsKey(evt)) return false;
            }
            return true;
        }


        public override void Execute()
        {
            //Enemy
            ExecuteAllMonsterEvents();

            // Old Enemy code to remove eventually
            EnemyType enemy = Assets.GetEnemy(enemyName.Value);
            Manager.AddEnemyToPoolWithRarity(ref RoundManager.Instance.currentLevel.Enemies, enemy, Get(ScaleType.InsideEnemyRarity));
            Manager.AddEnemyToPoolWithRarity(ref RoundManager.Instance.currentLevel.OutsideEnemies, enemy, Get(ScaleType.OutsideEnemyRarity));

            Manager.Spawn.OutsideEnemies(enemy, UnityEngine.Random.Range(Get(ScaleType.MinOutsideEnemy), Get(ScaleType.MaxOutsideEnemy) + 1));
            Manager.Spawn.InsideEnemies(enemy, UnityEngine.Random.Range(Get(ScaleType.MinInsideEnemy), Get(ScaleType.MaxInsideEnemy) + 1));

            //Items

            //Hazards
            foreach (HazardEvent hazard in hazardEvents)
            {
                hazard.Execute();
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

            public bool facingAwayFromWall, facingWall, backToWall, backFlushWithWall, requireDisanceBetween, disallowNearEntrances;

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

            public HazardEvent(GameObject hazardObject, bool facingAwayFromWall, bool facingWall, bool backToWall, bool backFlushWithWall, bool requireDisanceBetween, bool disallowNearEntrances)
            {
                this.isInside = true;
                this.hazardObject = hazardObject;
                AssignSpawnParameters(facingAwayFromWall, facingWall, backToWall, backFlushWithWall, requireDisanceBetween, disallowNearEntrances);
            }

            public HazardEvent(Assets.ObjectName hazardName, bool facingAwayFromWall, bool facingWall, bool backToWall, bool backFlushWithWall, bool requireDisanceBetween, bool disallowNearEntrances)
            {
                this.isInside = true;
                this.hazardObject = Assets.GetObject(hazardName);
                AssignSpawnParameters(facingAwayFromWall, facingWall, backToWall, backFlushWithWall, requireDisanceBetween, disallowNearEntrances);
            }

            public HazardEvent(string hazardName, bool facingAwayFromWall, bool facingWall, bool backToWall, bool backFlushWithWall, bool requireDisanceBetween, bool disallowNearEntrances)
            {
                this.isInside = true;
                this.hazardObject = Assets.GetObject(hazardName);
                AssignSpawnParameters(facingAwayFromWall, facingWall, backToWall, backFlushWithWall, requireDisanceBetween, disallowNearEntrances);
            }

            private void AssignSpawnParameters(Scale minDensity, Scale maxDensity)
            {
                this.minDensity = minDensity;
                this.maxDensity = maxDensity;
            }

            private void AssignSpawnParameters(bool facingAwayFromWall, bool facingWall, bool backToWall, bool backFlushWithWall, bool requireDisanceBetween, bool disallowNearEntrances)
            {
                this.facingWall = facingWall;
                this.facingAwayFromWall = facingAwayFromWall;
                this.backToWall = backToWall;
                this.backFlushWithWall = backFlushWithWall;
                this.requireDisanceBetween = requireDisanceBetween;
                this.disallowNearEntrances = disallowNearEntrances;
            }
            
            public void Execute()
            {
                if (isInside)
                {
                    // Spawn inside
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
