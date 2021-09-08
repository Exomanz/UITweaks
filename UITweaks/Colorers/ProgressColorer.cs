using SiraUtil.Tools;
using System.Collections.Generic;
using UITweaks.Configuration;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UITweaks.Colorers
{
    public class ProgressColorer : MonoBehaviour
    {
#pragma warning disable CS0649, CS0169
        [Inject] AudioTimeSyncController audioController;
        [Inject] SongProgressUIController progressPanel;
        [Inject] ProgressConfig config;
        [Inject] SiraLog log;
#pragma warning restore CS0649, CS0169

        List<Image> barComponents = new List<Image>(2);

        public void Start()
        {
            log.Logger.Debug("ProgressColorer:Start()");
            transform.SetParent(progressPanel.transform);

            foreach (Image x in progressPanel.transform.GetComponentsInChildren<Image>())
            {
                if (x.name != "BG")
                {
                    // [0] Progress, [1] Background, [2] Handle 
                    barComponents.Add(x);
                    //log.Logger.Info(x.name);
                }
            }

            if (config.DisplayType == "Original")
                barComponents[0].color = config.Fill;
            barComponents[1].color = config.BG.ColorWithAlpha(0.25f);
            barComponents[2].color = config.Handle;
        }

        public void Update()
        {
            if (config.DisplayType == "Lerp")
            {
                barComponents[0].color = HSBColor.Lerp(
                    HSBColor.FromColor(config.StartColor), 
                    HSBColor.FromColor(config.EndColor), 
                    audioController.songTime / audioController.songLength).ToColor();
            }
        }
    }
}
