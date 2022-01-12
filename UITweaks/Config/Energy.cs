using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UITweaks.Models;
using UnityEngine;

namespace UITweaks.Config
{
    public class Energy : ConfigBase
    {
        public override bool Enabled { get; set; } = true;

        /// <summary>
        /// If set to <see langword="true"/>, the Energy Bar will play a rainbow animation when you reach full energy.
        /// </summary>
        public virtual bool RainbowOnFullEnergy { get; set; } = false;

        [UseConverter(typeof(HexColorConverter))] public virtual Color Low { get; set; } = Color.red;
        [UseConverter(typeof(HexColorConverter))] public virtual Color Mid { get; set; } = Color.yellow;
        [UseConverter(typeof(HexColorConverter))] public virtual Color High { get; set; } = Color.green;
    }
}
