using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using SiraUtil.Tools;
using System;
using System.Collections.Generic;
using TMPro;
using UITweaks.Configuration;
using UnityEngine;
using Zenject;

namespace UITweaks.UI
{
    [ViewDefinition("UITweaks.Views.main.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\main.bsml")]
    public class ModSettingsViewController : BSMLAutomaticViewController
    {
#pragma warning disable CS0169, CS0649
        [Inject] private MultiplierConfig multiplier;
        [Inject] private EnergyConfig energy;
        [Inject] private ComboConfig combo;
        [Inject] private ProgressConfig progress;
        [Inject] private PositionConfig position;
        [Inject] private MiscConfig misc;
        [Inject] private SiraLog log;

        public float energyFillAmount = 0.01f;
        public float progressFillAmount = 0.01f;
        public int selectedTab = 0;
        public event Action<int> visibilityEvent;

        // This field shows information about the selected DisplayType for the Progress colorer.
        [UIComponent("type-text")] TextMeshProUGUI typeText;

        [UIAction("update-tab")] internal void UpdateTab(SegmentedControl _, int tab)
        {
            selectedTab = tab;
            this.visibilityEvent(tab);
        }

        [UIAction("update-energy-fill")] internal void UpdateEnergyFill(float fill) => energyFillAmount = fill;
        [UIAction("update-progress-fill")] internal void UpdateProgressFill(float fill) => progressFillAmount = fill;

        [UIValue("display-type-dropdown")]
        protected List<object> typeList => new List<object>()
        {
            "Original",
            "Lerp",
        };

        public void Trigger()
        {
            if (visibilityEvent == null) return;
            this.visibilityEvent(selectedTab);
        }

        #region Multiplier Config
        [UIValue("multi-enabled")] protected bool MultiEnabled
        {
            get => multiplier.Enabled;
            set
            {
                multiplier.Enabled = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(CanEdit8x));
            }
        }

