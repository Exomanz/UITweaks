﻿using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using System;
using System.Collections.Generic;
using UITweaks.Config;
using UnityEngine;
using Zenject;

namespace UITweaks.UI
{
    [ViewDefinition("UITweaks.Views.ModSettings.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\ModSettings.bsml")]
    public class ModSettingsViewController : BSMLAutomaticViewController
    {
        private PluginConfig pluginConfig;
        private MultiplierConfig multiplierConfig;
        private EnergyConfig energyConfig;
        private ComboConfig comboConfig;
        private ProgressConfig progressConfig;
        private PositionConfig positionConfig;
        private MiscConfig miscConfig;

        [UIValue("fools-toggle-check")] private bool _aprilFoolsToggle => Plugin.APRIL_FOOLS;
        [UIValue("progress-bar-render-modes")] private List<object> _renderModes = new List<object>() { "Fixed", "Fade" };
        [UIComponent("rainbowspeed-slider")] public SliderSetting rainbowSpeedSetting;

        public event Action<string> RankShouldBeUpdatedEvent = delegate { };
        public event Action<int> TabWasChangedEvent = delegate { };
        private int selectedTab = 0;
        private float energyBarFillAmount = 0.01f;
        private float progressBarFillAmount = 0.01f;

        [Inject] internal void Construct(PluginConfig c)
        {
            pluginConfig = c;
            multiplierConfig = c.Multiplier;
            energyConfig = c.Energy;
            comboConfig = c.Combo;
            progressConfig = c.Progress;
            positionConfig = c.Position;
            miscConfig = c.Misc;
        }

        public int SelectedTab
        {
            get => selectedTab;
            set
            {
                if (selectedTab != value)
                    selectedTab = value;
            }
        }

        public float EnergyBarFillAmount
        {
            get => energyBarFillAmount;
            set
            {
                if (energyBarFillAmount != value)
                    energyBarFillAmount = value;
            }
        }

        public float ProgressBarFillAmount
        {
            get => progressBarFillAmount;
            set
            {
                if (progressBarFillAmount != value)
                    progressBarFillAmount = value;
            }
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            if (firstActivation && rainbowSpeedSetting.gameObject != null)
            {
                rainbowSpeedSetting.Slider.valueDidChangeEvent += HandleRainbowSliderValueChanged;
            }
            TabWasChangedEvent.Invoke(SelectedTab);
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
            rainbowSpeedSetting.Slider.valueDidChangeEvent -= HandleRainbowSliderValueChanged;
        }

        [UIAction("update-tab")]
        internal void UpdateTab(SegmentedControl _, int tab)
        {
            SelectedTab = tab;
            TabWasChangedEvent.Invoke(tab);
        }

        [UIAction("update-energy-preview")]
        internal void UpdateEnergyPreview(float fillAmount) => EnergyBarFillAmount = fillAmount;

        [UIAction("update-progress-preview")]
        internal void UpdateProgressPreview(float time) => ProgressBarFillAmount = time;

        [UIAction("switch-rank-text.ss")]
        internal void SwitchRankText_SS() => HandleRankTextChangeRequest("SS");

        [UIAction("switch-rank-text.s")]
        internal void SwitchRankText_S() => HandleRankTextChangeRequest("S");

        [UIAction("switch-rank-text.a")]
        internal void SwitchRankText_A() => HandleRankTextChangeRequest("A");

        [UIAction("switch-rank-text.b")]
        internal void SwitchRankText_B() => HandleRankTextChangeRequest("B");

        [UIAction("switch-rank-text.c")]
        internal void SwitchRankText_C() => HandleRankTextChangeRequest("C");

        [UIAction("switch-rank-text.d")]
        internal void SwitchRankText_D() => HandleRankTextChangeRequest("D");

        [UIAction("switch-rank-text.e")]
        internal void SwitchRankText_E() => HandleRankTextChangeRequest("E");

        private void HandleRankTextChangeRequest(string rankString)
        {
            this.RankShouldBeUpdatedEvent.Invoke(rankString);
        }

        private void HandleRainbowSliderValueChanged(RangeValuesTextSlider _, float value)
        {
            if (RainbowSpeed != value)
                RainbowSpeed = value;
        }

