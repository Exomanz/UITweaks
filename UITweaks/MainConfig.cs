using IPA.Config.Stores.Attributes;
using System.Collections.Generic;
using UITweaks.Configuration;

namespace UITweaks
{
    public class MainConfig
    {
        [Ignore] public virtual bool AllowPreviews { get; set; } = true;

        [NonNullable] public virtual MultiplierConfig MultiConfig { get; set; } = new();
        [NonNullable] public virtual EnergyConfig EnergyConfig { get; set; } = new();
        [NonNullable] public virtual ComboConfig ComboConfig { get; set; } = new();
        [NonNullable] public virtual ProgressConfig ProgressConfig { get; set; } = new();
        [NonNullable] public virtual PositionConfig PositionConfig { get; set; } = new();
        [NonNullable] public virtual MiscConfig MiscConfig { get; set; } = new();
    }
}
