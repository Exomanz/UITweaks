using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UITweaks.Models;
using UnityEngine;

namespace UITweaks.Config
{
    public class EnergyConfig : UITweaksConfigBase
    {
        public override bool Enabled { get; set; } = true;

        /// <summary>
        /// If set to <see langword="true"/>, the Energy Bar will play a rainbow animation when you reach full energy.
        /// </summary>
        public virtual bool RainbowOnFullEnergy { get; set; } = false;

        /// <summary>
        /// The <see cref="Color"/> of the leftmost anchor point in the energy bar (0%).
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color Low { get; set; } = Color.red;

        /// <summary>
        /// The <see cref="Color"/> of the center anchor point in the energy bar (50%).
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color Mid { get; set; } = Color.yellow;

        /// <summary>
        /// The <see cref="Color"/> of the rightmost anchor point in the energy bar (100%).
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color High { get; set; } = Color.green;
    }
}
