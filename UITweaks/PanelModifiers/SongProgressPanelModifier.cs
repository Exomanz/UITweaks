using HMUI;
using System.Collections.Generic;
using UITweaks.Models;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class SongProgressPanelModifier : PanelModifier
    {
        private SongProgressUIController Controller = null!;
        private AudioTimeSyncController SyncController = null!;
        private Config.Progress Config = null!;
        private List<ImageView> BarComponents = null!;

        [Inject] internal void ModifierInit(SongProgressUIController spuic, AudioTimeSyncController atsc, Config.Progress c)
        {
            Logger.Logger.Debug("ProgressModifier:ModifierInit()");
            Controller = spuic;
            SyncController = atsc;
            Config = c;
            BarComponents = new();

            transform.SetParent(spuic.transform);
            ModPanel();
        }

        protected override void ModPanel()
        {
            foreach (ImageView x in Controller.GetComponentsInChildren<ImageView>())
            {
                if (x.name != "BG")
                {
                    // [0] Fill, [1] BG, [2] Handle
                    BarComponents.Add(x);
                }
            }

            if (!Config.UseFadeDisplayType)
            {
                BarComponents[0].color = Config.Fill;
            }
            BarComponents[1].color = Config.BG.ColorWithAlpha(0.25f);
            BarComponents[2].color = Config.Handle;
        }

        private void Update()
        {
            // The performance impact of this is unmeasured but probably negligable. I do want to find a way to do this without using Update()
            if (Config.UseFadeDisplayType)
            {
                BarComponents[0].color = HSBColor.Lerp(
                    HSBColor.FromColor(Config.StartColor),
                    HSBColor.FromColor(Config.EndColor),
                    SyncController.songTime / SyncController.songLength).ToColor();
            }
        }

        protected override void OnDestroy()
        {
            Controller = null!;
            SyncController = null!;
            Config = null!;
            BarComponents = null!;
        }
    }
}
