using Newtonsoft.Json;

namespace BrutalCompanyMinus.Minus.Handlers.CustomEvents
{
    internal class CustomEventSerializer : JsonSerializerSettings
    {
        public CustomEventSerializer()
        {
            this.NullValueHandling = NullValueHandling.Ignore;
            Converters.Add(new EventTypeConverter());
            Converters.Add(new HazardConverter());
        }
    }
}