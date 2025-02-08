using Newtonsoft.Json;

namespace BrutalCompanyMinus.Minus.Handlers.CustomEvents
{
    internal class OutsideHazardData : BaseHazardData
    {
        [JsonProperty("MinDensity")]
        public float[] MinDensity { get; set; }

        [JsonProperty("MaxDensity")]
        public float[] MaxDensity { get; set; }

        public OutsideHazardData(string prefabName, float[] minDensity, float[] maxDensity) : base(prefabName)
        {
            this.MinDensity = minDensity;
            this.MaxDensity = maxDensity;
        }
    }
}