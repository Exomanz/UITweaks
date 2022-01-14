using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using SiraUtil.Logging;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using UITweaks.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UITweaks.UI
{
    /// <summary>
    /// This class hosts every function that controls the object preview panel.
    /// </summary>
    [ViewDefinition("UITweaks.Views.ObjectPreview.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\ObjectPreview.bsml")]
    public class ObjectPreviewViewController : BSMLAutomaticViewController
    {
        private PluginConfig Config = null!;
        private ModSettingsViewController SettingsViewController = null!;
        private SiraLog Logger = null!;
        private SettingsPanelObjectGrabber ObjectGrabber = null!;
        private Vector3 DefaultGrabberPos = new(3.53f, 1.1f, 2.4f);
        private Vector3 Void = new(0, -1000, 0);
        private bool previewToggleIsReady = false;

        #region Preview Objects
        private Image[] multiplierCircles = null!;
        private CurvedTextMeshPro multiplierText = null!;
        private bool previewCoroOn8x = false;

        private Image energyBar = null!;
        private float fillAmount = 0.01f;

        private ImageView[] comboLines = null!;
        private CurvedTextMeshPro numText = null!;
        private CurvedTextMeshPro comboText = null!;

        private Image[] progressPanelImages = null!;

        private CurvedTextMeshPro scoreText = null!;
        private CurvedTextMeshPro percentText = null!;
        private CurvedTextMeshPro rankText = null!;
        private decimal rank = 0.00m;
        #endregion

        [Inject] internal void Construct(PluginConfig c, ModSettingsViewController msvc, SiraLog l)
        {
            l.Logger.Debug("ObjectPreviewViewController:Construct()");

            Logger = l;
            Config = c;
            SettingsViewController = msvc;
        }

        [UIValue("allow-previews")] private bool AllowPreviews
        {
            get => Config.AllowPreviews;
            set
            {
                Config.AllowPreviews = value;
                if (!previewToggleIsReady) return; // Prevent BSML Parsing Error
                UpdatePanelVisibility(value ? SettingsViewController.SelectedTab : -1);
            }
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);

            GameObject grabber = GameObject.Find("UITweaksPanelGrabber")?.gameObject;

            if (firstActivation && !grabber)
            {
                grabber = new GameObject("UITweaksPanelGrabber");
                grabber.transform.position = DefaultGrabberPos;
                grabber.transform.Rotate(0, 57, 0);
                ObjectGrabber = grabber.AddComponent<SettingsPanelObjectGrabber>();
                StartCoroutine(FinalizePanels());
            }

            StartCoroutine(MultiplierPreviewCoroutine());
            grabber.transform.position = DefaultGrabberPos;
            SettingsViewController.TabWasChangedEvent += UpdatePanelVisibility;
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
            SettingsViewController.TabWasChangedEvent -= UpdatePanelVisibility;
            ObjectGrabber.transform.position = Void;
        }

        private IEnumerator FinalizePanels()
        {
            yield return new WaitUntil(() => ObjectGrabber.isCompleted);

            var multiplierPanel = ObjectGrabber.MultiplierPanel;
            var comboPanel = ObjectGrabber.ComboPanel;
            var progressPanel = ObjectGrabber.ProgressPanel;

            try
            {
                ObjectGrabber.MultiplierPanel.SetActive(true);

                // Multiplier Panel Setup
                {
                    multiplierText = multiplierPanel.GetComponentsInChildren<CurvedTextMeshPro>().Last();
                    multiplierCircles = multiplierPanel.transform.GetComponentsInChildren<Image>();

                    multiplierCircles[1].color = Config.Multiplier.One;
                    multiplierCircles[0].color = Config.Multiplier.One.ColorWithAlpha(0.25f);
                }

                // Energy Bar Setup
                energyBar = ObjectGrabber.EnergyPanel.transform.Find("EnergyBarWrapper/EnergyBar")?.GetComponent<Image>();

                // Combo Panel Setup
                {
                    comboLines = comboPanel.transform.GetComponentsInChildren<ImageView>();
                    numText = comboPanel.transform.Find("ComboCanvas/NumText")?.GetComponent<CurvedTextMeshPro>();
                    comboText = comboPanel.transform.Find("ComboText")?.GetComponent<CurvedTextMeshPro>();
                }

                // Progress Panel Setup
                {
                    progressPanelImages = progressPanel.transform.GetComponentsInChildren<Image>();
                    var texts = progressPanel.transform.GetComponentsInChildren<CurvedTextMeshPro>();
                    texts[2].text = "0";
                    texts[3].text = "01";
                }

                // Immediate Rank Panel Setup
                {
                    var immediateRankTransform = ObjectGrabber.ImmediateRankPanel.transform;
                    scoreText = immediateRankTransform.Find("ScoreText")?.GetComponent<CurvedTextMeshPro>();
                    percentText = immediateRankTransform.Find("RelativeScoreText")?.GetComponent<CurvedTextMeshPro>();
                    rankText = immediateRankTransform.Find("ImmediateRankText")?.GetComponent<CurvedTextMeshPro>();

                    immediateRankTransform.localPosition = new Vector3(0.75f, 0, 0);
                }
            }
            catch (Exception ex)
            {
                Logger.Logger.Error(ex);
            }

            previewToggleIsReady = true;
            SettingsViewController.RaiseTabEvent(Config.AllowPreviews ? SettingsViewController.SelectedTab : -1);
            yield break;
        }

        private void UpdatePanelVisibility(int tab)
        {
            if (!ObjectGrabber.isCompleted) return;
            var host = ObjectGrabber;

            if (!Config.AllowPreviews)
            {
                host.MultiplierPanel.transform.position = Void;
                host.EnergyPanel.SetActive(false);
                host.ComboPanel.SetActive(false);
                host.ProgressPanel.SetActive(false);
                host.ImmediateRankPanel.SetActive(false);
                return;
            }

            switch (tab)
            {
                case 0:
                    host.MultiplierPanel.transform.localPosition = Vector3.zero;
                    host.EnergyPanel.SetActive(false);
                    host.ComboPanel.SetActive(false);
                    host.ProgressPanel.SetActive(false);
                    host.ImmediateRankPanel.SetActive(false);
                    break;
                case 1:
                    host.MultiplierPanel.transform.position = Void;
                    host.EnergyPanel.SetActive(true);
                    host.ComboPanel.SetActive(false);
                    host.ProgressPanel.SetActive(false);
                    host.ImmediateRankPanel.SetActive(false);
                    break;
                case 2:
                    host.MultiplierPanel.transform.position = Void;
                    host.EnergyPanel.SetActive(false);
                    host.ComboPanel.SetActive(true);
                    host.ProgressPanel.SetActive(false);
                    host.ImmediateRankPanel.SetActive(false);

                    numText.text = new System.Random().Next(0, 250).ToString();
                    host.ComboPanel.transform.localPosition = Vector3.zero;
                    break;
                case 3:
                    host.MultiplierPanel.transform.position = Void;
                    host.EnergyPanel.SetActive(false);
                    host.ComboPanel.SetActive(false);
                    host.ProgressPanel.SetActive(true);
                    host.ImmediateRankPanel.SetActive(false);
                    break;
                case 4:
                    host.MultiplierPanel.transform.position = Void;
                    host.EnergyPanel.SetActive(false);
                    host.ComboPanel.SetActive(false);
                    host.ProgressPanel.SetActive(false);
                    host.ImmediateRankPanel.SetActive(false);
                    break;
                case 5:
                    host.MultiplierPanel.transform.position = Void;
                    host.EnergyPanel.SetActive(false);
                    host.ComboPanel.SetActive(true);
                    host.ProgressPanel.SetActive(false);
                    host.ImmediateRankPanel.SetActive(true);

                    host.ComboPanel.transform.localPosition = new Vector3(-0.75f, 0, 0);
                    rank = Utilities.Utilities.RandomDecimal(100, 1);
                    numText.text = new System.Random().Next(1, 250).ToString();
                    scoreText.text = new System.Random().Next(1, 999999).ToString();
                    percentText.text = rank.ToString() + "%";
                    break;
                default:
                    break;
            }
        }

        internal void Update()
        {
            if (!ObjectGrabber.isCompleted) return;

            int tab = SettingsViewController.SelectedTab;
            switch (tab)
            {
                case 0:
                    if (previewCoroOn8x && Config.Multiplier.RainbowOnMaxMultiplier)
                        multiplierCircles[0].color = new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1), 1, 1).ToColor();
                    break;
                case 1:
                    UpdateEnergyBar(SettingsViewController.EnergyBarFillAmount);
                    if (fillAmount == 1 && Config.Energy.RainbowOnFullEnergy)
                        energyBar.color = new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1), 1, 1).ToColor();
                    break;
                case 2:
                    UpdateComboPanel();
                    break;
                case 3:
                    UpdateProgressBar(SettingsViewController.ProgressBarPreviewFillAmount);
                    break;
                case 5:
                    UpdateComboPanel();
                    UpdateImmediateRankPanel();
                    break;
            }
        }

        private IEnumerator MultiplierPreviewCoroutine()
        {
            yield return new WaitUntil(() => previewToggleIsReady);
            previewCoroOn8x = false;
            multiplierCircles[1].fillAmount = 0.5f;
            
            // Eventually Add "Smooth Transition" Preview

            {
                multiplierCircles[0].color = Config.Multiplier.One.ColorWithAlpha(0.25f);
                multiplierCircles[1].color = Config.Multiplier.One;
                multiplierText.text = "1";
                yield return new WaitForSecondsRealtime(1);

                multiplierCircles[0].color = Config.Multiplier.Two.ColorWithAlpha(0.25f);
                multiplierCircles[1].color = Config.Multiplier.Two;
                multiplierText.text = "2";
                yield return new WaitForSecondsRealtime(1);

                multiplierCircles[0].color = Config.Multiplier.Four.ColorWithAlpha(0.25f);
                multiplierCircles[1].color = Config.Multiplier.Four;
                multiplierText.text = "4";
                yield return new WaitForSecondsRealtime(1);

                previewCoroOn8x = true;
                multiplierText.text = "8";
                multiplierCircles[1].fillAmount = 0;
                if (!Config.Multiplier.RainbowOnMaxMultiplier)
                    multiplierCircles[0].color = Config.Multiplier.Eight.ColorWithAlpha(0.25f);
                yield return new WaitForSecondsRealtime(1);
            }

            yield return MultiplierPreviewCoroutine();
        }

        private void UpdateEnergyBar(float fillAmount)
        {
            this.fillAmount = fillAmount;
            energyBar.rectTransform.anchorMax = new Vector2(fillAmount, 1);

            if (fillAmount == 0.5f) energyBar.color = Config.Energy.Mid;

            if (fillAmount > 0.5f && fillAmount < 1) energyBar.color = HSBColor.Lerp(
                HSBColor.FromColor(Config.Energy.Mid),
                HSBColor.FromColor(Config.Energy.High),
                (fillAmount - 0.5f) * 2).ToColor();

            if (fillAmount < 0.5f) energyBar.color = HSBColor.Lerp(
                HSBColor.FromColor(Config.Energy.Low),
                HSBColor.FromColor(Config.Energy.Mid),
                fillAmount * 2).ToColor();
        }

        private void UpdateComboPanel()
        {
            if (Config.Combo.UseGradient)
            {
                comboLines[0].gradient = true;
                comboLines[1].gradient = true;

                comboLines[0].color0 = Config.Combo.TopLeft;
                comboLines[0].color1 = Config.Combo.TopRight;

                if (Config.Combo.MirrorBottomLine)
                {
                    comboLines[1].color0 = Config.Combo.TopRight;
                    comboLines[1].color1 = Config.Combo.TopLeft;
                }
                else
                {
                    comboLines[1].color0 = Config.Combo.BottomLeft;
                    comboLines[1].color1 = Config.Combo.BottomRight;
                }
            }
            else
            {
                comboLines[0].gradient = false;
                comboLines[1].gradient = false;

                comboLines[0].color = Config.Combo.TopLine;
                comboLines[1].color = Config.Combo.BottomLine;
            }

            if (Config.Misc.ItalicizeComboPanel)
            {
                comboText.fontStyle = TMPro.FontStyles.Italic;
                comboText.text = "COMBO";
                numText.fontStyle = TMPro.FontStyles.Italic;
                numText.transform.localPosition = new Vector3(-2.5f, 4);
            }
            else
            {
                comboText.fontStyle = TMPro.FontStyles.UpperCase;
                numText.fontStyle = TMPro.FontStyles.UpperCase;
                numText.transform.localPosition = new Vector3(0, 4);
            }
        }

        private void UpdateProgressBar(float time)
        {
            progressPanelImages[1].color = Config.Progress.BG.ColorWithAlpha(0.25f);
            progressPanelImages[2].color = Config.Progress.Handle;

            if (Config.Progress.UseFadeDisplayType)
            {
                var x = (time - 0.5f) * 50;
                progressPanelImages[0].rectTransform.anchorMax = new Vector2(time, 1);
                progressPanelImages[2].transform.localPosition = new Vector3(x, 0, 0);

                progressPanelImages[0].color = HSBColor.Lerp(
                    HSBColor.FromColor(Config.Progress.StartColor),
                    HSBColor.FromColor(Config.Progress.EndColor),
                    time).ToColor();
            }

            else
            {
                progressPanelImages[0].rectTransform.anchorMax = new Vector2(0.5f, 1);
                progressPanelImages[2].transform.localPosition = Vector3.zero;
                progressPanelImages[0].color = Config.Progress.Fill;
            }
        }

        private void UpdateImmediateRankPanel()
        {
            if (rank > 90.00m)
                rankText.text = "SS";
            else if (rank > 80.00m)
                rankText.text = "S";
            else if (rank > 65.00m)
                rankText.text = "A";
            else if (rank > 50.00m)
                rankText.text = "B";
            else if (rank  > 35.00m)
                rankText.text = "C";
            else if (rank > 20.00m)
                rankText.text = "D";

            else rankText.text = "E";

            if (Config.Misc.ItalicizeScore)
            {
                scoreText.fontStyle = TMPro.FontStyles.Italic;
                scoreText.transform.localPosition = new Vector3(-1, 20);
            }
            else
            {
                scoreText.fontStyle = TMPro.FontStyles.Normal;
                scoreText.transform.localPosition = new Vector3(0, 20);
            }

            if (Config.Misc.ItalicizeImmediateRank)
            {
                percentText.fontStyle = TMPro.FontStyles.Italic;
                rankText.fontStyle = TMPro.FontStyles.Italic;
                rankText.transform.localPosition = new Vector3(-3, 0.5f);
            }
            else
            {
                percentText.fontStyle = TMPro.FontStyles.Normal;
                rankText.fontStyle = TMPro.FontStyles.Normal;
                rankText.transform.localPosition = new Vector3(0, 0.5f);
            }
        }
    }
}
