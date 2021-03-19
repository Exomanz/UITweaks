using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UnityEngine;

namespace UITweaks
{
    public class PluginConfig
    {
        public virtual bool EnableMod { get; set; } = true;

        [NonNullable] public virtual MultiplierConfig Multiplier { get; set; } = new MultiplierConfig();
        [NonNullable] public virtual EnergyBarConfig EnergyBar { get; set; } = new EnergyBarConfig();
        [NonNullable] public virtual ComboConfig Combo { get; set; } = new ComboConfig();

        public class MultiplierConfig
        {
            public virtual bool EnableMultiplier { get; set; } = true;

            //Multiplier Values (1, 2, and 4)
            [UseConverter(typeof(HexColorConverter))] public virtual Color IColor { get; set; } = Color.white;
            [UseConverter(typeof(HexColorConverter))] public virtual Color IIColor { get; set; } = Color.yellow;
            [UseConverter(typeof(HexColorConverter))] public virtual Color IVColor { get; set; } = Color.green;

            public virtual bool RainbowAnimOnIIX { get; set; } = true;
            [UseConverter(typeof(HexColorConverter))] public virtual Color IIXColor { get; set; } = Color.cyan;
        }

        public class EnergyBarConfig
        {
            public virtual bool EnableBar { get; set; } = true;

            //Low and high energy colors
            [UseConverter(typeof(HexColorConverter))] public virtual Color LowEnergyColor { get; set; } = Color.red;
            [UseConverter(typeof(HexColorConverter))] public virtual Color HighEnergyColor { get; set; } = Color.green;
        }

        public class ComboConfig
        {
            public virtual bool EnableCombo { get; set; } = true;
        }
    }
}