        public void RaiseTabEvent(int raiseWithTab = 0) => TabWasChangedEvent.Invoke(raiseWithTab == SelectedTab ? SelectedTab : raiseWithTab);

#pragma warning disable IDE0051
        #region Notifiable Properties
        [UIValue("invert-rainbow-8x")] private bool _RainbowToggle => !RainbowAnimationOnMax;

        [UIValue("invert-mbl")] private bool _MirrorBottomLineToggle => !MirrorOnBottom;

        [UIValue("invert-gradient")] private bool _GradientToggle => !GradientEnabled;

        [UIValue("complex")] private bool _Complex { get => ComboEnabled && GradientEnabled; }

        [UIValue("displaytype-is-fixed")] private bool _ProgressBarDisplayTypeIsFixed => RenderMode.Equals(ProgressConfig.DisplayType.Fixed.ToString());

        [UIValue("displaytype-is-fade")] private bool _ProgressBarDisplayTypeIsFade => RenderMode.Equals(ProgressConfig.DisplayType.Fade.ToString());

        [UIValue("invert-rainbow-first-place")] private bool _InvertRainbowFirstPlace => !RainbowOnFirstPlace;

        [UIValue("invert-rainbow-ss")] private bool _InvertRainbowSS => !RainbowOnSSRank;
        #endregion

