using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMarkupLanguage.Attributes;
using UnityEngine;
using Zenject;

namespace UITweaks.Settings
{
    [ViewDefinition("UITweaks.Settings.Views.multiplierSettings.bsml")]
    [HotReload(RelativePathToLayout = @"..\Settings\Views\multiplierSettings.bsml")]
    public class MultiplierSettingsController : BSMLAutomaticViewController
    {
        PluginConfig.MultiplierConfig _config;

        [Inject]
        public void Construct(PluginConfig.MultiplierConfig config) => _config = config;

        [UIValue("MultiplierModEnabled")]
        protected bool MultiplierModEnabled
        {
            get => _config.EnableMultiplier;
            set
            {
                _config.EnableMultiplier = value;
                NotifyPropertyChanged(nameof(IsModEnabled));
            }
        }

        [UIValue("1xColor")]
        protected Color IColor
        {
            get => _config.Color1;
            set => _config.Color1 = value;
        }

        [UIValue("2xColor")]
        protected Color IIColor
        {
            get => _config.Color2;
            set => _config.Color2 = value;
        }

        [UIValue("4xColor")]
        protected Color IVColor
        {
            get => _config.Color4;
            set => _config.Color4 = value;
        }

        [UIValue("RainbowAnim")]
        protected bool RainbowAnim
        {
            get => _config.RainbowAnimOn8;
            set
            {
                _config.RainbowAnimOn8 = value;
                NotifyPropertyChanged(nameof(IsRainbowEnabled));
            }
        }

        [UIValue("8xColor")]
        protected Color IIXColor
        {
            get => _config.Color8;
            set => _config.Color8 = value;
        }

        [UIValue("IsModEnabled")]
        protected bool IsModEnabled
        {
            get {
                return MultiplierModEnabled switch {
                    true => true,
                    false => false, };
            }
        }

        [UIValue("IsRainbowEnabled")]
        protected bool IsRainbowEnabled
        {
            get {
                return RainbowAnim switch {
                    true => false,
                    false => true, };
            }
        }
    }
}
