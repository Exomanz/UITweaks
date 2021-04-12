using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using UnityEngine;
using Zenject;

namespace UITweaks.Settings
{
    [ViewDefinition("UITweaks.Settings.Views.mainSettingsController.bsml")]
    //[HotReload(RelativePathToLayout = @"..\Settings\Views\mainSettingsController.bsml")]
    public class UISettingsController : BSMLAutomaticViewController
    {
        PluginConfig.ComboConfig _comboConfig;
        PluginConfig.EnergyBarConfig _barConfig;
        PluginConfig.MultiplierConfig _multiConfig;

        [Inject]
        public void Construct(PluginConfig.ComboConfig combo, PluginConfig.EnergyBarConfig bar,
            PluginConfig.MultiplierConfig multi)
        {
            _comboConfig = combo;
            _barConfig = bar;
            _multiConfig = multi;
        }

        [UIComponent("tabSelector")]
#pragma warning disable CS0169
        readonly TabSelector tabSelector;
#pragma warning restore CS0169

        #region Multiplier Panel
        protected bool MultiEnabled
        {
            get => _multiConfig.Enabled;
            set
            {
                _multiConfig.Enabled = value;
                NotifyPropertyChanged(nameof(MultiEnabled));
                NotifyPropertyChanged(nameof(MultiRainbowAnim));
            }
        }

        protected Color Color1
        {
            get => _multiConfig.Color1;
            set => _multiConfig.Color1 = value;
        }

        protected Color Color2
        {
            get => _multiConfig.Color2;
            set => _multiConfig.Color2 = value;
        }

        protected Color Color4
        {
            get => _multiConfig.Color4;
            set => _multiConfig.Color4 = value;
        }

        protected Color Color8
        {
            get => _multiConfig.Color8;
            set => _multiConfig.Color8 = value;
        }

        //Honestly some pretty janky shit is happening here. Just to satisfy my OCD :OkayChamp:
        protected bool MultiRainbowAnim
        {
            get
            {
                return _multiConfig.Enabled switch
                {
                    true => _multiConfig.RainbowAnim,
                    false => _multiConfig.RainbowAnim = true,
                };
            }
            set
            {
                _multiConfig.RainbowAnim = value;
                NotifyPropertyChanged(nameof(M_FlipRainbowAnim));
            }
        }

        protected bool M_FlipRainbowAnim
        {
            get {
                return MultiRainbowAnim switch {
                    true => false,
                    false => true, };
            }
        }
        #endregion

        #region Energy Bar
        protected bool BarEnabled
        {
            get => _barConfig.Enabled;
            set
            {
                _barConfig.Enabled = value;
                NotifyPropertyChanged(nameof(BarEnabled));
            }
        }

        protected bool BarRainbowAnim
        {
            get
            {
                return BarEnabled switch
                {
                    true => _barConfig.RainbowAnim,
                    false => _barConfig.RainbowAnim = true,
                };
            }
            set => _barConfig.RainbowAnim = value;
        }

        protected Color HighEnergyColor
        {
            get => _barConfig.HighEnergyColor;
            set => _barConfig.HighEnergyColor = value;
        }

        protected Color LowEnergyColor
        {
            get => _barConfig.LowEnergyColor;
            set => _barConfig.LowEnergyColor = value;
        }
        #endregion

        #region Combo Panel
        protected bool ComboEnabled
        {
            get => _comboConfig.Enabled;
            set
            {
                _comboConfig.Enabled = value;
                NotifyPropertyChanged(nameof(ComboEnabled));
                NotifyPropertyChanged(nameof(GradientLines));
            }
        }

        protected Color NoGradient_TopLine
        {
            get => _comboConfig.T_Color;
            set => _comboConfig.T_Color = value;
        }

        protected Color NoGradient_BottomLine
        {
            get => _comboConfig.B_Color;
            set => _comboConfig.B_Color = value;
        }

        protected bool GradientLines
        {
            get
            {
                return ComboEnabled switch
                {
                    true => _comboConfig.GradientLines,
                    false => _comboConfig.GradientLines = false,
                };
            }
            set
            {
                _comboConfig.GradientLines = value;
                NotifyPropertyChanged(nameof(GradientLines));
            }
        }

        protected Color Gradient_TopLineColor0
        {
            get => _comboConfig.T_GradientColor0;
            set => _comboConfig.T_GradientColor0 = value;
        }

        protected Color Gradient_TopLineColor1
        {
            get => _comboConfig.T_GradientColor1;
            set => _comboConfig.T_GradientColor1 = value;
        }

        protected bool SeparateLines
        {
            get => _comboConfig.SeparateLineColors;
            set
            {
                _comboConfig.SeparateLineColors = value;
                NotifyPropertyChanged(nameof(SeparateLines));
            }
        }

        protected Color Gradient_BottomLineColor0
        {
            get => _comboConfig.B_GradientColor0;
            set => _comboConfig.B_GradientColor0 = value;
        }

        protected Color Gradient_BottomLineColor1
        {
            get => _comboConfig.B_GradientColor1;
            set => _comboConfig.B_GradientColor1 = value;
        }
        #endregion
    }
}
