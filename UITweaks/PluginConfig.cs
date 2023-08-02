using IPA.Config.Stores.Attributes;
using UITweaks.Config;

namespace UITweaks
{
    public class PluginConfig
    {
        public virtual bool AllowPreviews { get; set; } = true;
        public virtual bool AllowAprilFools { get; set; } = true;

        [NonNullable] 
        public virtual MultiplierConfig Multiplier { get; set; } = new();

        [NonNullable] 
        public virtual ComboConfig Combo { get; set; } = new();

        [NonNullable] 
        public virtual EnergyConfig Energy { get; set; } = new();

        [NonNullable] 
        public virtual ProgressConfig Progress { get; set; } = new();

        [NonNullable] 
        public virtual PositionConfig Position { get; set; } = new();

        [NonNullable] 
        public virtual MiscConfig Misc { get; set; } = new();
    }
}
