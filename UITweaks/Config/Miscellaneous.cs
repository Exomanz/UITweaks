using UITweaks.Models;

namespace UITweaks.Config
{
    public class Miscellaneous : ConfigBase
    {
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
