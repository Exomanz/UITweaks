using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UnityEngine;

namespace UITweaks.Configuration
{
    public class EnergyConfig
    {
        public virtual bool Enabled { get; set; } = true;
        public virtual bool RainbowFull { get; set; } = false;
        [UseConverter(typeof(HexColorConverter))] public virtual Color Low { get; set; } = Color.red;
        [UseConverter(typeof(HexColorConverter))] public virtual Color Mid { get; set; } = Color.yellow;
        [UseConverter(typeof(HexColorConverter))] public virtual Color High { get; set; } = Color.green;
    }
}