        [UIValue("rainbow-on-8x")] protected bool RainbowOn8x
        {
            get => multiplier.Rainbow8x;
            set
            {
                multiplier.Rainbow8x = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(CanEdit8x));
            }
        }

        [UIValue("one")] protected Color One
        {
            get => multiplier.One;
            set => multiplier.One = value;
        }

        [UIValue("two")] protected Color Two
        {
            get => multiplier.Two;
            set => multiplier.Two = value;
        }

        [UIValue("four")] protected Color Four
        {
            get => multiplier.Four;
            set => multiplier.Four = value;
        }

        [UIValue("eight")] protected Color Eight
        {
            get => multiplier.Eight;
            set => multiplier.Eight = value;
        }

        [UIValue("can-edit-8x")] protected bool CanEdit8x
        {
            get
            {
                if (MultiEnabled)
                {
                    if (!RainbowOn8x) return true;
                    else return false;
                }
                else return false;
            }
        }
        #endregion

        #region Energy Bar Config
        [UIValue("energy-enabled")] protected bool EnergyEnabled
        {
            get => energy.Enabled;
            set
            {
                energy.Enabled = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("rainbow-on-full")] protected bool RainbowOnFull
        {
            get => energy.RainbowFull;
            set => energy.RainbowFull = value;
        }

        [UIValue("low")] protected Color Low
        {
            get => energy.Low;
            set => energy.Low = value;
        }

        [UIValue("mid")] protected Color Mid
        {
            get => energy.Mid;
            set => energy.Mid = value;
        }

        [UIValue("high")] protected Color High
        {
            get => energy.High;
            set => energy.High = value;
        }
        #endregion

        #region Combo Lines Config
        [UIValue("combo-enabled")] protected bool ComboEnabled
        {
            get => combo.Enabled;
            set
            {
                combo.Enabled = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(GradientButtonInteractable));
            }
        }

        [UIValue("top-line")] protected Color TopLine
        {
            get => combo.TopLine;
            set => combo.TopLine = value;
        }

        [UIValue("bottom-line")] protected Color BottomLine
        {
            get => combo.BottomLine;
            set => combo.BottomLine = value;
        }

        [UIValue("use-gradient")] protected bool UseGradient
        {
            get => combo.UseGradient;
            set
            {
                combo.UseGradient = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(GradientButtonInteractable));
            }
        }

        [UIValue("gradient-button-interactable")] protected bool GradientButtonInteractable
        {
            get
            {
                if (ComboEnabled)
                {
                    if (UseGradient) return true;
                    else return false;
                }
                else return false;
            }
        }

        [UIValue("top-line-left")] protected Color TopLineLeft
        {
            get => combo.TopLeft;
            set => combo.TopLeft = value;
        }

        [UIValue("top-line-right")] protected Color TopLineRight
        {
            get => combo.TopRight;
            set => combo.TopRight = value;
        }

        [UIValue("mirror-top-line")] protected bool MirrorTopLine
        {
            get => combo.MirrorOnBottom;
            set
            {
                combo.MirrorOnBottom = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(InvertMTL));
            }
        }

        [UIValue("mirror-top-line-invert")] protected bool InvertMTL
        {
            get
            {
                if (MirrorTopLine) return false;
                else return true;
            }
        }

        [UIValue("bottom-line-left")] protected Color BottomLineLeft
        {
            get => combo.BottomLeft;
            set => combo.BottomLeft = value;
        }

        [UIValue("bottom-line-right")] protected Color BottomLineRight
        {
            get => combo.BottomRight;
            set => combo.BottomRight = value;
        }
        #endregion

        #region Progress Bar Config
        [UIValue("progress-enabled")] protected bool ProgressEnabled
        {
            get => progress.Enabled;
            set
            {
                progress.Enabled = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("display-type")] protected string DisplayType
        {
            get => progress.DisplayType;
            set
            {
                progress.DisplayType = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(OriginalType));
                NotifyPropertyChanged(nameof(LerpType));
                NotifyPropertyChanged(nameof(CanUsePreview));
            }
        }

        [UIValue("original-type")] protected bool OriginalType
        {
            get
            {
                if (DisplayType == "Original")
                {
                    typeText.text = "[Original mode uses one static fill color throughout the entire song.]";
                    return true;
                }
                else return false;
            }
        }
        [UIValue("lerp-type")] protected bool LerpType
        {
            get
            {
                if (DisplayType == "Lerp")
                {
                    typeText.text = "[Lerp mode will fade between two fill colors as the song progresses.]";
                    return true;
                }
                else return false;
            }
        }

        [UIValue("can-use-preview")] protected bool CanUsePreview
        {
            get
            {
                if (LerpType) return true;
                else return false;
            }
        }

        [UIValue("fill-color")] protected Color FillColor
        {
            get => progress.Fill;
            set => progress.Fill = value;
        }

        [UIValue("handle-color")] protected Color HandleColor
        {
            get => progress.Handle;
            set => progress.Handle = value;
        }

        [UIValue("bg-color")] protected Color BGColor
        {
            get => progress.BG;
            set => progress.BG = value;
        }

        [UIValue("song-start-color")] protected Color SongStartColor
        {
            get => progress.StartColor;
            set => progress.StartColor = value;
        }

        [UIValue("song-end-color")] protected Color SongEndColor
        {
            get => progress.EndColor;
            set => progress.EndColor = value;
        }
        #endregion

        #region Position Panel Config
        [UIValue("position-enabled")] protected bool PositionEnabled
        {
            get => position.Enabled;
            set
            {
                position.Enabled = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("hide-1st-place")] protected bool HideFirstPlace
        {
            get => position.HideFirstPlace;
            set => position.HideFirstPlace = value;
        }

        [UIValue("1st-place")] protected Color First 
        {
            get => position.First;
            set => position.First = value;
        }

        [UIValue("2nd-place")] protected Color Second
        {
            get => position.Second;
            set => position.Second = value;
        }

        [UIValue("3rd-place")] protected Color Third
        {
            get => position.Third;
            set => position.Third = value;
        }

        [UIValue("4th-place")] protected Color Fourth
        {
            get => position.Fourth;
            set => position.Fourth = value;
        } 

        [UIValue("5th-place")] protected Color Fifth
        {
            get => position.Fifth;
            set => position.Fifth = value;
        }
        #endregion

        #region Misc Config
        [UIValue("rank-panel-italics")]
        protected bool RankPanelItalics
        {
            get => misc.RestoreRankPanelItalics;
            set => misc.RestoreRankPanelItalics = value;
        }
        #endregion
    }
#pragma warning restore CS0169, CS0649
}
