using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UITweaks.Models;
using UnityEngine;

namespace UITweaks.Config
{
    public class MultiplierConfig : UITweaksConfigBase
    {
        public override bool Enabled { get; set; } = true;

        /// <summary>
        /// If set to <see langword="true"/>, the multiplier ring will crossfade between each set <see cref="Color"/> for each stage.
        /// </summary>
        public virtual bool SmoothTransition { get; set; } = false;

        /// <summary>
        /// Setting this to <see langword="true"/> will enable a rainbow animation when your combo reaches 8x.
        /// </summary>
        public virtual bool RainbowOnMaxMultiplier { get; set; } = true;

        /// <summary>
        /// The <see cref="Color"/> of the Multiplier ring when your combo is at 1x.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color One { get; set; } = Color.red;

        /// <summary>
        /// The <see cref="Color"/> of the Multiplier ring when your combo is at 2x.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color Two { get; set; } = Color.yellow;

        /// <summary>
        /// The <see cref="Color"/> of the Multiplier ring when your combo is at 4x.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color Four { get; set; } = Color.green;

        /// <summary>
        /// The <see cref="Color"/> of the Multiplier ring when your combo is at 8x and <see cref="RainbowOnMaxMultiplier"/> is set to <see langword="false"/>.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color Eight { get; set; } = Color.cyan;
    }
}
