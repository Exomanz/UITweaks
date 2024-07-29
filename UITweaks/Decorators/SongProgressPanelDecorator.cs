using HMUI;
using System.Collections.Generic;
using UITweaks.Config;
using UITweaks.Models;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class SongProgressPanelDecorator : PanelDecoratorBase
    {
        [InjectOptional] private readonly StandardGameplaySceneSetupData gameplaySceneSetupData;
        [Inject] private readonly AudioTimeSyncController audioTimeSyncController;
        [Inject] private readonly ProgressConfig progressConfig;

        private SongProgressUIController songProgressUIController;
        private readonly List<ImageView> barComponents = new List<ImageView>();

        [Inject]
        protected override void Init()
        {
            songProgressUIController = base.gameHUDController.GetComponentInChildren<SongProgressUIController>();
            ParentPanel = songProgressUIController.gameObject;
            Config = progressConfig;
            transform.SetParent(ParentPanel?.transform);

            ModPanel(this);
        }

        protected override bool ModPanel(in PanelDecoratorBase decorator)
        {
            if (!base.ModPanel(this)) return false;

            if (gameplaySceneSetupData?.beatmapKey.beatmapCharacteristic.containsRotationEvents == true)
            {
                logger.Debug("Selected map is 360/90. Disabling the SongProgressPanelModifier");
                CanBeUsedSafely = false;
                return false;
            }

            foreach (ImageView x in songProgressUIController.GetComponentsInChildren<ImageView>())
            {
                if (x.name != "BG")
                {
                    // [0] Fill, [1] BG, [2] Handle
                    barComponents.Add(x);
                }
            }

            if (progressConfig.Mode == ProgressConfig.DisplayType.Fixed)
            {
                barComponents[0].color = progressConfig.Fill;
            }
            barComponents[1].color = progressConfig.BG.ColorWithAlpha(0.25f);
            barComponents[2].color = progressConfig.Handle;

            return true;
        }

        public void Update()
        {
            if (!CanBeUsedSafely) return;

            if (progressConfig.Mode == ProgressConfig.DisplayType.Fade)
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
