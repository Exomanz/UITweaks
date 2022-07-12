using IPA.Config.Stores.Attributes;
using UITweaks.Models;

namespace UITweaks.Config
{
    public class MiscConfig : ConfigBase
    {
        // This property is serialized despite the fact that you can't disable Misc as a whole--just individual features.
        [Ignore] public override bool Enabled { get; set; } = false;

        /// <summary>
        /// If set to <see langword="true"/>, the "Combo" text and number will be re-italicized.
        /// </summary>
        public virtual bool ItalicizeComboPanel { get; set; } = false;

        /// <summary>
        /// If set to <see langword="true"/>, the score will be re-italicized.
        /// </summary>
        public virtual bool ItalicizeScore { get; set; } = false;

        /// <summary>
        /// If set to <see langword="true"/>, the immediate rank panel will be re-italicized.
        /// </summary>
        public virtual bool ItalicizeImmediateRank { get; set; } = false;
    }
}
