using Newtonsoft.Json;

namespace BrutalCompanyMinus.Minus.Handlers.CustomEvents
{
    internal class InsideHazardData : BaseHazardData
    {
        [JsonProperty("MinSpawn")]
        public int MinSpawn { get; set; }

        [JsonProperty("MaxSpawn")]
        public int MaxSpawn { get; set; }

        [JsonProperty("SpawnOptions")]
        public SpawnOptions_ SpawnOptions { get; set; }

        public InsideHazardData(string prefabName, int minSpawn, int maxSpawn, SpawnOptions_ options) : base(prefabName)
        {
            this.MinSpawn = minSpawn;
            this.MaxSpawn = maxSpawn;
            this.SpawnOptions = options;
        }

        public class SpawnOptions_
        {
            [JsonProperty("FacingAwayFromWall")]
            bool FacingAwayFromWall { get; set; }

            [JsonProperty("FacingWall")]
            bool FacingWall { get; set; }
            
            [JsonProperty("BackToWall")]
            bool BackToWall { get; set; }
            
            [JsonProperty("BackFlushToWall")]
            bool BackFlushWithWall { get; set; }

            [JsonProperty("RequireDistanceBetween")]
            bool RequireDistanceBetween { get; set; }

            [JsonProperty("DisallowNearEntrances")]
            bool DisallowNearEntrances { get; set; }
        }
    }
}