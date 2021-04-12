using IPA.Utilities;
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
        Image _bar;
        SiraLog _log;

        public EnergyBarColorer(PluginConfig.EnergyBarConfig config, IGameEnergyCounter energy, GameEnergyUIPanel panel
            , SiraLog log)
        {
            _config = config;
            _energy = energy;
            _panel = panel;
            _log = log;
        }

        public void Initialize()
        {
            var bar = _panel.GetField<Image, GameEnergyUIPanel>("_energyBar");
            _bar = bar;
            _log.Logger.Debug("Got EnergyBar");
        }

        public void Tick()
        {
            if (_energy.energy == 0.5f)
                _bar.color = Color.white;
            if (_energy.energy < 0.5f)
                _bar.color = Color.Lerp(_config.LowEnergyColor, Color.white, _energy.energy * 2);
            if (_energy.energy > 0.5)
                _bar.color = Color.Lerp(Color.white, _config.HighEnergyColor, (_energy.energy - .5f) * 2);
            if (_energy.energy == 1 && _config.RainbowAnim)
                _bar.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * .5f, 1), 1, 1));
        }
    }
}
