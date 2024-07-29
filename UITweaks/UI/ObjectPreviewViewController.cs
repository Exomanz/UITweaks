using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using SiraUtil.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tweening;
using UITweaks.Config;
using UITweaks.Models;
using UITweaks.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UITweaks.UI
{
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

        [UIComponent("loading-container")] private readonly VerticalLayoutGroup loadingContainer;

        private readonly Vector3 DEFAULT_POSITION = new(3.53f, 1.3f, 2.4f);
        private readonly Vector3 VOID_POSITION = new(0, -1000, 0);

        private List<PreviewPanel> enableablePanels;
        private List<PreviewPanel> transformablePanels;

        #region Preview Objects
        // Multiplier Panel
        public PreviewPanel MultiplierPanel => objectGrabber.PreviewPanels[0];
        private Image[] multiplierCircles;
        private CurvedTextMeshPro multiplierText;
        private bool previewCoroOn8x = false;

        // Energy Panel
        public PreviewPanel EnergyPanel => objectGrabber.PreviewPanels[1];
        private Image energyBar;
        private float fillAmount = 0.01f;

        // Combo Panel
        public PreviewPanel ComboPanel => objectGrabber.PreviewPanels[2];
        private ImageView[] comboLines;
        private CurvedTextMeshPro comboNumberText;
        private CurvedTextMeshPro comboText;

        // Progress Panel
        public PreviewPanel ProgressPanel => objectGrabber.PreviewPanels[3];
        private Image[] progressPanelImages;

        // Position Panel
        public PreviewPanel PositionPanel => objectGrabber.PreviewPanels[4];
        private MockMultiplayerPositionPanel positionPanel;
        private GameObject firstPlaceAnimationGO;
        private CurvedTextMeshPro firstPlaceAnimationGOText;
        private CurvedTextMeshPro positionText;
        private CurvedTextMeshPro playerCountText;
        private bool previewPositionIsFirst = false;
        private float animationOpacity;
        private Vector3 animationScale;
        private FloatTween positionPanelScaleTween;
        private FloatTween positionPanelOpacityTween;

        // Score Panel
        public PreviewPanel RankPanel => objectGrabber.PreviewPanels[5];
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
                this.StartCoroutine(FinalizePanels());
            }

            loadingContainer?.gameObject.SetActive(!objectGrabber.IsCompleted);
            objectGrabber.transform.position = objectGrabber.IsCompleted ? DEFAULT_POSITION : VOID_POSITION;
            modSettingsViewController.TabWasChangedEvent += UpdatePanelVisibility;
            modSettingsViewController.RankShouldBeUpdatedEvent += HandleRankLetterRequest;

            this.StartCoroutine(MultiplierPreviewCoroutine());
            this.StartCoroutine(MultiplayerPositionPreviewCoroutine());

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
            this.StartCoroutine(objectGrabber.GetPanels());
            yield return new WaitUntil(() => objectGrabber.IsCompleted);

            try
            {
                MultiplierPanel.Panel.SetActive(true);
                GameObject panel;

                // Multiplier Panel Setup
                {
                    panel = MultiplierPanel.Panel;
                    multiplierText = panel.GetComponentsInChildren<CurvedTextMeshPro>().Last();
                    multiplierCircles = panel.transform.GetComponentsInChildren<Image>();

                    multiplierCircles[1].color = multiplierConfig.One;
                    multiplierCircles[0].color = multiplierConfig.One.ColorWithAlpha(0.25f);
                }

                // Energy Bar Setup
                panel = EnergyPanel.Panel;
                energyBar = panel.transform.Find("EnergyBarWrapper/EnergyBar").GetComponent<Image>();

                // Combo Panel Setup
                {
                    panel = ComboPanel.Panel;
                    comboLines = panel.transform.GetComponentsInChildren<ImageView>();
                    comboNumberText = panel.transform.Find("ComboCanvas/NumText").GetComponent<CurvedTextMeshPro>();
                    comboText = panel.transform.Find("ComboText").GetComponent<CurvedTextMeshPro>();
                }

                // Progress Panel Setup
                {
                    panel = ProgressPanel.Panel;
                    progressPanelImages = panel.transform.GetComponentsInChildren<Image>();
                    CurvedTextMeshPro[] texts = panel.transform.GetComponentsInChildren<CurvedTextMeshPro>();
                    texts[0].text = "0";
                    texts[1].text = "00";
                    texts[2].text = "0";
                    texts[3].text = "01";
                }

                // Position Panel Setup 
                {
                    panel = PositionPanel.Panel;
                    positionPanel = panel.GetComponent<MockMultiplayerPositionPanel>();
                    positionPanel.transform.localPosition = VOID_POSITION;
                    positionText = positionPanel.positionText;
                    playerCountText = positionPanel.playerCountText;

                    firstPlaceAnimationGO = GameObject.Instantiate(positionText.gameObject, panel.transform, true);
                    firstPlaceAnimationGO.SetActive(false);

                    firstPlaceAnimationGOText = firstPlaceAnimationGO.GetComponent<CurvedTextMeshPro>();
                    firstPlaceAnimationGOText.text = "1";
                    firstPlaceAnimationGOText.color = positionConfig.First;

                    positionPanelOpacityTween = new FloatTween(0.75f, 0f, time =>
                    {
                        animationOpacity = time;
                    }, 1f, EaseType.Linear);

                    positionPanelScaleTween = new FloatTween(1f, 3f, time =>
                    {
                        animationScale = new Vector3(time, time, time);
                    }, 1f, EaseType.Linear);
                }

                // Immediate Rank Panel Setup
                {
                    panel = RankPanel.Panel;
                    Transform immediateRankTransform = panel.transform;
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

            if (objectGrabber.PreviewPanels.Count > 0)
            {
                enableablePanels = new List<PreviewPanel>()
                {
                    objectGrabber.PreviewPanels[1],
                    objectGrabber.PreviewPanels[2],
                    objectGrabber.PreviewPanels[3],
                    objectGrabber.PreviewPanels[5],
                };

                transformablePanels = new List<PreviewPanel>()
                {
                    objectGrabber.PreviewPanels[0],
                    objectGrabber.PreviewPanels[4],
                };
            }

            objectGrabber.transform.position = DEFAULT_POSITION;
            loadingContainer.gameObject.SetActive(false);
            modSettingsViewController.RaiseTabEvent(modSettingsViewController.SelectedTab);
            yield break;
        }

        private void UpdatePanelVisibility(int tab)
        {
            if (!objectGrabber.IsCompleted) return;
            System.Random rand = new System.Random();

            foreach (PreviewPanel panel in enableablePanels)
            {
                panel.Panel.SetActive(tab == panel.ActiveTab);
            }

            foreach (PreviewPanel panel in transformablePanels)
            {
                panel.Panel.transform.localPosition = tab == panel.ActiveTab ? Vector3.zero : VOID_POSITION;
            }

            if (tab == 2)
            {
                comboNumberText.text = rand.Next(0, 250).ToString();
                objectGrabber.PreviewPanels[2].Panel.transform.localPosition = Vector3.zero;
            }

            if (tab == 5)
            {
                rank = Utilities.Utilities.RandomDecimal(100, 1);
                percentText.text = rank.ToString() + "%";
                comboNumberText.text = rand.Next(0, 250).ToString();
                scoreText.text = rand.Next(0, 999999).ToString();

                objectGrabber.PreviewPanels[2].Panel.SetActive(true);
                objectGrabber.PreviewPanels[2].Panel.transform.localPosition = new Vector3(-0.75f, 0, 0);
            }
        }

        public void Update()
        {
            if (!objectGrabber.IsCompleted) return;

            int tab = modSettingsViewController.SelectedTab;
            switch (tab)
            {
                case 0:
                    if (previewCoroOn8x)
                    {
                        if (multiplierConfig.RainbowOnMaxMultiplier)
                            multiplierCircles[0].color = rainbowEffectManager.Rainbow;
                        else multiplierCircles[0].color = multiplierConfig.Eight.ColorWithAlpha(0.25f);
                    }
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
                case 4:
                    if (previewPositionIsFirst)
                    {
                        bool firstPlaceAnimPresent = firstPlaceAnimationGO.gameObject != null;

                        if (positionConfig.RainbowOnFirstPlace)
                        {
                            positionPanel.positionText.color = rainbowEffectManager.Rainbow;
                            if (firstPlaceAnimPresent)
                                firstPlaceAnimationGOText.color = rainbowEffectManager.Rainbow;

                            if (!positionConfig.UseStaticColorForStaticPanel)
                                positionPanel.playerCountText.color = rainbowEffectManager.Rainbow.ColorWithAlpha(0.25f);
                            else positionPanel.playerCountText.color = positionConfig.StaticPanelColor.ColorWithAlpha(0.25f);
                        }
                        else
                        {
                            positionPanel.positionText.color = positionConfig.First;
                            if (firstPlaceAnimPresent)
                                firstPlaceAnimationGOText.color = positionConfig.First;
                        }

                        if (firstPlaceAnimPresent)
                        {
                            firstPlaceAnimationGO.transform.localScale = animationScale;
                            firstPlaceAnimationGOText.alpha = animationOpacity;
                        }
                    }
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
            yield return new WaitUntil(() => objectGrabber.IsCompleted);
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
                tweeningManager.AddTween(new FloatTween(0, 1, (time) =>
                {
                    Color frame = HSBColor.Lerp(
                        HSBColor.FromColor(multiplierConfig.Four),
                        HSBColor.FromColor(multiplierConfig.Eight), time)
                        .ToColor();

                    multiplierCircles[1].fillAmount = time;
                    multiplierCircles[1].color = frame;
                    multiplierCircles[0].color = frame.ColorWithAlpha(0.25f);
                }, 0.98f, EaseType.Linear), this);
                yield return new WaitForSecondsRealtime(1);


                previewCoroOn8x = true;
                multiplierText.text = "8";
                multiplierCircles[1].color = Color.clear;
                multiplierCircles[1].fillAmount = 0;

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

            if (progressConfig.Mode == ProgressConfig.DisplayType.Fade)
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

        private IEnumerator MultiplayerPositionPreviewCoroutine()
        {
            yield return new WaitUntil(() => objectGrabber.IsCompleted);
            previewPositionIsFirst = false;

            positionPanel.positionText.text = "5";
            positionPanel.positionText.color = positionConfig.Fifth;
            positionPanel.playerCountText.color = positionConfig.UseStaticColorForStaticPanel ? positionConfig.StaticPanelColor : positionConfig.Fifth;
            positionPanel.playerCountText.color = positionPanel.playerCountText.color.ColorWithAlpha(0.25f);
            yield return new WaitForSecondsRealtime(1f);

            positionText.text = "4";
            positionText.color = positionConfig.Fourth;
            playerCountText.color = positionConfig.UseStaticColorForStaticPanel ? positionConfig.StaticPanelColor : positionConfig.Fourth;
            positionPanel.playerCountText.color = positionPanel.playerCountText.color.ColorWithAlpha(0.25f);
            yield return new WaitForSecondsRealtime(1f);

            positionText.text = "3";
            positionText.color = positionConfig.Third;
            playerCountText.color = positionConfig.UseStaticColorForStaticPanel ? positionConfig.StaticPanelColor : positionConfig.Third;
            positionPanel.playerCountText.color = positionPanel.playerCountText.color.ColorWithAlpha(0.25f);
            yield return new WaitForSecondsRealtime(1f);

            positionText.text = "2";
            positionText.color = positionConfig.Second;
            playerCountText.color = positionConfig.UseStaticColorForStaticPanel ? positionConfig.StaticPanelColor : positionConfig.Second;
            positionPanel.playerCountText.color = positionPanel.playerCountText.color.ColorWithAlpha(0.25f);
            yield return new WaitForSecondsRealtime(1f);

            positionText.text = "1";
            positionText.color = positionConfig.First;
            playerCountText.color = positionConfig.UseStaticColorForStaticPanel ? positionConfig.StaticPanelColor : positionConfig.First;
            positionPanel.playerCountText.color = positionPanel.playerCountText.color.ColorWithAlpha(0.25f);
            previewPositionIsFirst = true;

            if (!positionConfig.HideFirstPlaceAnimation)
            {
                firstPlaceAnimationGO.SetActive(true);
                tweeningManager.RestartTween(positionPanelScaleTween, this);
                tweeningManager.RestartTween(positionPanelOpacityTween, this);
            }

            yield return new WaitForSecondsRealtime(1f);
            firstPlaceAnimationGO.SetActive(false);

            yield return MultiplayerPositionPreviewCoroutine();
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
