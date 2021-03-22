using HMUI;
using IPA.Utilities;
using UnityEngine;
using Zenject;

namespace UITweaks.Services
{
    public class ComboColorer : IInitializable
    {
        PluginConfig.ComboConfig _config;
        ImageView[] _fcLines;

        public ComboColorer(PluginConfig.ComboConfig config) =>
            _config = config;

        public void Initialize()
        { 
            var comboFCLines = GameObject.Find("ComboPanel").GetComponentsInChildren<ImageView>();
            _fcLines = comboFCLines;

            ReflectionUtil.SetField(_fcLines[0], "_gradient", true);
            _fcLines[0].color0 = new Color(1f, 1f, 0.75f);
            _fcLines[0].color1 = Color.yellow;

            ReflectionUtil.SetField(_fcLines[1], "_gradient", true);
            _fcLines[1].color0 = Color.yellow;
            _fcLines[1].color1 = new Color(1f, 1f, 0.75f);
        }
    }
}
