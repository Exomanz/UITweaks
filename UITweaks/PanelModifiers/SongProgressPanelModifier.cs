using HMUI;
using System.Collections.Generic;
using UITweaks.Config;
using UITweaks.Models;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class SongProgressPanelModifier : PanelModifier
    {
        [InjectOptional] private readonly StandardGameplaySceneSetupData gameplaySceneSetupData;
        [Inject] private readonly SongProgressUIController songProgressUIController;
        [Inject] private readonly AudioTimeSyncController audioTimeSyncController;
        [Inject] private readonly ProgressConfig progressConfig;

        private List<ImageView> barComponents = new List<ImageView>();
        private bool canBeUsed = true;

        [Inject] protected override void Init()
        {
            logger.Debug("SongProgressPanelModifier::Init()");
            base.parentPanel = songProgressUIController.gameObject;
            base.config = progressConfig;

            this.transform.SetParent(parentPanel?.transform);

            if (gameplaySceneSetupData?.beatmapCharacteristic.containsRotationEvents == true)
            {
                logger.Debug("Selected map is 360/90. Disabling the SongProgressPanelModifier");
                canBeUsed = false;
                return;
            }

            this.ModPanel();
        }

        protected override void ModPanel()
        {
            base.ModPanel();

            foreach (ImageView x in songProgressUIController.GetComponentsInChildren<ImageView>())
            {
                if (x.name != "BG")
                {
                    // [0] Fill, [1] BG, [2] Handle
                    barComponents.Add(x);
                }
            }

            if (!progressConfig.UseFadeDisplayType)
            {
                barComponents[0].color = progressConfig.Fill;
            }
            barComponents[1].color = progressConfig.BG.ColorWithAlpha(0.25f);
            barComponents[2].color = progressConfig.Handle;
        }

        private void Update()
        {
            if (!canBeUsed) return;

            if (progressConfig.UseFadeDisplayType)
            {
                barComponents[0].color = HSBColor.Lerp(
                    HSBColor.FromColor(progressConfig.StartColor),
                    HSBColor.FromColor(progressConfig.EndColor),
                    audioTimeSyncController.songTime / audioTimeSyncController.songLength).ToColor();
            }
        }

        protected override void OnDestroy()
        {
            barComponents = null!;
        }
    }
}
