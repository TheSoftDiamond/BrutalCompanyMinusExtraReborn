using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using SelfSortingStorage.Cupboard;
using SelfSortingStorage.Utils;
using UnityEngine;
using UnityEngine.InputSystem.HID;

namespace BrutalCompanyMinus.Minus.Handlers.Modded
{
    internal class SelfSortingStorageCompat
    {
        public static int GetSelfSortingStorageScrapValue()
        {
            try
            {
                int sssCount = 0;
                foreach (var items in SmartMemory.Instance.ItemList)
                {
                    foreach (var Item in items)
                    {
                        if (Item.IsValid() && Item.Values.Count > 0)
                        {
                            foreach (var scrapvalue in Item.Values)
                            {
                                if (Configuration.ExtraLogging.Value)
                                {
                                    Log.LogInfo($"Found SelfSortingStorage Item with total scrap value: {scrapvalue}");
                                }
                                sssCount += scrapvalue;
                            }
                        }
                    }
                }

                //Fix
                int sssFixCount = sssFixCountFunc();
                sssCount -= sssFixCount;

                return sssCount;
            }
            catch (Exception)
            {
                Console.WriteLine("SelfSortingStorage types not found.");
                int sssCount = 0;
                return sssCount;
            }
        }

        private static int sssFixCountFunc()
        {
            int totalScrapValue = 0;

            GameObject sssBox = GameObject.Find("SSS_Module(Clone)"); // The Storage Shelf

            if (sssBox != null)
            {
                Log.LogInfo("sssBox Found");
                foreach (Transform shelf in sssBox.transform) // Each shelf spot
                {
                    Log.LogInfo("Shelf Spot Found: " + shelf.name);
                    Transform item = shelf.GetChild(0); // Item in shelf
                    Log.LogInfo("Item Found: " + item.name);

                    if (item.childCount > 0) // If item has children (scan nodes)
                    {
                        foreach (Transform child in item)
                        {
                            if (child.name.ToLower().StartsWith("scannode"))
                            {
                                // Get component ScanNodeProperties and get ScanNodeProperties.scrapValue
                                ScanNodeProperties scanNodeProperties = (ScanNodeProperties)child.GetComponent("ScanNodeProperties");

                                if (scanNodeProperties != null)
                                {
                                    totalScrapValue += (int)scanNodeProperties.scrapValue;
                                }
                            }
                        }
                    }
                }
            }

            return totalScrapValue;
        }
    }
}

