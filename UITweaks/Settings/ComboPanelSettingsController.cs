using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using UnityEngine;
using Zenject;

namespace UITweaks.Settings
{
    [ViewDefinition("UITweaks.Settings.Views.comboSettings.bsml")]
    [HotReload(RelativePathToLayout = @"..\Settings\Views\comboSettings.bsml")]
    public class ComboPanelSettingsController : BSMLAutomaticViewController
    {
        PluginConfig.ComboConfig _config;

        [Inject]
        public void Construct(PluginConfig.ComboConfig config) => _config = config;

        [UIValue("EnableComboMod")]
        protected bool EnableComboMod
        {
            get => _config.EnableCombo;
            set
            {
                _config.EnableCombo = value;
                NotifyPropertyChanged(nameof(IsModEnabled));
            }
        }

        [UIValue("TopLineColorNoGrad")]
        protected Color TopLineColorNoGrad
        {
            get => _config.T_Color;
            set => _config.T_Color = value;
        }

        [UIValue("BottomLineColorNoGrad")]
        protected Color BottomLineColorNoGrad
        {
            get => _config.B_Color;
            set => _config.B_Color = value;
        }
        #region Gradient
        [UIValue("GradientLines")]
        protected bool GradientLines
        {
            get => _config.GradientLines;
            set
            {
                _config.GradientLines = value;
                NotifyPropertyChanged(nameof(IsGradientEnabled));
            }
        }

        [UIValue("TopLineGradColor0")]
        protected Color TopLineGradColor0
        {
            get => _config.T_GradientColor0;
            set => _config.T_GradientColor0 = value;
        }

        [UIValue("TopLineGradColor1")]
        protected Color TopLineGradColor1
        {
            get => _config.T_GradientColor1;
            set => _config.T_GradientColor1 = value;
        }

        [UIValue("SeparateLineColors")]
        protected bool SeparateLineColors
        {
            get => _config.SeparateLineColors;
            set
            {
                _config.SeparateLineColors = value;
                NotifyPropertyChanged(nameof(OnSeparateLines));
                NotifyPropertyChanged(nameof(OffSeparateLines));
            }
        }

        [UIValue("BottomLineGradColor0")]
        protected Color BottomLineGradColor0
        {
            get => _config.B_GradientColor0;
            set => _config.B_GradientColor0 = value;
        }
        
        [UIValue("BottomLineGradColor1")]
        protected Color BottomLineGradColor1
        {
            get => _config.B_GradientColor1;
            set => _config.B_GradientColor1 = value;
        }
        #endregion

        #region Switch Properties
        protected bool IsModEnabled
        {
            get {
                return EnableComboMod switch {
                    true => true,
                    false => false };
            }
        }

        protected bool IsGradientEnabled
        {
            get {
                return GradientLines switch {
                    true => true,
                    false => false };
            }
        }

        protected bool OnSeparateLines
        {
            get {
                return SeparateLineColors switch {
                    true => true,
                    false => false };
            }
        }

        protected bool OffSeparateLines
        {
            get {
                return SeparateLineColors switch {
                    true => false,
                    false => true, };
            }
        }
        #endregion
    }
}
