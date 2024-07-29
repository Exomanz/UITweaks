using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UITweaks.Models;
using UnityEngine;

namespace UITweaks.Config
{
    public class MiscConfig : UITweaksConfigBase
    {
        [Ignore] public override bool Enabled { get; set; } = true;

        /// <summary>
        /// Controls the speed of the rainbow effect globally.
        /// </summary>
        public virtual float GlobalRainbowSpeed { get; set; } = 0.5f;

        /// <summary>
        /// If set to <see langword="true"/>, the "Combo" text and number will be italicized.
        /// </summary>
        public virtual bool ItalicizeComboPanel { get; set; } = false;

        /// <summary>
        /// If set to <see langword="true"/>, the score will be italicized.
        /// </summary>
        public virtual bool ItalicizeScore { get; set; } = false;

        /// <summary>
        /// If set to <see langword="true"/>, the immediate rank panel will be italicized.
        /// </summary>
        public virtual bool ItalicizeImmediateRank { get; set; } = false;

        /// <summary>
        /// If set to true, <see langword="true"/>, the rank text (SS, A, etc.) will be colored.
        /// </summary>
        public virtual bool AllowRankColoring { get; set; } = true;

        /// <summary>
        /// If set to <see langword="true"/>, enables the rainbow effect of the rank text while maintaining an "SS" rank.
        /// </summary>
        public virtual bool RainbowOnSSRank { get; set; } = false;

        /// <summary>
        /// The <see cref="Color"/> of the rank text when the rank is "SS".
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color RankSSColor { get; set; } = Color.cyan;

        /// <summary>
        /// The <see cref="Color"/> of the rank text when the rank is "S".
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color RankSColor { get; set; } = Color.green;

        /// <summary>
        /// The <see cref="Color"/> of the rank text when the rank is "A".
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color RankAColor { get; set; } = Color.yellow;

        /// <summary>
        /// The <see cref="Color"/> of the rank text when the rank is "B".
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color RankBColor { get; set; } = new Color(1, 0.66f, 0);

        /// <summary>
        /// The <see cref="Color"/> of the rank text when the rank is "C".
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color RankCColor { get; set; } = new Color(1, 0.33f, 0);

        /// <summary>
        /// The <see cref="Color"/> of the rank text when the rank is "D".
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color RankDColor { get; set; } = Color.red;

        /// <summary>
        /// The <see cref="Color"/> of the rank text when the rank is "E".
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color RankEColor { get; set; } = Color.red;
    }
}
