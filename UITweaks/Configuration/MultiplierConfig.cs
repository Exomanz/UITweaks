using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UnityEngine;

namespace UITweaks.Configuration
{
    public class MultiplierConfig
    {
        public virtual bool Enabled { get; set; } = true;
        public virtual bool Rainbow8x { get; set; } = true;

        [UseConverter(typeof(HexColorConverter))] public virtual Color One { get; set; } = Color.red;
        [UseConverter(typeof(HexColorConverter))] public virtual Color Two { get; set; } = Color.yellow;
        [UseConverter(typeof(HexColorConverter))] public virtual Color Four { get; set; } = Color.green;
        [UseConverter(typeof(HexColorConverter))] public virtual Color Eight { get; set; } = Color.cyan;
    }
}
