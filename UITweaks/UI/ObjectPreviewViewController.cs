using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using SiraUtil.Logging;
using System;
using System.Collections;
using System.Linq;
using Tweening;
using UITweaks.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UITweaks.UI
{
    /// <summary>
    /// Controller class for the Object Previewer ViewController.
    /// </summary>
    [ViewDefinition("UITweaks.Views.ObjectPreview.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\ObjectPreview.bsml")]
    public class ObjectPreviewViewController : BSMLAutomaticViewController
    {
        [Inject] private readonly SettingsPanelObjectGrabber objectGrabber;
        [Inject] private readonly PluginConfig pluginConfig;
        [Inject] private readonly ModSettingsViewController modSettingsViewController;
        [Inject] private readonly SiraLog logger;
        [Inject] private readonly TimeTweeningManager tweeningManager;

        private readonly Vector3 DEFAULT_POSITION = new(3.53f, 1.1f, 2.4f);
        private readonly Vector3 VOID_POSITION = new(0, -1000, 0);

        private bool previewToggleIsReady = false;

        #region Preview Objects
        // Combo Panel
        private Image[] multiplierCircles = null!;
        private CurvedTextMeshPro multiplierText = null!;
        private bool previewCoroOn8x = false;

        // Energy Panel
        private Image energyBar = null!;
        private float fillAmount = 0.01f;

        // Combo Panel
        private ImageView[] comboLines = null!;
        private CurvedTextMeshPro numText = null!;
        private CurvedTextMeshPro comboText = null!;

        // Progress Panel
        private Image[] progressPanelImages = null!;

        // Score Panel
        private CurvedTextMeshPro scoreText = null!;
        private CurvedTextMeshPro percentText = null!;
        private CurvedTextMeshPro rankText = null!;
        private decimal rank = 0.00m;
        #endregion

        [UIValue("allow-previews")] private bool AllowPreviews
        {
            get => pluginConfig.AllowPreviews;
            set
            {
                pluginConfig.AllowPreviews = value;
                if (!previewToggleIsReady) return; // Prevent BSML Parsing Error
                UpdatePanelVisibility(value ? modSettingsViewController.SelectedTab : -1);
            }
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);

            if (firstActivation && objectGrabber.gameObject != null)
            {
                objectGrabber.transform.Rotate(0, 57, 0);
                this.StartCoroutine(objectGrabber.GetPanels()); 
                this.StartCoroutine(FinalizePanels());
            }

            objectGrabber.transform.position = DEFAULT_POSITION;
            modSettingsViewController.TabWasChangedEvent += UpdatePanelVisibility;
            this.StartCoroutine(MultiplierPreviewCoroutine());
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
            tweeningManager.KillAllTweens(this);
            modSettingsViewController.TabWasChangedEvent -= UpdatePanelVisibility;
            objectGrabber.transform.position = VOID_POSITION;
        }

        private IEnumerator FinalizePanels()
        {
            yield return new WaitUntil(() => objectGrabber.isCompleted);

            var multiplierPanel = objectGrabber.MultiplierPanel;
            var comboPanel = objectGrabber.ComboPanel;
            var progressPanel = objectGrabber.ProgressPanel;

            // Grab all of the important parts of the panels for previewer.
            try
            {
                objectGrabber.MultiplierPanel.SetActive(true);

                // Multiplier Panel Setup
                {
                    multiplierText = multiplierPanel.GetComponentsInChildren<CurvedTextMeshPro>().Last();
                    multiplierCircles = multiplierPanel.transform.GetComponentsInChildren<Image>();

                    multiplierCircles[1].color = pluginConfig.Multiplier.One;
                    multiplierCircles[0].color = pluginConfig.Multiplier.One.ColorWithAlpha(0.25f);
                }

                // Energy Bar Setup
                energyBar = objectGrabber.EnergyPanel.transform.Find("EnergyBarWrapper/EnergyBar")?.GetComponent<Image>();

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
                    var immediateRankTransform = objectGrabber.ImmediateRankPanel.transform;
                    scoreText = immediateRankTransform.Find("ScoreText")?.GetComponent<CurvedTextMeshPro>();
                    percentText = immediateRankTransform.Find("RelativeScoreText")?.GetComponent<CurvedTextMeshPro>();
                    rankText = immediateRankTransform.Find("ImmediateRankText")?.GetComponent<CurvedTextMeshPro>();

                    immediateRankTransform.localPosition = new Vector3(0.75f, 0, 0);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            previewToggleIsReady = true;
            modSettingsViewController.RaiseTabEvent(pluginConfig.AllowPreviews ? modSettingsViewController.SelectedTab : -1);
            yield break;
        }

        private void UpdatePanelVisibility(int tab)
        {
            if (!objectGrabber.isCompleted) return;
            var host = objectGrabber;

            if (!pluginConfig.AllowPreviews)
            {
                host.MultiplierPanel.transform.position = VOID_POSITION;
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
                    host.MultiplierPanel.transform.position = VOID_POSITION;
                    host.EnergyPanel.SetActive(true);
                    host.ComboPanel.SetActive(false);
                    host.ProgressPanel.SetActive(false);
                    host.ImmediateRankPanel.SetActive(false);
                    break;
                case 2:
                    host.MultiplierPanel.transform.position = VOID_POSITION;
                    host.EnergyPanel.SetActive(false);
                    host.ComboPanel.SetActive(true);
                    host.ProgressPanel.SetActive(false);
                    host.ImmediateRankPanel.SetActive(false);

                    numText.text = new System.Random().Next(0, 250).ToString();
                    host.ComboPanel.transform.localPosition = Vector3.zero;
                    break;
                case 3:
                    host.MultiplierPanel.transform.position = VOID_POSITION;
                    host.EnergyPanel.SetActive(false);
                    host.ComboPanel.SetActive(false);
                    host.ProgressPanel.SetActive(true);
                    host.ImmediateRankPanel.SetActive(false);
                    break;
                case 4:
                    host.MultiplierPanel.transform.position = VOID_POSITION;
                    host.EnergyPanel.SetActive(false);
                    host.ComboPanel.SetActive(false);
                    host.ProgressPanel.SetActive(false);
                    host.ImmediateRankPanel.SetActive(false);
                    break;
                case 5:
                    host.MultiplierPanel.transform.position = VOID_POSITION;
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
            if (!objectGrabber.isCompleted) return;

            int tab = modSettingsViewController.SelectedTab;
            switch (tab)
            {
                case 0:
                    if (previewCoroOn8x && pluginConfig.Multiplier.RainbowOnMaxMultiplier)
                        multiplierCircles[0].color = new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1), 1, 1).ToColor();
                    break;
                case 1:
                    if (fillAmount == 1 && pluginConfig.Energy.RainbowOnFullEnergy)
                        energyBar.color = new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1), 1, 1).ToColor();
                    else if (!pluginConfig.Energy.RainbowOnFullEnergy)
                        energyBar.color = pluginConfig.Energy.High; // Not sure why it won't work in the updater method, so I have to set it here.
                    UpdateEnergyBar(modSettingsViewController.EnergyBarFillAmount);
                    break;
                case 2:
                    UpdateComboPanel();
                    break;
                case 3:
                    UpdateProgressBar(modSettingsViewController.ProgressBarFillAmount);
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
            if (!pluginConfig.Multiplier.SmoothTransition)
            {
                multiplierCircles[0].color = pluginConfig.Multiplier.One.ColorWithAlpha(0.25f);
                multiplierCircles[1].color = pluginConfig.Multiplier.One;
                multiplierText.text = "1";
                yield return new WaitForSecondsRealtime(1);

                multiplierCircles[0].color = pluginConfig.Multiplier.Two.ColorWithAlpha(0.25f);
                multiplierCircles[1].color = pluginConfig.Multiplier.Two;
                multiplierText.text = "2";
                yield return new WaitForSecondsRealtime(1);

                multiplierCircles[0].color = pluginConfig.Multiplier.Four.ColorWithAlpha(0.25f);
                multiplierCircles[1].color = pluginConfig.Multiplier.Four;
                multiplierText.text = "4";
                yield return new WaitForSecondsRealtime(1);

                previewCoroOn8x = true;
                multiplierText.text = "8";
                multiplierCircles[1].fillAmount = 0;
                if (!pluginConfig.Multiplier.RainbowOnMaxMultiplier)
                    multiplierCircles[0].color = pluginConfig.Multiplier.Eight.ColorWithAlpha(0.25f);
                else { /* The rainbow effect is controlled outside of this method body */ }

                yield return new WaitForSecondsRealtime(1);
            }
            else
            {
                multiplierText.text = "1";
                tweeningManager.AddTween(new FloatTween(0, 1, (float time) =>
                {
                    Color frame = HSBColor.Lerp(
                        HSBColor.FromColor(pluginConfig.Multiplier.One),
                        HSBColor.FromColor(pluginConfig.Multiplier.Two), time).ToColor();

                    multiplierCircles[1].fillAmount = time;
                    multiplierCircles[1].color = frame;
                    multiplierCircles[0].color = frame.ColorWithAlpha(0.25f);
                }, 
                1, EaseType.Linear), this);
                yield return new WaitForSecondsRealtime(1);

                multiplierText.text = "2";
                tweeningManager.AddTween(new FloatTween(0, 1, (float time) =>
                {
                    Color frame = HSBColor.Lerp(
                        HSBColor.FromColor(pluginConfig.Multiplier.Two),
                        HSBColor.FromColor(pluginConfig.Multiplier.Four), time).ToColor();

                    multiplierCircles[1].fillAmount = time;
                    multiplierCircles[1].color = frame;
                    multiplierCircles[0].color = frame.ColorWithAlpha(0.25f);
                },
                1, EaseType.Linear), this);
                yield return new WaitForSecondsRealtime(1);

                multiplierText.text = "4";
                tweeningManager.AddTween(new FloatTween(0, 1, (float time) =>
                {
                    Color frame = HSBColor.Lerp(
                        HSBColor.FromColor(pluginConfig.Multiplier.Four),
                        HSBColor.FromColor(pluginConfig.Multiplier.Eight), time).ToColor();

                    multiplierCircles[1].fillAmount = time;
                    multiplierCircles[1].color = frame;
                    multiplierCircles[0].color = frame.ColorWithAlpha(0.25f);
                },
                1, EaseType.Linear), this);
                yield return new WaitForSecondsRealtime(1);

                tweeningManager.KillAllTweens(this);

                previewCoroOn8x = true;
                multiplierText.text = "8";
                multiplierCircles[1].fillAmount = 0;
                if (!pluginConfig.Multiplier.RainbowOnMaxMultiplier)
                    multiplierCircles[0].color = pluginConfig.Multiplier.Eight.ColorWithAlpha(0.25f);
                else { /* The rainbow effect is controlled outside of this method body */ }

                yield return new WaitForSecondsRealtime(1);
            }

            yield return MultiplierPreviewCoroutine();
        }

        private void UpdateEnergyBar(float fillAmount)
        {
            this.fillAmount = fillAmount;
            energyBar.rectTransform.anchorMax = new Vector2(fillAmount, 1);

            if (fillAmount == 0.5f) energyBar.color = pluginConfig.Energy.Mid;

            else if (fillAmount > 0.5f && fillAmount < 1) energyBar.color = HSBColor.Lerp(
                HSBColor.FromColor(pluginConfig.Energy.Mid),
                HSBColor.FromColor(pluginConfig.Energy.High),
                (fillAmount - 0.5f) * 2).ToColor();

            else if (fillAmount < 0.5f) energyBar.color = HSBColor.Lerp(
                HSBColor.FromColor(pluginConfig.Energy.Low),
                HSBColor.FromColor(pluginConfig.Energy.Mid),
                fillAmount * 2).ToColor();
        }

        private void UpdateComboPanel()
        {
            if (pluginConfig.Combo.UseGradient)
            {
                comboLines[0].gradient = true;
                comboLines[0].color = Color.white;
                comboLines[1].gradient = true;
                comboLines[1].color = Color.white;

                comboLines[0].color0 = pluginConfig.Combo.TopLeft;
                comboLines[0].color1 = pluginConfig.Combo.TopRight;

                if (pluginConfig.Combo.MirrorBottomLine)
                {
                    comboLines[1].color0 = pluginConfig.Combo.TopRight;
                    comboLines[1].color1 = pluginConfig.Combo.TopLeft;
                }
                else
                {
                    comboLines[1].color0 = pluginConfig.Combo.BottomLeft;
                    comboLines[1].color1 = pluginConfig.Combo.BottomRight;
                }
            }
            else
            {
                comboLines[0].gradient = false;
                comboLines[1].gradient = false;

                comboLines[0].color = pluginConfig.Combo.TopLine;
                comboLines[1].color = pluginConfig.Combo.BottomLine;
            }

            if (pluginConfig.Misc.ItalicizeComboPanel)
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
            progressPanelImages[1].color = pluginConfig.Progress.BG.ColorWithAlpha(0.25f);
            progressPanelImages[2].color = pluginConfig.Progress.Handle;

            if (pluginConfig.Progress.UseFadeDisplayType)
            {
                var x = (time - 0.5f) * 50;
                progressPanelImages[0].rectTransform.anchorMax = new Vector2(time, 1);
                progressPanelImages[2].transform.localPosition = new Vector3(x, 0, 0);

                progressPanelImages[0].color = HSBColor.Lerp(
                    HSBColor.FromColor(pluginConfig.Progress.StartColor),
                    HSBColor.FromColor(pluginConfig.Progress.EndColor),
                    time).ToColor();
            }

            else
            {
                progressPanelImages[0].rectTransform.anchorMax = new Vector2(0.5f, 1);
                progressPanelImages[2].transform.localPosition = Vector3.zero;
                progressPanelImages[0].color = pluginConfig.Progress.Fill;
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

            if (pluginConfig.Misc.ItalicizeScore)
            {
                scoreText.fontStyle = TMPro.FontStyles.Italic;
                scoreText.transform.localPosition = new Vector3(-1, 20);
            }
            else
            {
                scoreText.fontStyle = TMPro.FontStyles.Normal;
                scoreText.transform.localPosition = new Vector3(0, 20);
            }

            if (pluginConfig.Misc.ItalicizeImmediateRank)
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
