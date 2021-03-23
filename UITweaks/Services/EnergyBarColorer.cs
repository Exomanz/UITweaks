using HMUI;
using IPA.Utilities;
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

        public EnergyBarColorer(PluginConfig.EnergyBarConfig config, IGameEnergyCounter gameEnergyCounter, GameEnergyUIPanel gameEnergyUIPanel)
        {
            _energy = gameEnergyCounter;
            _panel = gameEnergyUIPanel;
            _config = config;
        }

        public void Initialize()
        {
            var bar = _panel.GetField<Image, GameEnergyUIPanel>("_energyBar");
            _bar = bar;
        }

        public void Tick()
        {             
            if (_energy.energy < 0.5f)
                _bar.color = Color.Lerp(_config.LowEnergyColor, Color.white, _energy.energy * 2);
            if (_energy.energy > 0.5)
                _bar.color = Color.Lerp(Color.white, _config.HighEnergyColor, (_energy.energy - .5f) * 2);
        }
    }
}
