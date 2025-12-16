using System;
using System.Collections.Generic;
using System.Text;
using ShipInventoryUpdated.Objects;

namespace BrutalCompanyMinus.Minus.Handlers.Modded
{
    internal class ShipInventoryCompat
    {
        public static int GetShipInventoryScrapValue()
        {
            try
            {
                int siCount = 0;

                //Getting inventory from ship inventory
                foreach (ItemData data in ShipInventoryUpdated.Scripts.Inventory.Items)
                {
                    Item? item = data.GetItem();
                    if (item != null && item.isScrap)
                    {
                        siCount += data.SCRAP_VALUE;
                    }
                }

                return siCount;
            }
            catch (Exception)
            {
                Console.WriteLine("ShipInventory types not found.");
                int siCount = 0;
                return siCount;
            }
        }
    }
}

