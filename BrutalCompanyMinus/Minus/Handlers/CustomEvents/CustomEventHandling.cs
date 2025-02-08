using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrutalCompanyMinus.Minus;

namespace BrutalCompanyMinus.Minus.Handlers.CustomEvents
{
    internal class CustomEventHandling
    {
        public void ReadFile(string path)
        {
            // Parse JSON data to an EventData object
            string json = System.IO.File.ReadAllText(path);

            CustomEventSerializer serializer = new CustomEventSerializer();

            List<EventData>? data = JsonConvert.DeserializeObject<List<EventData>>(json, serializer);

            if (data != null)
            {
                if (data.Count > 0)
                {
                    // Make the event here
                    return;
                }
                else
                {
                    Console.WriteLine("File was read, but no events were found");
                }
            }
            else
            {
                Console.WriteLine("No data was read from the file");
            }
        }

        public class EventData
        {
            public string Name;
            public MEvent.EventType Type;
            public int Weight;
            public string Color;
            public List<string> Descriptions;
            public bool Enabled;
            public List<string> AddEventIfOnly;
            public List<ItemData>? Items;
            public List<EnemyData>? Enemies;

            [JsonConverter(typeof(HazardConverter))]
            public List<BaseHazardData>? Hazards;

            public List<string>? EventsToRemove;
            public List<string>? EventsToSpawnWith;
        }

        public struct ItemData
        {
            public string Name;
            public int[] Rarity;
            public float[] ScrapAmount;
            public float[] ScrapValue;
        }

        public struct EnemyData
        {
            public string Name;
            public float[] InsideRarity;
            public float[] OutsideRarity;
            public float[] MinInside;
            public float[] MaxInside;
            public float[] MinOutside;
            public float[] MaxOutside;
        }
    }
}