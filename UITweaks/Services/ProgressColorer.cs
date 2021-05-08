using HMUI;
using SiraUtil.Tools;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UITweaks.Services
{
    public class ProgressColorer : IInitializable
    {
        //..ctor stuff
        PluginConfig.ProgressConfig _config;
        SongProgressUIController _ui;
        SiraLog _log;

        public ProgressColorer(PluginConfig.ProgressConfig config, SongProgressUIController ui, SiraLog log)
        {
            _config = config;
            _ui = ui;
            _log = log;
        }

        public void Initialize()
        {
            var progressGO = _ui.transform.GetComponentInChildren<Image>();
            var slider = _ui.transform.Find("Slider/Handle Slide Area/Handle").GetComponent<Image>();
            var bg = _ui.transform.Find("Slider/Background").GetComponent<Image>();

            progressGO.color = _config.FillColor;
            slider.color = _config.HandleColor;
            bg.color = _config.BackgroundColor.ColorWithAlpha(0.25f);
        }
    }
}
