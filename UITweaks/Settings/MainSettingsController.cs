using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using IPA.Utilities;
using SiraUtil.Tools;
using System.Collections;
using UITweaks.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UITweaks.Settings
{
    [ViewDefinition("UITweaks.Settings.Views.main.bsml")]
    //[HotReload(RelativePathToLayout = @"..\Settings\Views\main.bsml")]
    public class MainSettingsController : BSMLAutomaticViewController
    {
        PluginConfig.ComboConfig _comboConfig;
        PluginConfig.EnergyBarConfig _barConfig;
        PluginConfig.MultiplierConfig _multiConfig;
        PluginConfig.PositionConfig _posConfig;
        PluginConfig.ProgressConfig _progressConfig;
        SiraLog _log;
        public int selectedTab = 0;

        //Multiplier Ring
        CurvedTextMeshPro[] multiplierTextGOs;
        Image[] circles;
        bool isOn8x = false;

        //Energy Bar
        Image energyBar;
        float _fillAmount = 0.01f;
        bool isFilled = false;

        //Combo Panel
        ImageView[] fcLines;
        CurvedTextMeshPro comboText;

        //Progress Bar
        Image[] progressBarImages;

        [Inject]
        public void Construct(PluginConfig.ComboConfig combo, PluginConfig.EnergyBarConfig bar, PluginConfig.MultiplierConfig multi,
            PluginConfig.PositionConfig pos, PluginConfig.ProgressConfig progress, SiraLog log)
        {
            _comboConfig = combo;
            _barConfig = bar;
            _multiConfig = multi;
            _progressConfig = progress;
            _posConfig = pos;
            _log = log;
        }

        internal void LateUpdate()
        {
            if (PanelGrabber.MultiplierPanel || PanelGrabber.EnergyBar || PanelGrabber.ComboPanel || PanelGrabber.ProgressBar/* || PanelGrabber.PositionPanel*/)
            {
                if (selectedTab == 0)
                {
                    PanelGrabber.MultiplierPanel.transform.localPosition = Vector3.zero;

                    if (isOn8x && _multiConfig.RainbowAnim)
                        circles[0].color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1), 1, 1));
                    if (_multiConfig.Enabled) PanelGrabber.MultiplierPanel.SetActive(true);
                    else PanelGrabber.MultiplierPanel.transform.localPosition = new Vector3(0f, -1000f, 0f);
                }
                //POV: You're the multiplier panel and I don't feel like disabling you when I switch tabs
                else PanelGrabber.MultiplierPanel.transform.localPosition = new Vector3(0f, -1000f, 0f);

                if (selectedTab == 1)
                {
                    EnergyBarPreviewObjectHelper();
                    if (isFilled)
                    {
                        if (_barConfig.RainbowAnim)
                            energyBar.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1), 1, 1));
                        else energyBar.color = _barConfig.HighEnergyColor;
                    }
                    if (!_barConfig.Enabled) PanelGrabber.EnergyBar.SetActive(false);
                }
                else PanelGrabber.EnergyBar.SetActive(false);

                if (selectedTab == 2)
                {
                    ComboPanelPreviewObjectHelper();
                    if (!_comboConfig.Enabled) PanelGrabber.ComboPanel.SetActive(false);
                }
                else PanelGrabber.ComboPanel.SetActive(false);

                if (selectedTab == 3)
                {
                    ProgressBarPreviewObjectHelper();
                    if (!_progressConfig.Enabled) PanelGrabber.ProgressBar.SetActive(false);
                }
                else PanelGrabber.ProgressBar.SetActive(false);
            }
        }

#pragma warning disable CS0169
        [UIComponent("tabSelector")]
        TabSelector tabSelector;
