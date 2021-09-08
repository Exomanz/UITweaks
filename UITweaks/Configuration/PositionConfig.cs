using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UnityEngine;

namespace UITweaks.Configuration
{
    public class PositionConfig
    {
        public virtual bool Enabled { get; set; } = true;
        public virtual bool HideFirstPlace { get; set; } = false;

        [UseConverter(typeof(HexColorConverter))] public virtual Color First { get; set; } = Color.cyan;
        [UseConverter(typeof(HexColorConverter))] public virtual Color Second { get; set; } = Color.green;
        [UseConverter(typeof(HexColorConverter))] public virtual Color Third { get; set; } = Color.yellow;
        [UseConverter(typeof(HexColorConverter))] public virtual Color Fourth { get; set; } = new Color(1f, 0.5f, 0f);
        [UseConverter(typeof(HexColorConverter))] public virtual Color Fifth { get; set; } = Color.red;

    }
}
