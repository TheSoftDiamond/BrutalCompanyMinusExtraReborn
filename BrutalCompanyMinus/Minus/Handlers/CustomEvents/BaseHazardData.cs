namespace BrutalCompanyMinus.Minus.Handlers.CustomEvents
{
    public abstract class BaseHazardData
    {
        public string PrefabName { get; set; }

        public BaseHazardData(string prefabName)
        {
            this.PrefabName = prefabName;
        }
    }
}