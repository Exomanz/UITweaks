using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UnityEngine;

namespace UITweaks.Configuration
{
    public class ComboConfig
    {
        public virtual bool Enabled { get; set; } = true;
        [UseConverter(typeof(HexColorConverter))] public virtual Color TopLine { get; set; } = Color.green;
        [UseConverter(typeof(HexColorConverter))] public virtual Color BottomLine { get; set; } = Color.green;

        public virtual bool UseGradient { get; set; } = false;
        public virtual bool MirrorOnBottom { get; set; } = false;
        [UseConverter(typeof(HexColorConverter))] public virtual Color TopLeft { get; set; } = Color.green;
        [UseConverter(typeof(HexColorConverter))] public virtual Color TopRight { get; set; } = Color.cyan;
        [UseConverter(typeof(HexColorConverter))] public virtual Color BottomLeft { get; set; } = Color.green;
        [UseConverter(typeof(HexColorConverter))] public virtual Color BottomRight { get; set; } = Color.cyan;
    }
}
