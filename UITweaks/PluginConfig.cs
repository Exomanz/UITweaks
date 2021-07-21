using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UnityEngine;

namespace UITweaks
{
    public class PluginConfig
    {
        [NonNullable] public virtual MultiplierConfig Multiplier { get; set; } = new MultiplierConfig();
        [NonNullable] public virtual EnergyBarConfig EnergyBar { get; set; } = new EnergyBarConfig();
        [NonNullable] public virtual ComboConfig Combo { get; set; } = new ComboConfig();
        [NonNullable] public virtual PositionConfig Position { get; set; } = new PositionConfig();
        [NonNullable] public virtual ProgressConfig Progress { get; set; } = new ProgressConfig();

        public class MultiplierConfig
        {
            public virtual bool Enabled { get; set; } = true;
            public virtual bool RainbowAnim { get; set; } = true;

            [UseConverter(typeof(HexColorConverter))] public virtual Color Color1 { get; set; } = Color.white;
            [UseConverter(typeof(HexColorConverter))] public virtual Color Color2 { get; set; } = Color.yellow;
            [UseConverter(typeof(HexColorConverter))] public virtual Color Color4 { get; set; } = Color.green;
            [UseConverter(typeof(HexColorConverter))] public virtual Color Color8 { get; set; } = Color.cyan;
        }

        public class EnergyBarConfig
        {
            public virtual bool Enabled { get; set; } = true;
            public virtual bool RainbowAnim { get; set; } = false;

            [UseConverter(typeof(HexColorConverter))] public virtual Color LowEnergyColor { get; set; } = Color.red;
            [UseConverter(typeof(HexColorConverter))] public virtual Color MiddleEnergyColor { get; set; } = Color.white;
            [UseConverter(typeof(HexColorConverter))] public virtual Color HighEnergyColor { get; set; } = Color.green;
        }

        public class ComboConfig
        {
            public virtual bool Enabled { get; set; } = true;
            [UseConverter(typeof(HexColorConverter))] public virtual Color T_Color { get; set; } = Color.white;
            [UseConverter(typeof(HexColorConverter))] public virtual Color B_Color { get; set; } = Color.white;

            public virtual bool GradientLines { get; set; } = true;
            public virtual bool SeparateLineColors { get; set; } = false;
            [UseConverter(typeof(HexColorConverter))] public virtual Color T_GradientColor0 { get; set; } = Color.yellow;
            [UseConverter(typeof(HexColorConverter))] public virtual Color T_GradientColor1 { get; set; } = new Color(1f, 1f, 0.5f);
            [UseConverter(typeof(HexColorConverter))] public virtual Color B_GradientColor0 { get; set; } = Color.yellow;
            [UseConverter(typeof(HexColorConverter))] public virtual Color B_GradientColor1 { get; set; } = new Color(1f, 1f, 0.5f);
        }

        public class ProgressConfig
        {
            public virtual bool Enabled { get; set; } = true;
            [UseConverter(typeof(HexColorConverter))] public virtual Color FillColor { get; set; } = Color.white;
            [UseConverter(typeof(HexColorConverter))] public virtual Color HandleColor { get; set; } = Color.white;
            [UseConverter(typeof(HexColorConverter))] public virtual Color BackgroundColor { get; set; }= new Color(1f, 1f, 1f, 0.25f);
        }

        public class PositionConfig
        {
            public virtual bool Enabled { get; set; } = true;
            [UseConverter(typeof(HexColorConverter))] public virtual Color Fifth { get; set; } = Color.red;
            [UseConverter(typeof(HexColorConverter))] public virtual Color Fourth { get; set; } = new Color(1f, 0.4f, 0f);
            [UseConverter(typeof(HexColorConverter))] public virtual Color Third { get; set; } = Color.yellow;
            [UseConverter(typeof(HexColorConverter))] public virtual Color Second { get; set; } = Color.green;
            [UseConverter(typeof(HexColorConverter))] public virtual Color First { get; set; } = Color.cyan;

            public virtual bool HideFirstPlaceAnimation { get; set; } = false;
        }
    }
}
