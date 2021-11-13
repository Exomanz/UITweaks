using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using IPA.Utilities;
using SiraUtil.Tools;
using System.Collections;
using System.Linq;
using TMPro;
using UITweaks.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UITweaks.UI
{
    [ViewDefinition("UITweaks.Views.preview.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\preview.bsml")]
    public class ObjectPreviewViewController : BSMLAutomaticViewController
    {
#pragma warning disable CS0649
        [Inject] private MainConfig config;
        [Inject] private ModSettingsViewController modSettingsViewController;
        [Inject] private SiraLog log;

        public GameObject grabber;
        private SettingsPanelObjectGrabber settingsPanelObjectGrabber;
        private Vector3 defaultGrabberPosition = new Vector3(3.33f, 1.2f, 2.2f);

        #region Stuff For Previews
        private Image[] multiplierCircles;
        private CurvedTextMeshPro multiplierText;
        private bool isCoroutineOn8x = false;

        private Image energyBar;
        private float fillAmount = 0.01f;

        private ImageView[] lines;
        private CurvedTextMeshPro comboText;

        private Image[] progressPanelImages;

        private MockPositionPanel mockPositionPanel;
        #endregion

        [UIValue("enable-previews")]
        protected bool EnablePreviews
        {
            get => config.AllowPreviews;
            set
            {
                config.AllowPreviews = value;
                modSettingsViewController.Trigger();
            }
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);

            if (!grabber)
            {
                grabber = new GameObject("PanelGrabber");
                grabber.transform.position = defaultGrabberPosition;
                grabber.transform.Rotate(0f, 57f, 0f);
                settingsPanelObjectGrabber = grabber.AddComponent<SettingsPanelObjectGrabber>();

                mockPositionPanel = new GameObject("MockPositionPanel").AddComponent<MockPositionPanel>();
                mockPositionPanel.transform.SetParent(settingsPanelObjectGrabber.transform, false);
            }

            grabber.transform.position = defaultGrabberPosition;

            StartCoroutine(FinalizePanelsOnceGot());
            modSettingsViewController.visibilityEvent += UpdatePanelVisibility;
        }

        internal IEnumerator FinalizePanelsOnceGot()
        {
            yield return new WaitUntil(() => settingsPanelObjectGrabber.isCompleted);
            try
            {
                settingsPanelObjectGrabber.MultiplierPanel.SetActive(true);
                FinalMultiplierSetup();
                energyBar = settingsPanelObjectGrabber.EnergyPanel.transform.Find("EnergyBarWrapper/EnergyBar").GetComponent<Image>();

                var panel = settingsPanelObjectGrabber.ComboPanel;
                lines = panel.transform.GetComponentsInChildren<ImageView>();
                comboText = panel.transform.Find("ComboCanvas/NumText").GetComponent<CurvedTextMeshPro>();
                lines[0].SetField("_gradient", true);
                lines[1].SetField("_gradient", true);

                progressPanelImages = settingsPanelObjectGrabber.ProgressPanel.transform.GetComponentsInChildren<Image>();
                var textsToModify = settingsPanelObjectGrabber.ProgressPanel.transform.GetComponentsInChildren<CurvedTextMeshPro>();
                textsToModify[2].text = "0";
                textsToModify[3].text = "01";

                StartCoroutine(FinalPositionPanelSetup());
            }
            catch (System.Exception ex)
            {
                log.Logger.Error(ex);
            }

            modSettingsViewController.Trigger();
            yield break;
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
            grabber.transform.position = new Vector3(0, -500, 0);
            modSettingsViewController.visibilityEvent -= UpdatePanelVisibility;
        }

        public void LateUpdate()
        {
            if (!settingsPanelObjectGrabber.isCompleted) return;

            if (!EnablePreviews)
            {
                settingsPanelObjectGrabber.MultiplierPanel.transform.localPosition = new Vector3(0, -1000, 0);
                settingsPanelObjectGrabber.EnergyPanel.SetActive(false);
                settingsPanelObjectGrabber.ComboPanel.SetActive(false);
                settingsPanelObjectGrabber.ProgressPanel.SetActive(false);
                mockPositionPanel.transform.localPosition = new Vector3(0, -1000, 0);
                return;
            }

            int tab = modSettingsViewController.selectedTab;
            switch (tab)
            {
                case 0:
                    if (isCoroutineOn8x)
                    {
                        if (config.MultiConfig.Rainbow8x)
                            multiplierCircles[0].color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1), 1, 1));
                        else multiplierCircles[0].color = config.MultiConfig.Eight.ColorWithAlpha(0.25f);
                    }
                    break;
                case 1:
                    UpdateEnergyBar();
                    if (fillAmount == 1)
                    {
                        if (config.EnergyConfig.RainbowFull)
                            energyBar.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1), 1, 1));
                        else energyBar.color = config.EnergyConfig.High;
                    }
                    break;
                case 2:
                    UpdateComboPanel();
                    break;
                case 3:
                    UpdateProgressPanel();
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
        }

        private void UpdatePanelVisibility(int tab)
        {
            var host = settingsPanelObjectGrabber;

            switch (tab)
            {
                case 0:
                    host.MultiplierPanel.transform.localPosition = Vector3.zero;
                    host.EnergyPanel.SetActive(false);
                    host.ComboPanel.SetActive(false);
                    host.ProgressPanel.SetActive(false);
                    mockPositionPanel.transform.localPosition = new Vector3(0, -1000, 0);
                    break;
                case 1:
                    host.MultiplierPanel.transform.localPosition = new Vector3(0, -1000, 0);
                    host.EnergyPanel.SetActive(true);
                    host.ComboPanel.SetActive(false);
                    host.ProgressPanel.SetActive(false);
                    mockPositionPanel.transform.localPosition = new Vector3(0, -1000, 0);
                    break;
                case 2:
                    host.MultiplierPanel.transform.localPosition = new Vector3(0, -1000, 0);
                    host.EnergyPanel.SetActive(false);
                    host.ComboPanel.SetActive(true);
                    host.ProgressPanel.SetActive(false);
                    mockPositionPanel.transform.localPosition = new Vector3(0, -1000, 0);

                    comboText.text = new System.Random().Next(0, 250).ToString();
                    break;
                case 3:
                    host.MultiplierPanel.transform.localPosition = new Vector3(0, -1000, 0);
                    host.EnergyPanel.SetActive(false);
                    host.ComboPanel.SetActive(false);
                    host.ProgressPanel.SetActive(true);
                    mockPositionPanel.transform.localPosition = new Vector3(0, -1000, 0);
                    break;
                case 4:
                    host.MultiplierPanel.transform.localPosition = new Vector3(0, -1000, 0);
                    host.EnergyPanel.SetActive(false);
                    host.ComboPanel.SetActive(false);
                    host.ProgressPanel.SetActive(false);
                    mockPositionPanel.transform.localPosition = Vector3.zero;
                    break;
                case 5:
                    host.MultiplierPanel.transform.localPosition = new Vector3(0, -1000, 0);
                    host.EnergyPanel.SetActive(false);
                    host.ComboPanel.SetActive(false);
                    host.ProgressPanel.SetActive(false);
                    mockPositionPanel.transform.localPosition = new Vector3(0, -1000, 0);
                    break;
            }
        }

        private void FinalMultiplierSetup()
        {
            var panel = settingsPanelObjectGrabber.MultiplierPanel;
            multiplierText = panel.transform.GetComponentsInChildren<CurvedTextMeshPro>().Last();
            multiplierCircles = panel.transform.GetComponentsInChildren<Image>();

            multiplierCircles[1].fillAmount = 0.5f;
            multiplierCircles[1].color = config.MultiConfig.One;
            multiplierCircles[0].color = config.MultiConfig.One.ColorWithAlpha(0.25f);

            StartCoroutine(MultiplierPreviewCoroutine());
        }

        private IEnumerator MultiplierPreviewCoroutine()
        {
            if (!settingsPanelObjectGrabber.MultiplierPanel) yield break;
            isCoroutineOn8x = false;
            multiplierCircles[1].fillAmount = 0.5f;

            multiplierCircles[0].color = config.MultiConfig.One.ColorWithAlpha(0.25f);
            multiplierCircles[1].color = config.MultiConfig.One;
            multiplierText.text = "1";
            yield return new WaitForSecondsRealtime(1f);

            multiplierCircles[0].color = config.MultiConfig.Two.ColorWithAlpha(0.25f);
            multiplierCircles[1].color = config.MultiConfig.Two;
            multiplierText.text = "2";
            yield return new WaitForSecondsRealtime(1f);

            multiplierCircles[0].color = config.MultiConfig.Four.ColorWithAlpha(0.25f);
            multiplierCircles[1].color = config.MultiConfig.Four;
            multiplierText.text = "4";
            yield return new WaitForSecondsRealtime(1f);

            isCoroutineOn8x = true;
            multiplierText.text = "8";
            multiplierCircles[1].fillAmount = 0;
            if (!config.MultiConfig.Rainbow8x)
                multiplierCircles[0].color = config.MultiConfig.Eight.ColorWithAlpha(0.25f);
            yield return new WaitForSecondsRealtime(1f);

            yield return MultiplierPreviewCoroutine();
        }

        private void UpdateEnergyBar()
        {
            fillAmount = modSettingsViewController.energyFillAmount;
            energyBar.rectTransform.anchorMax = new Vector2(fillAmount, 1f);

            if (fillAmount == 0.5f) energyBar.color = config.EnergyConfig.Mid;

            if (fillAmount > 0.5f && fillAmount < 1) energyBar.color = HSBColor.Lerp(
                HSBColor.FromColor(config.EnergyConfig.Mid), 
                HSBColor.FromColor(config.EnergyConfig.High), 
                (fillAmount - 0.5f) * 2).ToColor();

            if (fillAmount < 0.5f) energyBar.color = HSBColor.Lerp(
                HSBColor.FromColor(config.EnergyConfig.Low), 
                HSBColor.FromColor(config.EnergyConfig.Mid), 
                fillAmount * 2).ToColor();
        }

        private void UpdateComboPanel()
        {
            if (config.ComboConfig.UseGradient)
            {
                lines[0].color0 = config.ComboConfig.TopLeft;
                lines[0].color1 = config.ComboConfig.TopRight;

                if (config.ComboConfig.MirrorOnBottom)
                {
                    lines[1].color0 = config.ComboConfig.TopRight;
                    lines[1].color1 = config.ComboConfig.TopLeft;
                }
                else
                {
                    lines[1].color0 = config.ComboConfig.BottomLeft;
                    lines[1].color1 = config.ComboConfig.BottomRight;
                }
            }
            else
            {
                lines[0].color0 = config.ComboConfig.TopLine;
                lines[0].color1 = config.ComboConfig.TopLine;
                lines[1].color0 = config.ComboConfig.BottomLine;
                lines[1].color1 = config.ComboConfig.BottomLine;
            }
        }

        private void UpdateProgressPanel()
        {
            var fill = modSettingsViewController.progressFillAmount;
            progressPanelImages[1].color = config.ProgressConfig.BG.ColorWithAlpha(0.25f);
            progressPanelImages[2].color = config.ProgressConfig.Handle;

            if (config.ProgressConfig.DisplayType == "Original")
            {
                progressPanelImages[0].rectTransform.anchorMax = new Vector2(0.5f, 1);
                progressPanelImages[2].transform.localPosition = Vector3.zero;

                progressPanelImages[0].color = config.ProgressConfig.Fill;
            }
            if (config.ProgressConfig.DisplayType == "Lerp")
            {
                var modifiedFill = (fill - 0.5f) * 50;
                progressPanelImages[0].rectTransform.anchorMax = new Vector2(fill, 1);
                progressPanelImages[2].transform.localPosition = new Vector3(modifiedFill, 0, 0);

                progressPanelImages[0].color = HSBColor.Lerp(
                    HSBColor.FromColor(config.ProgressConfig.StartColor),
                    HSBColor.FromColor(config.ProgressConfig.EndColor),
                    fill).ToColor();
            }
        }

        private IEnumerator FinalPositionPanelSetup()
        {
            yield return new WaitUntil(() => mockPositionPanel.isDoneSettingUp);
            StartCoroutine(PositionPanelCoroutine());
        }

        private IEnumerator PositionPanelCoroutine()
        {
            if (!mockPositionPanel) yield break;
            var position = mockPositionPanel.positionText;
            var players = mockPositionPanel.playersText;
            var div = mockPositionPanel.dividerSlash;

            position.color = config.PositionConfig.Fifth;
            position.text = "5";
            players.color = config.PositionConfig.Fifth.ColorWithAlpha(0.25f);
            div.color = config.PositionConfig.Fifth.ColorWithAlpha(0.25f);
            yield return new WaitForSecondsRealtime(1f);

            position.color = config.PositionConfig.Fourth;
            position.text = "4";
            players.color = config.PositionConfig.Fourth.ColorWithAlpha(0.25f);
            div.color = config.PositionConfig.Fourth.ColorWithAlpha(0.25f);
            yield return new WaitForSecondsRealtime(1f);

            position.color = config.PositionConfig.Third;
            position.text = "3";
            players.color = config.PositionConfig.Third.ColorWithAlpha(0.25f);
            div.color = config.PositionConfig.Third.ColorWithAlpha(0.25f);
            yield return new WaitForSecondsRealtime(1f);

            position.color = config.PositionConfig.Second;
            position.text = "2";
            players.color = config.PositionConfig.Second.ColorWithAlpha(0.25f);
            div.color = config.PositionConfig.Second.ColorWithAlpha(0.25f);
            yield return new WaitForSecondsRealtime(1f);

            position.color = config.PositionConfig.First;
            position.text = "1";
            players.color = config.PositionConfig.First.ColorWithAlpha(0.25f);
            div.color = config.PositionConfig.First.ColorWithAlpha(0.25f);
            yield return new WaitForSecondsRealtime(1f);

            yield return PositionPanelCoroutine();
        }
    }
#pragma warning restore CS0649
}
