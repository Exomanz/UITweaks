using Hive.Versioning;
using IPA.Config.Stores.Attributes;
using UITweaks.Configuration;

namespace UITweaks
{
    public class MainConfig
    {
        [Ignore] public virtual bool AllowPreviews { get; set; } = true;

        [NonNullable] public virtual MultiplierConfig MultiConfig { get; set; } = new MultiplierConfig();
        [NonNullable] public virtual EnergyConfig EnergyConfig { get; set; } = new EnergyConfig();
        [NonNullable] public virtual ComboConfig ComboConfig { get; set; } = new ComboConfig();
        [NonNullable] public virtual ProgressConfig ProgressConfig { get; set; } = new ProgressConfig();
        [NonNullable] public virtual PositionConfig PositionConfig { get; set; } = new PositionConfig();

        public enum ProgressDisplayType
        {
            Original = 0,
            Lerp = 1,
        };
    }
}
