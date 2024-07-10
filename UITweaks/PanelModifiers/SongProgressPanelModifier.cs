using HMUI;
using System.Collections.Generic;
using UITweaks.Config;
using UITweaks.Models;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class SongProgressPanelModifier : PanelModifierBase
    {
        [InjectOptional] private readonly StandardGameplaySceneSetupData gameplaySceneSetupData;
        [Inject] private readonly AudioTimeSyncController audioTimeSyncController;
        [Inject] private readonly ProgressConfig progressConfig;

        private SongProgressUIController songProgressUIController;
        private List<ImageView> barComponents = new List<ImageView>();
        private bool canBeUsed = true;

        [Inject] protected override void Init(string _)
        {
            base.Init();
            songProgressUIController = base.gameHUDController.GetComponentInChildren<SongProgressUIController>();

            base.parentPanel = songProgressUIController.gameObject;
            base.config = progressConfig;

            this.transform.SetParent(parentPanel?.transform);
            this.ModPanel();
        }

        protected override void ModPanel()
        {
            base.ModPanel();

            if (gameplaySceneSetupData?.beatmapKey.beatmapCharacteristic.containsRotationEvents == true)
            {
                logger.Debug("Selected map is 360/90. Disabling the SongProgressPanelModifier");
                canBeUsed = false;
                return;
            }

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

        public void Update()
        {
            if (!canBeUsed) return;

            if (progressConfig.UseFadeDisplayType)
            {
                barComponents[0].color = HSBColor.Lerp(
                    HSBColor.FromColor(progressConfig.StartColor),
                    HSBColor.FromColor(progressConfig.EndColor),
                    audioTimeSyncController.songTime / audioTimeSyncController.songLength)
                    .ToColor();
            }
        }

        protected override void OnDestroy() => base.OnDestroy();
    }
}