        #region Multiplier Config
        [UIValue("multiplier-enabled")]
        private bool MultiplierEnabled
        {
            get => multiplierConfig.Enabled;
            set
            {
                if (multiplierConfig.Enabled != value)
                {
                    multiplierConfig.Enabled = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [UIValue("rainbow-on-8x")]
        private bool RainbowAnimationOnMax
        {
            get => multiplierConfig.RainbowOnMaxMultiplier;
            set
            {
                if (multiplierConfig.RainbowOnMaxMultiplier != value)
                {
                    multiplierConfig.RainbowOnMaxMultiplier = value;
                    NotifyPropertyChanged(nameof(_RainbowToggle));
                }
            }
        }

        [UIValue("smooth-transition")]
        private bool SmoothTransition
        {
            get => multiplierConfig.SmoothTransition;
            set => multiplierConfig.SmoothTransition = value;
        }

        [UIValue("color-1x")]
        private Color One
        {
            get => multiplierConfig.One;
            set => multiplierConfig.One = value;
        }

        [UIValue("color-2x")]
        private Color Two
        {
            get => multiplierConfig.Two;
            set => multiplierConfig.Two = value;
        }

        [UIValue("color-4x")]
        private Color Four
        {
            get => multiplierConfig.Four;
            set => multiplierConfig.Four = value;
        }

        [UIValue("color-8x")]
        private Color Eight
        {
            get => multiplierConfig.Eight;
            set => multiplierConfig.Eight = value;
        }
        #endregion

        #region Energy Config
        [UIValue("energy-enabled")]
        private bool EnergyEnabled
        {
            get => energyConfig.Enabled;
            set
            {
                if (energyConfig.Enabled != value)
                {
                    energyConfig.Enabled = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [UIValue("rainbow-on-full")]
        private bool RainbowOnFullEnergy
        {
            get => energyConfig.RainbowOnFullEnergy;
            set
            {
                if (energyConfig.RainbowOnFullEnergy != value)
                {
                    energyConfig.RainbowOnFullEnergy = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [UIValue("color-low")]
        private Color LowEnergy
        {
            get => energyConfig.Low;
            set => energyConfig.Low = value;
        }

        [UIValue("color-mid")]
        private Color MidEnergy
        {
            get => energyConfig.Mid;
            set => energyConfig.Mid = value;
        }

        [UIValue("color-high")]
        private Color HighEnergy
        {
            get => energyConfig.High;
            set => energyConfig.High = value;
        }
        #endregion

        #region Combo Config
        [UIValue("combo-enabled")]
        private bool ComboEnabled
        {
            get => comboConfig.Enabled;
            set
            {
                if (comboConfig.Enabled != value)
                {
                    comboConfig.Enabled = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(_Complex));
                }
            }
        }

        [UIValue("color-top-line")]
        private Color TopLine
        {
            get => comboConfig.TopLine;
            set => comboConfig.TopLine = value;
        }

        [UIValue("color-bottom-line")]
        private Color BottomLine
        {
            get => comboConfig.BottomLine;
            set => comboConfig.BottomLine = value;
        }

        [UIValue("gradient-enabled")]
        private bool GradientEnabled
        {
            get => comboConfig.UseGradient;
            set
            {
                if (comboConfig.UseGradient != value)
                {
                    comboConfig.UseGradient = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(_Complex));
                    NotifyPropertyChanged(nameof(_GradientToggle));
                }
            }
        }

        [UIValue("color-top-line-left")]
        private Color TopLeftGradient
        {
            get => comboConfig.TopLeft;
            set => comboConfig.TopLeft = value;
        }

        [UIValue("color-top-line-right")]
        private Color TopRightGradient
        {
            get => comboConfig.TopRight;
            set => comboConfig.TopRight = value;
        }

        [UIValue("mirror-bottom-line")]
        private bool MirrorOnBottom
        {
            get => comboConfig.MirrorBottomLine;
            set
            {
                if (comboConfig.MirrorBottomLine != value)
                {
                    comboConfig.MirrorBottomLine = value;
                    NotifyPropertyChanged(nameof(_MirrorBottomLineToggle));
                }
            }
        }

        [UIValue("color-bottom-line-left")]
        private Color BottomLeftGradient
        {
            get => comboConfig.BottomLeft;
            set => comboConfig.BottomLeft = value;
        }

        [UIValue("color-bottom-line-right")]
        private Color BottomRightGradient
        {
            get => comboConfig.BottomRight;
            set => comboConfig.BottomRight = value;
        }
        #endregion

        #region Progress Config
        [UIValue("progress-enabled")]
        private bool ProgressEnabled
        {
            get => progressConfig.Enabled;
            set
            {
                if (progressConfig.Enabled != value)
                {
                    progressConfig.Enabled = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [UIValue("color-fill")]
        private Color FillColor
        {
            get => progressConfig.Fill;
            set => progressConfig.Fill = value;
        }

        [UIValue("color-bg")]
        private Color BGColor
        {
            get => progressConfig.BG;
            set => progressConfig.BG = value;
        }

        [UIValue("color-handle")]
        private Color HandleColor
        {
            get => progressConfig.Handle;
            set => progressConfig.Handle = value;
        }

        [UIValue("render-mode")]
        private string RenderMode
        {
            get => progressConfig.Mode.ToString();
            set
            {
                if (value != progressConfig.Mode.ToString())
                {
                    ProgressConfig.DisplayType convertedValue = (ProgressConfig.DisplayType)Enum.Parse(typeof(ProgressConfig.DisplayType), value.ToString(), true);
                    progressConfig.Mode = convertedValue;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(_ProgressBarDisplayTypeIsFade));
                    NotifyPropertyChanged(nameof(_ProgressBarDisplayTypeIsFixed));
                }
            }
        }

        [UIValue("color-song-start")]
        private Color SongStartColor
        {
            get => progressConfig.StartColor;
            set => progressConfig.StartColor = value;
        }

        [UIValue("color-song-end")]
        private Color SongEndColor
        {
            get => progressConfig.EndColor;
            set => progressConfig.EndColor = value;
        }
        #endregion

        #region Position Config
        [UIValue("position-enabled")]
        private bool PositionEnabled
        {
            get => positionConfig.Enabled;
            set
            {
                if (positionConfig.Enabled != value)
                {
                    positionConfig.Enabled = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [UIValue("hide-first-place-anim")]
        private bool HideFirstPlace
        {
            get => positionConfig.HideFirstPlaceAnimation;
            set => positionConfig.HideFirstPlaceAnimation = value;
        }

        [UIValue("rainbow-on-first-place")]
        private bool RainbowOnFirstPlace
        {
            get => positionConfig.RainbowOnFirstPlace;
            set
            {
                if (positionConfig.RainbowOnFirstPlace != value)
                {
                    positionConfig.RainbowOnFirstPlace = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(_InvertRainbowFirstPlace));
                }
            }
        }

        [UIValue("use-static-color")]
        private bool UseStaticColor
        {
            get => positionConfig.UseStaticColorForStaticPanel;
            set
            {
                if (positionConfig.UseStaticColorForStaticPanel != value)
                {
                    positionConfig.UseStaticColorForStaticPanel = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [UIValue("color-static-panel")]
        private Color StaticPanelColor
        {
            get => positionConfig.StaticPanelColor;
            set => positionConfig.StaticPanelColor = value;
        }

        [UIValue("color-first-place")]
        private Color FirstPlace
        {
            get => positionConfig.First;
            set => positionConfig.First = value;
        }

        [UIValue("color-second-place")]
        private Color SecondPlace
        {
            get => positionConfig.Second;
            set => positionConfig.Second = value;
        }

        [UIValue("color-third-place")]
        private Color ThirdPlace
        {
            get => positionConfig.Third;
            set => positionConfig.Third = value;
        }

        [UIValue("color-fourth-place")]
        private Color FourthPlace
        {
            get => positionConfig.Fourth;
            set => positionConfig.Fourth = value;
        }

        [UIValue("color-fifth-place")]
        private Color FifthPlace
        {
            get => positionConfig.Fifth;
            set => positionConfig.Fifth = value;
        }
        #endregion

        #region Miscellaneous Config
        [UIValue("rainbow-speed")]
        private float RainbowSpeed
        {
            get => miscConfig.GlobalRainbowSpeed;
            set => miscConfig.GlobalRainbowSpeed = value;
        }

        [UIValue("combo-panel-italics")]
        private bool ComboPanelItalics
        {
            get => miscConfig.ItalicizeComboPanel;
            set => miscConfig.ItalicizeComboPanel = value;
        }

        [UIValue("score-panel-italics")]
        private bool ScorePanelItalics
        {
            get => miscConfig.ItalicizeScore;
            set => miscConfig.ItalicizeScore = value;
        }

        [UIValue("rank-panel-italics")]
        private bool RankPanelItalics
        {
            get => miscConfig.ItalicizeImmediateRank;
            set => miscConfig.ItalicizeImmediateRank = value;
        }

        [UIValue("allow-april-fools")]
        private bool AllowAprilFools
        {
            get => pluginConfig.AllowAprilFools;
            set => pluginConfig.AllowAprilFools = value;
        }

        [UIValue("allow-rank-colors")]
        private bool AllowRankColors
        {
            get => miscConfig.AllowRankColoring;
            set
            {
                if (miscConfig.AllowRankColoring != value)
                {
                    miscConfig.AllowRankColoring = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [UIValue("rainbow-on-ss-rank")]
        private bool RainbowOnSSRank
        {
            get => miscConfig.RainbowOnSSRank;
            set
            {
                if (miscConfig.RainbowOnSSRank != value)
                {
                    miscConfig.RainbowOnSSRank = value;
                    NotifyPropertyChanged(nameof(_InvertRainbowSS));
                    NotifyPropertyChanged();
                }
            }
        }

        [UIValue("rank-ss-color")]
        private Color RankSSColor
        {
            get => miscConfig.RankSSColor;
            set => miscConfig.RankSSColor = value;
        }

        [UIValue("rank-s-color")]
        private Color RankSColor
        {
            get => miscConfig.RankSColor;
            set => miscConfig.RankSColor = value;
        }

        [UIValue("rank-a-color")]
        private Color RankAColor
        {
            get => miscConfig.RankAColor;
            set => miscConfig.RankAColor = value;
        }

        [UIValue("rank-b-color")]
        private Color RankBColor
        {
            get => miscConfig.RankBColor;
            set => miscConfig.RankBColor = value;
        }

        [UIValue("rank-c-color")]
        private Color RankCColor
        {
            get => miscConfig.RankCColor;
            set => miscConfig.RankCColor = value;
        }

        [UIValue("rank-d-color")]
        private Color RankDColor
        {
            get => miscConfig.RankDColor;
            set => miscConfig.RankDColor = value;
        }

        [UIValue("rank-e-color")]
        private Color RankEColor
        {
            get => miscConfig.RankEColor;
            set => miscConfig.RankEColor = value;
        }
        #endregion
#pragma warning restore
    }
}
