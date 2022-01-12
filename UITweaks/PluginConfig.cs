using IPA.Config.Stores.Attributes;
using UITweaks.Config;

namespace UITweaks
{
    public class PluginConfig
    {
        public virtual bool AllowPreviews { get; set; } = true;

        [NonNullable] public virtual Multiplier Multiplier { get; set; } = new();
        [NonNullable] public virtual Combo Combo { get; set; } = new();
        [NonNullable] public virtual Energy Energy { get; set; } = new();
        [NonNullable] public virtual Progress Progress { get; set; } = new();
        [NonNullable] public virtual Position Position { get; set; } = new();
        [NonNullable] public virtual Miscellaneous Misc { get; set; } = new();
    }
}
