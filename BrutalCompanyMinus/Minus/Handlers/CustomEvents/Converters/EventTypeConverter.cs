using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace BrutalCompanyMinus.Minus.Handlers.CustomEvents
{    
    internal class EventTypeConverter : StringEnumConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Implement how to write the EventType to JSON.  Usually just the string value.
            writer.WriteValue(value.ToString()); // Example: Convert enum to string
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            // Implement how to read the EventType from JSON.
            string value = reader.Value?.ToString();

            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (Enum.TryParse(objectType, value, true, out object eventType))
            {
                return eventType;
            }
            
            throw new JsonSerializationException($"Invalid EventType value: {value}");
        }
    }
}