using UITweaks.Config;
using UnityEngine;
using Zenject;

namespace UITweaks.Utilities
{
    internal class RainbowEffectManager : ITickable
    {
        [Inject] private readonly MiscConfig miscConfig;

        /// <summary>
        /// The <see cref="Color"/> which controls the rainbow effect that UITweaks uses. All objects which use this effect will be in sync.
        /// </summary>
        /// <remarks>This property is updated every frame.</remarks>
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
