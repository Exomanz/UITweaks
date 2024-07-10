using IPA.Config.Stores.Attributes;
using UITweaks.Config;

namespace UITweaks
{
    public class PluginConfig
    {
        public virtual bool AllowAprilFools { get; set; } = true;

        [NonNullable] 
        public virtual MultiplierConfig Multiplier { get; set; } = new MultiplierConfig();

        [NonNullable] 
        public virtual ComboConfig Combo { get; set; } = new ComboConfig();

        [NonNullable] 
        public virtual EnergyConfig Energy { get; set; } = new EnergyConfig();

        [NonNullable] 
        public virtual ProgressConfig Progress { get; set; } = new ProgressConfig();

        [NonNullable] 
        public virtual PositionConfig Position { get; set; } = new PositionConfig();

        [NonNullable] 
        public virtual MiscConfig Misc { get; set; } = new MiscConfig();
    }
}
