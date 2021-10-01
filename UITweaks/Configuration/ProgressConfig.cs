using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UnityEngine;

namespace UITweaks.Configuration
{
    public class ProgressConfig
    {
        public virtual bool Enabled { get; set; } = true;
        [UseConverter(typeof(HexColorConverter))] public virtual Color Fill { get; set; } = Color.white;
        [UseConverter(typeof(HexColorConverter))] public virtual Color Handle { get; set; } = Color.white;
        [UseConverter(typeof(HexColorConverter))] public virtual Color BG { get; set; } = Color.white;
        [UseConverter(typeof(HexColorConverter))] public virtual Color StartColor { get; set; } = Color.red;
        [UseConverter(typeof(HexColorConverter))] public virtual Color EndColor { get; set; } = Color.green;
        public virtual string DisplayType { get; set; } = "Original";
    }
}
