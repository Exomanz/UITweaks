using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using SiraUtil.Logging;
using System;
using UITweaks.Config;
using UnityEngine;
using Zenject;

namespace UITweaks.UI
{
    /// <summary>
    /// This class hosts all of the Config properties.
    /// </summary>
    [ViewDefinition("UITweaks.Views.ModSettings.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\ModSettings.bsml")]
    public class ModSettingsViewController : BSMLAutomaticViewController
    {
        public int SelectedTab = 0;
        public event Action<int> TabWasChangedEvent;
        public float EnergyBarFillAmount = 0.01f;
        public float ProgressBarPreviewFillAmount = 0.01f;

        private PluginConfig Config = null!;
        private Multiplier MultiplierConfig = null!;
        private Energy EnergyConfig = null!;
        private Combo ComboConfig = null!;
        private Progress ProgressConfig = null!;
        private Position PositionConfig = null!;
        private Miscellaneous MiscConfig = null!;
        private SiraLog Logger = null!;

        [Inject] internal void Construct(PluginConfig conf, SiraLog l)
        {
            l.Logger.Debug("ModSettingsViewController:Construct()");

            Logger = l;
            Config = conf;
            MultiplierConfig = conf.Multiplier;
            EnergyConfig = conf.Energy;
            ComboConfig = conf.Combo;
            ProgressConfig = conf.Progress;
            PositionConfig = conf.Position;
            MiscConfig = conf.Misc;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            TabWasChangedEvent(SelectedTab);
        }

        [UIAction("update-tab")] internal void UpdateTab(SegmentedControl _, int tab)
        {
            SelectedTab = tab;
            TabWasChangedEvent(tab);
        }

        [UIAction("update-energy-preview")] internal void UpdateEnergyPreview(float fillAmount) => EnergyBarFillAmount = fillAmount;

        [UIAction("update-progress-preview")] internal void UpdateProgressPreview(float time) => ProgressBarPreviewFillAmount = time;

        public void RaiseTabEvent(int raiseWithTab = 0) => TabWasChangedEvent(raiseWithTab == SelectedTab ? SelectedTab : raiseWithTab);

        #region Notifiable Properties
        [UIValue("invert-rainbow")] private bool _RainbowToggle
        {
            get => !RainbowAnimationOnMax;
        }

        [UIValue("invert-mbl")] private bool _MirrorBottomLineToggle
        {
            get => !MirrorOnBottom;
        }

        [UIValue("invert-gradient")] private bool _GradientToggle
        {
            get => !GradientEnabled;
        }

        [UIValue("complex")] private bool _Complex
        {
            get
            {
                if (ComboEnabled)
                {
                    return GradientEnabled ? true : false;
                }
                else return false;
            }
        }

        [UIValue("invert-ufdt")] private bool _FadeDisplayToggle
        {
            get => !UseFadeDisplayType;
        }
        #endregion

        #region Multiplier Config
        [UIValue("multiplier-enabled")] private bool MultiplierEnabled
        {
            get => MultiplierConfig.Enabled;
            set
            {
                MultiplierConfig.Enabled = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("rainbow-on-8x")] private bool RainbowAnimationOnMax
        {
            get => MultiplierConfig.RainbowOnMaxMultiplier;
            set
            {
                MultiplierConfig.RainbowOnMaxMultiplier = value;
                NotifyPropertyChanged(nameof(_RainbowToggle));
            }
        }

        [UIValue("smooth-transition")] private bool SmoothTransition
        {
            get => MultiplierConfig.SmoothTransition;
            set => MultiplierConfig.SmoothTransition = value;
        }

        [UIValue("color-1x")] private Color One
        {
            get => MultiplierConfig.One;
            set => MultiplierConfig.One = value;
        }

        [UIValue("color-2x")] private Color Two
        {
            get => MultiplierConfig.Two;
            set => MultiplierConfig.Two = value;
        }

        [UIValue("color-4x")] private Color Four
        {
            get => MultiplierConfig.Four;
            set => MultiplierConfig.Four = value;
        }

        [UIValue("color-8x")] private Color Eight
        {
            get => MultiplierConfig.Eight;
            set => MultiplierConfig.Eight = value;
        }
        #endregion

        #region Energy Config
        [UIValue("energy-enabled")] private bool EnergyEnabled
        {
            get => EnergyConfig.Enabled;
            set
            {
                EnergyConfig.Enabled = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("rainbow-on-full")] private bool RainbowOnFullEnergy
        {
            get => EnergyConfig.RainbowOnFullEnergy;
            set => EnergyConfig.RainbowOnFullEnergy = value;
        }

        [UIValue("color-low")] private Color LowEnergy
        {
            get => EnergyConfig.Low;
            set => EnergyConfig.Low = value;
        }

        [UIValue("color-mid")] private Color MidEnergy
        {
            get => EnergyConfig.Mid;
            set => EnergyConfig.Mid = value;
        }

        [UIValue("color-high")] private Color HighEnergy
        {
            get => EnergyConfig.High;
            set => EnergyConfig.High = value;
        }
        #endregion

        #region Combo Config
        [UIValue("combo-enabled")] private bool ComboEnabled
        {
            get => ComboConfig.Enabled;
            set
            {
                ComboConfig.Enabled = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(_Complex));
            }
        }

        [UIValue("color-top-line")] private Color TopLine
        {
            get => ComboConfig.TopLine;
            set => ComboConfig.TopLine = value;
        }

        [UIValue("color-bottom-line")] private Color BottomLine
        {
            get => ComboConfig.BottomLine;
            set => ComboConfig.BottomLine = value;
        }

        [UIValue("gradient-enabled")] private bool GradientEnabled
        {
            get => ComboConfig.UseGradient;
            set
            {
                ComboConfig.UseGradient = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(_Complex));
                NotifyPropertyChanged(nameof(_GradientToggle));
            }
        }

