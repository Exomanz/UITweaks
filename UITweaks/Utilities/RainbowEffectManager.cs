using UITweaks.Config;
using UnityEngine;
using Zenject;

namespace UITweaks.Utilities
{
    internal class RainbowEffectManager : ITickable
    {
        [Inject] private readonly MiscConfig miscConfig;

        public Color Rainbow { get; private set; } 

        [Inject] public RainbowEffectManager() { }

        public void Tick()
        {
            this.Rainbow = new HSBColor(
                Mathf.PingPong(Time.time * miscConfig.GlobalRainbowSpeed, 1),
                1,
                1)
                .ToColor();
        }
    }
}
