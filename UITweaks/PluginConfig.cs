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

        public class MultiplierConfig
        {
            public virtual bool EnableMultiplier { get; set; } = true;

            //Multiplier Values (1, 2, and 4)
            [UseConverter(typeof(HexColorConverter))] public virtual Color Color1 { get; set; } = Color.white;
            [UseConverter(typeof(HexColorConverter))] public virtual Color Color2 { get; set; } = Color.yellow;
            [UseConverter(typeof(HexColorConverter))] public virtual Color Color4 { get; set; } = Color.green;

            public virtual bool RainbowAnimOn8 { get; set; } = true;
            [UseConverter(typeof(HexColorConverter))] public virtual Color Color8 { get; set; } = Color.cyan;
        }

        public class EnergyBarConfig
        {
            public virtual bool EnableBar { get; set; } = true;

            //Low and high energy colors
            [UseConverter(typeof(HexColorConverter))] public virtual Color LowEnergyColor { get; set; } = Color.red;
            [UseConverter(typeof(HexColorConverter))] public virtual Color HighEnergyColor { get; set; } = Color.green;

            public virtual bool RainbowAnimOnFull { get; set; } = false;
        }

        public class ComboConfig
        {
            public virtual bool EnableCombo { get; set; } = true;
            public virtual bool GradientLines { get; set; } = true;

            [UseConverter(typeof(HexColorConverter))]
            public virtual Color T_GradientColor0 { get; set; } = Color.yellow;
            [UseConverter(typeof(HexColorConverter))]
            public virtual Color T_GradientColor1 { get; set; } = new Color(1f, 1f, 0.5f);

            public virtual bool SeparateLineColors { get; set; } = true;

            [UseConverter(typeof(HexColorConverter))]
            public virtual Color B_GradientColor0 { get; set; } = Color.yellow;
            [UseConverter(typeof(HexColorConverter))]
            public virtual Color B_GradientColor1 { get; set; } = new Color(1f, 1f, 0.5f);

            //If Not Gradient
            [UseConverter(typeof(HexColorConverter))]
            public virtual Color T_Color { get; set; } = Color.white;
            [UseConverter(typeof(HexColorConverter))]
            public virtual Color B_Color { get; set; } = Color.white;
        }
    }
}
