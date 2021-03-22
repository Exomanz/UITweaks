using HMUI;
using UnityEngine;
using Zenject;

namespace UITweaks.Services
{
    public class EnergyBarColorer : IInitializable, ITickable
    {
        PluginConfig.EnergyBarConfig _config;
        IGameEnergyCounter _energy;
        GameEnergyUIPanel _panel;
        ImageView _bar;

        public EnergyBarColorer(PluginConfig.EnergyBarConfig config, IGameEnergyCounter gameEnergyCounter, GameEnergyUIPanel gameEnergyUIPanel)
        {
            _energy = gameEnergyCounter;
            _panel = gameEnergyUIPanel;
            _config = config;
        }

        public void Initialize()
        {
            var bar = _panel.transform.Find("EnergyBar").GetComponent<ImageView>();
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