#pragma warning restore CS0169

        [UIAction("UpdateSelectedTab")]
        internal void UpdateSelectedTab(SegmentedControl _, int tab) => selectedTab = tab;

        [UIAction("UpdateFillAmount")]
        internal void EnergyBarPreviewFillHelper(float fillAmount) => _fillAmount = fillAmount;

        [UIAction("UpdateComboText")]
        internal void ComboTextPreviewTextHelper(float num) => comboText.text = num.ToString();

        #region Preview Helpers
        public void MultiplierPreviewObjectHelper()
        {
            PanelGrabber.MultiplierPanel.SetActive(true);
            multiplierTextGOs = PanelGrabber.MultiplierPanel.transform.GetComponentsInChildren<CurvedTextMeshPro>();
            circles = PanelGrabber.MultiplierPanel.transform.GetComponentsInChildren<Image>();

            circles[1].fillAmount = 0.5f;
            circles[1].color = _multiConfig.Color1;
            circles[0].color = _multiConfig.Color1.ColorWithAlpha(0.25f);

            StartCoroutine(MultiplierPreviewCoroutine());
        }

        internal IEnumerator MultiplierPreviewCoroutine()
        {
            if (!PanelGrabber.MultiplierPanel) yield break;
            isOn8x = false;
            circles[1].fillAmount = 0.5f;

            circles[1].color = _multiConfig.Color1;
            circles[0].color = _multiConfig.Color1.ColorWithAlpha(0.25f);
            multiplierTextGOs[1].text = "1";
            yield return new WaitForSecondsRealtime(1f);

            circles[1].color = _multiConfig.Color2;
            circles[0].color = _multiConfig.Color2.ColorWithAlpha(0.25f);
            multiplierTextGOs[1].text = "2";
            yield return new WaitForSecondsRealtime(1f);

            circles[1].color = _multiConfig.Color4;
            circles[0].color = _multiConfig.Color4.ColorWithAlpha(0.25f);
            multiplierTextGOs[1].text = "4";
            yield return new WaitForSecondsRealtime(1f);

            isOn8x = true;
            multiplierTextGOs[1].text = "8";
            circles[1].fillAmount = 0f;
            if (!_multiConfig.RainbowAnim)
                circles[0].color = _multiConfig.Color8.ColorWithAlpha(0.25f);
            yield return new WaitForSecondsRealtime(1f);

            yield return MultiplierPreviewCoroutine();
        }

        public void EnergyBarPreviewObjectHelper()
        {
            PanelGrabber.EnergyBar.SetActive(true);
            energyBar = PanelGrabber.EnergyBar.transform.Find("EnergyBarWrapper/EnergyBar").GetComponent<Image>();

            energyBar.rectTransform.anchorMax = new Vector2(_fillAmount, 1f);
            if (_fillAmount < 0.5f) energyBar.color = Color.Lerp(_barConfig.LowEnergyColor, _barConfig.MiddleEnergyColor, _fillAmount * 2);
            if (_fillAmount == 0.5f) energyBar.color = _barConfig.MiddleEnergyColor;
            if (_fillAmount > 0.5f && _fillAmount < 1f) energyBar.color = Color.Lerp(_barConfig.MiddleEnergyColor, _barConfig.HighEnergyColor, (_fillAmount - 0.5f) * 2);
            if (_fillAmount == 1f) isFilled = true;
            else isFilled = false;
        }

        public void ComboPanelPreviewObjectHelper()
        {
            PanelGrabber.ComboPanel.SetActive(true);
            fcLines = PanelGrabber.ComboPanel.transform.GetComponentsInChildren<ImageView>();
            comboText = PanelGrabber.ComboPanel.transform.Find("ComboCanvas/NumText").GetComponent<CurvedTextMeshPro>();
            comboText.text = "0";

            ReflectionUtil.SetField(fcLines[0], "_gradient", true);
            ReflectionUtil.SetField(fcLines[1], "_gradient", true);
            ReflectionUtil.SetField(fcLines[1], "_flipGradientColors", true);

            if (_comboConfig.GradientLines)
            {
                fcLines[0].color0 = _comboConfig.T_GradientColor0;
                fcLines[0].color1 = _comboConfig.T_GradientColor1;
                if (!_comboConfig.SeparateLineColors)
                {
                    fcLines[1].color0 = _comboConfig.T_GradientColor0;
                    fcLines[1].color1 = _comboConfig.T_GradientColor1;
                }
                else
                {
                    fcLines[1].color0 = _comboConfig.B_GradientColor0;
                    fcLines[1].color1 = _comboConfig.B_GradientColor1;
                }
            }
            else
            {
                fcLines[0].color0 = _comboConfig.T_Color;
                fcLines[0].color1 = _comboConfig.T_Color;
                fcLines[1].color0 = _comboConfig.B_Color;
                fcLines[1].color1 = _comboConfig.B_Color;
            }
        }

        public void ProgressBarPreviewObjectHelper()
        {
            PanelGrabber.ProgressBar.SetActive(true);
            progressBarImages = PanelGrabber.ProgressBar.transform.GetComponentsInChildren<Image>();

            progressBarImages[0].rectTransform.anchorMax = new Vector2(0.5f, 1f);
            progressBarImages[2].transform.localPosition = Vector3.zero;

            progressBarImages[0].color = _progressConfig.FillColor;
            progressBarImages[1].color = _progressConfig.BackgroundColor.ColorWithAlpha(0.25f);
            progressBarImages[2].color = _progressConfig.HandleColor;
        }
        #endregion

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

        // Honestly some pretty janky shit is happening here. Just to satisfy my OCD :OkayChamp:
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

        protected Color MiddleEnergyColor
        {
            get => _barConfig.MiddleEnergyColor;
            set => _barConfig.MiddleEnergyColor = value;
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
            get => _comboConfig.GradientLines;
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

        #region Progress Bar
        protected bool ProgressEnabled
        {
            get => _progressConfig.Enabled;
            set
            {
                _progressConfig.Enabled = value;
                NotifyPropertyChanged(nameof(ProgressEnabled));
            }
        }

        protected Color FillColor
        {
            get => _progressConfig.FillColor;
            set => _progressConfig.FillColor = value;
        }

        protected Color HandleColor
        {
            get => _progressConfig.HandleColor;
            set => _progressConfig.HandleColor = value;
        }

        protected Color BackgroundColor
        {
            get => _progressConfig.BackgroundColor;
            set => _progressConfig.BackgroundColor = value;
        }
        #endregion

        #region MP Position
        protected bool PosEnabled
        {
            get => _posConfig.Enabled;
            set
            {
                _posConfig.Enabled = value;
                NotifyPropertyChanged(nameof(PosEnabled));
            }
        }

        protected Color FirstPlace
        {
            get => _posConfig.First;
            set => _posConfig.First = value;
        }

        protected Color SecondPlace
        {
            get => _posConfig.Second;
            set => _posConfig.Second = value;
        }

        protected Color ThirdPlace
        {
            get => _posConfig.Third;
            set => _posConfig.Third = value;
        }

        protected Color FourthPlace
        {
            get => _posConfig.Fourth;
            set => _posConfig.Fourth = value;
        }

        protected Color FifthPlace
        {
            get => _posConfig.Fifth;
            set => _posConfig.Fifth = value;
        }

        protected bool HideFirstPlaceAnim
        {
            get => _posConfig.HideFirstPlaceAnimation;
            set => _posConfig.HideFirstPlaceAnimation = value;
        }
        #endregion
    }
}