        [UIValue("color-top-line-left")] private Color TopLeftGradient
        {
            get => ComboConfig.TopLeft;
            set => ComboConfig.TopLeft = value;
        }

        [UIValue("color-top-line-right")] private Color TopRightGradient
        {
            get => ComboConfig.TopRight;
            set => ComboConfig.TopRight = value;
        }

        [UIValue("mirror-bottom-line")] private bool MirrorOnBottom
        {
            get => ComboConfig.MirrorBottomLine;
            set
            {
                ComboConfig.MirrorBottomLine = value;
                NotifyPropertyChanged(nameof(_MirrorBottomLineToggle));
            }
        }

        [UIValue("color-bottom-line-left")] private Color BottomLeftGradient
        {
            get => ComboConfig.BottomLeft;
            set => ComboConfig.BottomLeft = value;
        }

        [UIValue("color-bottom-line-right")] private Color BottomRightGradient
        {
            get => ComboConfig.BottomRight;
            set => ComboConfig.BottomRight = value;
        }
        #endregion

        #region Progress Config
        [UIValue("progress-enabled")] private bool ProgressEnabled
        {
            get => ProgressConfig.Enabled;
            set
            {
                ProgressConfig.Enabled = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("color-fill")] private Color FillColor
        {
            get => ProgressConfig.Fill;
            set => ProgressConfig.Fill = value;
        }

        [UIValue("color-bg")] private Color BGColor
        {
            get => ProgressConfig.BG;
            set => ProgressConfig.BG = value;
        }

        [UIValue("color-handle")] private Color HandleColor
        {
            get => ProgressConfig.Handle;
            set => ProgressConfig.Handle = value;
        }

        [UIValue("use-fade-display-type")]
        private bool UseFadeDisplayType
        {
            get => ProgressConfig.UseFadeDisplayType;
            set
            {
                ProgressConfig.UseFadeDisplayType = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(_FadeDisplayToggle));
            }
        }

        [UIValue("color-song-start")] private Color SongStartColor
        {
            get => ProgressConfig.StartColor;
            set => ProgressConfig.StartColor = value;
        }

        [UIValue("color-song-end")] private Color SongEndColor
        {
            get => ProgressConfig.EndColor;
            set => ProgressConfig.EndColor = value;
        }
        #endregion

        #region Position Config
        [UIValue("position-enabled")] private bool PositionEnabled
        {
            get => PositionConfig.Enabled;
            set
            {
                PositionConfig.Enabled = value;
                //NotifyPropertyChanged();
            }
        }

        [UIValue("hide-first-place-anim")] private bool HideFirstPlace
        {
            get => PositionConfig.HideFirstPlaceAnimation;
            set => PositionConfig.HideFirstPlaceAnimation = value;
        }

        [UIValue("use-static-color")] private bool UseStaticColor
        {
            get => PositionConfig.UseStaticColorForStaticPanel;
            set => PositionConfig.UseStaticColorForStaticPanel = value;
        }

        [UIValue("color-static-panel")] private Color StaticPanelColor
        {
            get => PositionConfig.StaticPanelColor;
            set => PositionConfig.StaticPanelColor = value;
        }

        [UIValue("color-first-place")] private Color FirstPlace
        {
            get => PositionConfig.First;
            set => PositionConfig.First = value;
        }

        [UIValue("color-second-place")] private Color SecondPlace
        {
            get => PositionConfig.Second;
            set => PositionConfig.Second = value;
        }

        [UIValue("color-third-place")] private Color ThirdPlace
        {
            get => PositionConfig.Third;
            set => PositionConfig.Third = value;
        }

        [UIValue("color-fourth-place")] private Color FourthPlace
        {
            get => PositionConfig.Fourth;
            set => PositionConfig.Fourth = value;
        }

        [UIValue("color-fifth-place")] private Color FifthPlace
        {
            get => PositionConfig.Fifth;
            set => PositionConfig.Fifth = value;
        }
        #endregion

        #region Miscellaneous Config
        [UIValue("legacy-combo-panel")] private bool ComboPanelItalics
        {
            get => MiscConfig.ItalicizeComboPanel;
            set => MiscConfig.ItalicizeComboPanel = value;
        }

        [UIValue("legacy-score-panel")] private bool ScorePanelItalics
        {
            get => MiscConfig.ItalicizeScore;
            set => MiscConfig.ItalicizeScore = value;
        }

        [UIValue("legacy-rank-panel")] private bool RankPanelItalics
        {
            get => MiscConfig.ItalicizeImmediateRank;
            set => MiscConfig.ItalicizeImmediateRank = value;
        }
        #endregion
    }
}
