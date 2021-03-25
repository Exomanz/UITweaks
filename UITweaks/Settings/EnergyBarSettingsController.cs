﻿using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using UnityEngine;
using Zenject;

namespace UITweaks.Settings
{
    [ViewDefinition("UITweaks.Settings.Views.energyBarSettings.bsml")]
    [HotReload(RelativePathToLayout = @"..\Settings\Views\energyBarSettings.bsml")]
    public class EnergyBarSettingsController : BSMLAutomaticViewController
    {
        PluginConfig.EnergyBarConfig _config;

        [Inject]
        public void Construct(PluginConfig.EnergyBarConfig config) => _config = config;

        [UIValue("EnergyBarModEnabled")]
        protected bool EnergyBarModEnabled
        {
            get => _config.EnableBar;
            set
            {
                _config.EnableBar = value;
                NotifyPropertyChanged(nameof(IsModEnabled));
            }
        }

        [UIValue("LowEnergyColor")]
        protected Color LowEnergyColor
        {
            get => _config.LowEnergyColor;
            set => _config.LowEnergyColor = value;
        }

        [UIValue("HighEnergyColor")]
        protected Color HighEnergyColor
        {
            get => _config.HighEnergyColor;
            set => _config.HighEnergyColor = value;
        }

        [UIValue("RainbowAnimOnFull")]
        protected bool RainbowAnimOnFull
        {
            get => _config.RainbowAnimOnFull;
            set => _config.RainbowAnimOnFull = value;
        }

        [UIValue("IsModEnabled")]
        protected bool IsModEnabled
        {
            get { 
                return EnergyBarModEnabled switch {
                    true => true,
                    false => false, };
            }
        }
    }
}
