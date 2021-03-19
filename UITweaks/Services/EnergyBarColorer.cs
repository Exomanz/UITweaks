using HMUI;
using UnityEngine;
using Zenject;

namespace UITweaks.Services
{
    public class EnergyBarColorer : IInitializable, ITickable
    {
        PluginConfig.EnergyBarConfig _config;
        GameEnergyCounter _energy;
        ImageView _bar;

        public EnergyBarColorer(PluginConfig.EnergyBarConfig config) =>
            _config = config;

        public void Initialize()
        {
            var bar = GameObject.Find("EnergyBar").GetComponent<ImageView>();
            _bar = bar;

            var energy = GameObject.Find("GameplayData").GetComponent<GameEnergyCounter>();
            _energy = energy;
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
