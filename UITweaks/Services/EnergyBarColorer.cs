using SiraUtil.Tools;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UITweaks.Services
{
    public class EnergyBarColorer : IInitializable, ITickable
    {
        PluginConfig.EnergyBarConfig _config;
        IGameEnergyCounter _energy;
        GameEnergyUIPanel _panel;
        GameplayModifiers _mods;
        SiraLog _log;

        //GameObjects
        Image[] _barImages;

        public EnergyBarColorer(PluginConfig.EnergyBarConfig config, IGameEnergyCounter energy, GameEnergyUIPanel panel
            , SiraLog log, GameplayModifiers mods)
        {
            _config = config;
            _energy = energy;
            _panel = panel;
            _log = log;
            _mods = mods;
        }

        public void Initialize() => _barImages = _panel.transform.GetComponentsInChildren<Image>();

        /// Standard or Insta-Fail => Use index [3].
        /// Battery Energy => Use indexes [3] through [6], left to right

        public void Tick()
        {
            if (_barImages is null) return;

            if (_mods.energyType == GameplayModifiers.EnergyType.Bar)
            {
                if (_energy.energy == 0.5f)
                    _barImages[3].color = _config.MiddleEnergyColor;
                if (_energy.energy < 0.5f)
                    _barImages[3].color = Color.Lerp(_config.LowEnergyColor, _config.MiddleEnergyColor, _energy.energy * 2);
                if (_energy.energy > 0.5)
                    _barImages[3].color = Color.Lerp(_config.MiddleEnergyColor, _config.HighEnergyColor, (_energy.energy - .5f) * 2);
                if (_energy.energy == 1 && _config.RainbowAnim)
                    _barImages[3].color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * .5f, 1), 1, 1));
            }

            if (_mods.energyType == GameplayModifiers.EnergyType.Battery)
            {
                _barImages[3].color = _config.LowEnergyColor;
                _barImages[4].color = Color.Lerp(_config.LowEnergyColor, _config.MiddleEnergyColor, 0.5f);
                _barImages[5].color = Color.Lerp(_config.MiddleEnergyColor, _config.HighEnergyColor, 0.5f);
                _barImages[6].color = _config.HighEnergyColor;
            }

            if (_mods.instaFail)
            {
                if (_config.RainbowAnim) 
                    _barImages[3].color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * .5f, 1), 1, 1));
                else _barImages[3].color = _config.HighEnergyColor;
            }
        }
    }
}
