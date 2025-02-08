using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace BrutalCompanyMinus.Minus.Handlers.CustomEvents
{
    public class HazardConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(List<BaseHazardData>).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                List<BaseHazardData> hazards = new List<BaseHazardData>();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        JObject jsonObject = JObject.Load(reader);
                        string typeName = jsonObject["Type"].ToString();

                        BaseHazardData? hazard = null;

                        switch (typeName)
                        {
                            case "Inside":
                                hazard = jsonObject.ToObject<InsideHazardData>();
                                break;
                            case "Outside":
                                hazard = jsonObject.ToObject<OutsideHazardData>();
                                break;
                            default:
                                throw new JsonSerializationException("Unknown hazard type: " + typeName);
                        }

                        if (hazard != null)
                        {
                            hazards.Add(hazard);
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndArray)
                    {
                        break;
                    }
                }

                return hazards;
            }
            else
            {
                throw new JsonSerializationException("Expected StartArray for Hazards, but got: " + reader.TokenType);
            }
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
