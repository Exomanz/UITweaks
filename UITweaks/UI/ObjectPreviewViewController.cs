﻿using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using SiraUtil.Logging;
using System;
using System.Collections;
using System.Linq;
using Tweening;
using UITweaks.Config;
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
        [Inject] private readonly ModSettingsViewController modSettingsViewController;
        [Inject] private readonly SiraLog logger;
        [Inject] private readonly TimeTweeningManager tweeningManager;
        [Inject] private readonly RainbowEffectManager rainbowEffectManager;

        [Inject] private readonly ComboConfig comboConfig;
        [Inject] private readonly EnergyConfig energyConfig;
        [Inject] private readonly MiscConfig miscConfig;
        [Inject] private readonly MultiplierConfig multiplierConfig;
        [Inject] private readonly PositionConfig positionConfig;
        [Inject] private readonly ProgressConfig progressConfig;

        private readonly Vector3 DEFAULT_POSITION = new(3.53f, 1.3f, 2.4f);
        private readonly Vector3 VOID_POSITION = new(0, -1000, 0);

        private bool previewToggleIsReady = false;

        #region Preview Objects
        // Combo Panel
        private Image[] multiplierCircles;
        private CurvedTextMeshPro multiplierText;
        private bool previewCoroOn8x = false;

        // Energy Panel
        private Image energyBar;
        private float fillAmount = 0.01f;

        // Combo Panel
        private ImageView[] comboLines;
        private CurvedTextMeshPro comboNumberText;
        private CurvedTextMeshPro comboText;

        // Progress Panel
        private Image[] progressPanelImages;

        // Score Panel
        private CurvedTextMeshPro scoreText;
        private CurvedTextMeshPro percentText;
        private CurvedTextMeshPro rankText;
        private decimal rank = 0.00m;
        #endregion

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
            modSettingsViewController.RankShouldBeUpdatedEvent += HandleRankLetterRequest;
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
            yield return new WaitUntil(() => objectGrabber.IsCompleted);

            try
            {
                objectGrabber.MultiplierPanel.SetActive(true);

                // Multiplier Panel Setup
                {
                    multiplierText = objectGrabber.MultiplierPanel.GetComponentsInChildren<CurvedTextMeshPro>().Last();
                    multiplierCircles = objectGrabber.MultiplierPanel.transform.GetComponentsInChildren<Image>();

                    multiplierCircles[1].color = multiplierConfig.One;
                    multiplierCircles[0].color = multiplierConfig.One.ColorWithAlpha(0.25f);
                }

                // Energy Bar Setup
                energyBar = objectGrabber.EnergyPanel.transform.Find("EnergyBarWrapper/EnergyBar").GetComponent<Image>();

                // Combo Panel Setup
                {
                    comboLines = objectGrabber.ComboPanel.transform.GetComponentsInChildren<ImageView>();
                    comboNumberText = objectGrabber.ComboPanel.transform.Find("ComboCanvas/NumText").GetComponent<CurvedTextMeshPro>();
                    comboText = objectGrabber.ComboPanel.transform.Find("ComboText").GetComponent<CurvedTextMeshPro>();
                }

                // Progress Panel Setup
                {
                    progressPanelImages = objectGrabber.ProgressPanel.transform.GetComponentsInChildren<Image>();
                    CurvedTextMeshPro[] texts = objectGrabber.ProgressPanel.transform.GetComponentsInChildren<CurvedTextMeshPro>();
                    texts[0].text = "0";
                    texts[1].text = "00";
                    texts[2].text = "0";
                    texts[3].text = "01";
                }

                // Immediate Rank Panel Setup
                {
                    Transform immediateRankTransform = objectGrabber.ImmediateRankPanel.transform;
                    scoreText = immediateRankTransform.Find("ScoreText").GetComponent<CurvedTextMeshPro>();
                    percentText = immediateRankTransform.Find("RelativeScoreText").GetComponent<CurvedTextMeshPro>();
                    rankText = immediateRankTransform.Find("ImmediateRankText").GetComponent<CurvedTextMeshPro>();

                    immediateRankTransform.localPosition = new Vector3(0.75f, 0, 0);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            previewToggleIsReady = true;
            modSettingsViewController.RaiseTabEvent(modSettingsViewController.SelectedTab);
            yield break;
        }

        private void UpdatePanelVisibility(int tab)
        {
            if (!objectGrabber.IsCompleted) return;
            System.Random rand = new System.Random();

            switch (tab)
            {
                case 0:
                    objectGrabber.MultiplierPanel.transform.localPosition = Vector3.zero;
                    objectGrabber.EnergyPanel.SetActive(false);
                    objectGrabber.ComboPanel.SetActive(false);
                    objectGrabber.ProgressPanel.SetActive(false);
                    objectGrabber.ImmediateRankPanel.SetActive(false);
                    break;
                case 1:
                    objectGrabber.MultiplierPanel.transform.position = VOID_POSITION;
                    objectGrabber.EnergyPanel.SetActive(true);
                    objectGrabber.ComboPanel.SetActive(false);
                    objectGrabber.ProgressPanel.SetActive(false);
                    objectGrabber.ImmediateRankPanel.SetActive(false);
                    break;
                case 2:
                    objectGrabber.MultiplierPanel.transform.position = VOID_POSITION;
                    objectGrabber.EnergyPanel.SetActive(false);
                    objectGrabber.ComboPanel.SetActive(true);
                    objectGrabber.ProgressPanel.SetActive(false);
                    objectGrabber.ImmediateRankPanel.SetActive(false);

                    comboNumberText.text = rand.Next(0, 250).ToString();
                    objectGrabber.ComboPanel.transform.localPosition = Vector3.zero;
                    break;
                case 3:
                    objectGrabber.MultiplierPanel.transform.position = VOID_POSITION;
                    objectGrabber.EnergyPanel.SetActive(false);
                    objectGrabber.ComboPanel.SetActive(false);
                    objectGrabber.ProgressPanel.SetActive(true);
                    objectGrabber.ImmediateRankPanel.SetActive(false);
                    break;
                case 4:
                    objectGrabber.MultiplierPanel.transform.position = VOID_POSITION;
                    objectGrabber.EnergyPanel.SetActive(false);
                    objectGrabber.ComboPanel.SetActive(false);
                    objectGrabber.ProgressPanel.SetActive(false);
                    objectGrabber.ImmediateRankPanel.SetActive(false);
                    break;
                case 5:
                    objectGrabber.MultiplierPanel.transform.position = VOID_POSITION;
                    objectGrabber.EnergyPanel.SetActive(false);
                    objectGrabber.ComboPanel.SetActive(true);
                    objectGrabber.ProgressPanel.SetActive(false);
                    objectGrabber.ImmediateRankPanel.SetActive(true);

                    rank = Utilities.Utilities.RandomDecimal(100, 1);
                    percentText.text = rank.ToString() + "%";
                    comboNumberText.text = rand.Next(0, 250).ToString();
                    scoreText.text = rand.Next(0, 999999).ToString();

                    objectGrabber.ComboPanel.transform.localPosition = new Vector3(-0.75f, 0, 0);
                    break;

                default:
                    break;
            }
        }

        public void Update()
        {
            if (!objectGrabber.IsCompleted) return;

            int tab = modSettingsViewController.SelectedTab;
            switch (tab)
            {
                case 0:
                    if (previewCoroOn8x && multiplierConfig.RainbowOnMaxMultiplier)
                        multiplierCircles[0].color = rainbowEffectManager.Rainbow;
                    break;
                case 1:
                    if (fillAmount == 1 && energyConfig.RainbowOnFullEnergy)
                        energyBar.color = rainbowEffectManager.Rainbow;
                    else if (!energyConfig.RainbowOnFullEnergy)
                        energyBar.color = energyConfig.High; 

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
                    if (rank >= 90.00m && miscConfig.RainbowOnSSRank && miscConfig.AllowRankColoring)
                        rankText.color = rainbowEffectManager.Rainbow;
                    break;
            }
        }

        private IEnumerator MultiplierPreviewCoroutine()
        {
            yield return new WaitUntil(() => previewToggleIsReady);
            previewCoroOn8x = false;
            
            if (!multiplierConfig.SmoothTransition)
            {
                multiplierCircles[1].fillAmount = 0.5f;

                multiplierCircles[0].color = multiplierConfig.One.ColorWithAlpha(0.25f);
                multiplierCircles[1].color = multiplierConfig.One;
                multiplierText.text = "1";
                yield return new WaitForSecondsRealtime(1);

                multiplierCircles[0].color = multiplierConfig.Two.ColorWithAlpha(0.25f);
                multiplierCircles[1].color = multiplierConfig.Two;
                multiplierText.text = "2";
                yield return new WaitForSecondsRealtime(1);

                multiplierCircles[0].color = multiplierConfig.Four.ColorWithAlpha(0.25f);
                multiplierCircles[1].color = multiplierConfig.Four;
                multiplierText.text = "4";
                yield return new WaitForSecondsRealtime(1);

                previewCoroOn8x = true;
                multiplierText.text = "8";
                multiplierCircles[1].fillAmount = 0;
                if (!multiplierConfig.RainbowOnMaxMultiplier)
                    multiplierCircles[0].color = multiplierConfig.Eight.ColorWithAlpha(0.25f);
                else { /* The rainbow effect is controlled outside of this method body */ }

                yield return new WaitForSecondsRealtime(1);
            }
            else
            {
                multiplierText.text = "1";
                tweeningManager.AddTween(new FloatTween(0, 1, (float time) =>
                {
                    Color frame = HSBColor.Lerp(
                        HSBColor.FromColor(multiplierConfig.One),
                        HSBColor.FromColor(multiplierConfig.Two), time)
                        .ToColor();

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
                        HSBColor.FromColor(multiplierConfig.Two),
                        HSBColor.FromColor(multiplierConfig.Four), time)
                        .ToColor();

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
                        HSBColor.FromColor(multiplierConfig.Four),
                        HSBColor.FromColor(multiplierConfig.Eight), time)
                        .ToColor();

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
                if (!multiplierConfig.RainbowOnMaxMultiplier)
                    multiplierCircles[0].color = multiplierConfig.Eight.ColorWithAlpha(0.25f);
                else { /* The rainbow effect is controlled outside of this method body */ }

                yield return new WaitForSecondsRealtime(1);
            }

            yield return MultiplierPreviewCoroutine();
        }

        private void UpdateEnergyBar(float fillAmount)
        {
            this.fillAmount = fillAmount;
            energyBar.rectTransform.anchorMax = new Vector2(fillAmount, 1);

            if (fillAmount == 0.5f) energyBar.color = energyConfig.Mid;

            else if (fillAmount > 0.5f && fillAmount < 1) energyBar.color = HSBColor.Lerp(
                HSBColor.FromColor(energyConfig.Mid),
                HSBColor.FromColor(energyConfig.High),
                (fillAmount - 0.5f) * 2).ToColor();

            else if (fillAmount < 0.5f) energyBar.color = HSBColor.Lerp(
                HSBColor.FromColor(energyConfig.Low),
                HSBColor.FromColor(energyConfig.Mid),
                fillAmount * 2).ToColor();
        }

        private void UpdateComboPanel()
        {
            if (comboConfig.UseGradient)
            {
                comboLines[0].gradient = true;
                comboLines[0].color = Color.white;
                comboLines[1].gradient = true;
                comboLines[1].color = Color.white;

                comboLines[0].color0 = comboConfig.TopLeft;
                comboLines[0].color1 = comboConfig.TopRight;

                if (comboConfig.MirrorBottomLine)
                {
                    comboLines[1].color0 = comboConfig.TopRight;
                    comboLines[1].color1 = comboConfig.TopLeft;
                }
                else
                {
                    comboLines[1].color0 = comboConfig.BottomLeft;
                    comboLines[1].color1 = comboConfig.BottomRight;
                }
            }
            else
            {
                comboLines[0].gradient = false;
                comboLines[1].gradient = false;

                comboLines[0].color = comboConfig.TopLine;
                comboLines[1].color = comboConfig.BottomLine;
            }

            if (miscConfig.ItalicizeComboPanel)
            {
                comboText.fontStyle = TMPro.FontStyles.Italic | TMPro.FontStyles.UpperCase;
                comboText.text = "COMBO";
                comboNumberText.fontStyle = TMPro.FontStyles.Italic;
                comboNumberText.transform.localPosition = new Vector3(-2.5f, 4);
            }
            else
            {
                comboText.fontStyle = TMPro.FontStyles.UpperCase;
                comboNumberText.fontStyle = TMPro.FontStyles.UpperCase;
                comboNumberText.transform.localPosition = new Vector3(0, 4);
            }
        }

        private void UpdateProgressBar(float time)
        {
            progressPanelImages[1].color = progressConfig.BG.ColorWithAlpha(0.25f);
            progressPanelImages[2].color = progressConfig.Handle;

            if (progressConfig.UseFadeDisplayType)
            {
                var x = (time - 0.5f) * 50;
                progressPanelImages[0].rectTransform.anchorMax = new Vector2(time, 1);
                progressPanelImages[2].transform.localPosition = new Vector3(x, 0, 0);

                progressPanelImages[0].color = HSBColor.Lerp(
                    HSBColor.FromColor(progressConfig.StartColor),
                    HSBColor.FromColor(progressConfig.EndColor),
                    time).ToColor();
            }

            else
            {
                progressPanelImages[0].rectTransform.anchorMax = new Vector2(0.5f, 1);
                progressPanelImages[2].transform.localPosition = Vector3.zero;
                progressPanelImages[0].color = progressConfig.Fill;
            }
        }

        private void UpdateImmediateRankPanel()
        {
            rankText.text = ConvertRankToText(rank);

            if (miscConfig.ItalicizeScore)
            {
                scoreText.fontStyle = TMPro.FontStyles.Italic;
                scoreText.transform.localPosition = new Vector3(-1, 20);
            }
            else
            {
                scoreText.fontStyle = TMPro.FontStyles.Normal;
                scoreText.transform.localPosition = new Vector3(0, 20);
            }

            if (miscConfig.ItalicizeImmediateRank)
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

            if (miscConfig.AllowRankColoring)
                rankText.color = ConvertRankToColor(rank);
            else rankText.color = Color.white.ColorWithAlpha(0.5f);
        }

        public void HandleRankLetterRequest(string rankString)
        {
            rank = rankString switch
            {
                "SS" => Utilities.Utilities.RandomDecimal(90, 100, 2),
                "S" => Utilities.Utilities.RandomDecimal(80, 89.99, 2),
                "A" => Utilities.Utilities.RandomDecimal(65, 79.99, 2),
                "B" => Utilities.Utilities.RandomDecimal(50, 64.99, 2),
                "C" => Utilities.Utilities.RandomDecimal(35, 49.99, 2),
                "D" => Utilities.Utilities.RandomDecimal(20, 34.99, 2),
                _ => Utilities.Utilities.RandomDecimal(0, 19.99, 2)
            };

            percentText.text = rank.ToString() + "%";
            rankText.text = rankString;
        }

        private string ConvertRankToText(decimal rank)
        {
            string result = rank switch
            {
                > 90.00m => "SS",
                > 80.00m => "S",
                > 65.00m => "A",
                > 50.00m => "B",
                > 35.00m => "C",
                > 20.00m => "D",
                _ => "E"
            };

            return result;
        }

        private Color ConvertRankToColor(decimal rank)
        {
            Color result = rank switch
            {
                > 90.00m => miscConfig.RankSSColor,
                > 80.00m => miscConfig.RankSColor,
                > 65.00m => miscConfig.RankAColor,
                > 50.00m => miscConfig.RankBColor,
                > 35.00m => miscConfig.RankCColor,
                > 20.00m => miscConfig.RankDColor,
                _ => miscConfig.RankEColor
            };

            return result;
        }
    }
}
