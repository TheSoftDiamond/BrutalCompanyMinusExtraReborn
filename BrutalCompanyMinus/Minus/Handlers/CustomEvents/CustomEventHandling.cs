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
        public static EventData? ReadFile(string path)
        {
            // Parse JSON data to an EventData object
            string json = System.IO.File.ReadAllText(path);

            CustomEventSerializer serializer = new CustomEventSerializer();
            List<EventData>? data = null;
            
            try
            {
                data = JsonConvert.DeserializeObject<List<EventData>>(json, serializer);
            }
            catch (JsonReaderException e)
            {
                Log.LogError($"An error occurred while parsing event file: {path} Message: {e.Message}");
                return null;
            }

            if (data != null)
            {
                if (data.Count > 0)
                {
                    Log.LogInfo($"Adding custom event {data[0].Name}");
                    return data[0];
                }
                else
                {
                    Log.LogInfo("File was read, but no events were found");
                }
            }
            else
            {
                Log.LogInfo("No data was read from the file");
            }

            return null;
        }

        public static MEvent.Scale ArrayToScale(float[] array)
        {
            if (array.Length == 4)
            {
                return new MEvent.Scale(array[0], array[1], array[2], array[3]);
            }
            
            throw new ArgumentException($"ArrayToScale expected length of 4, got {array.Length}");
        }

        public static MEvent.Scale ArrayToScale(int[] array)
        {
            if (array.Length == 4)
            {
                return new MEvent.Scale(array[0], array[1], array[2], array[3]);
            }
            
            throw new ArgumentException($"ArrayToScale expected length of 4, got {array.Length}");
        }

        public class EventData
        {
            public string Name;
            public MEvent.EventType Type;
            public int Weight;
            public string Color;
            public List<string> Descriptions;
            public bool Enabled;
            public List<string>? AddEventIfOnly;
            public ItemEventData? Items;
            public List<EnemyData>? Enemies;
            public string? Weather;

            [JsonConverter(typeof(HazardConverter))]
            public List<BaseHazardData>? Hazards;

            public List<string>? EventsToRemove;
            public List<string>? EventsToSpawnWith;
            public List<string>? MoonBlacklist;
        }

        public class ItemEventData
        {
            public float[] TransmuteAmount;
            public float[] ScrapAmount;
            public float[] ScrapValue;
            public List<ItemData> Items;
        }

        public struct ItemData
        {
            public string Name;
            public int Rarity;
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