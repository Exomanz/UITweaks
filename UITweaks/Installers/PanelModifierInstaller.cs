using UITweaks.Models;
using UITweaks.PanelModifiers;
using UnityEngine;
using Zenject;

namespace UITweaks.Installers
{
    public class PanelModifierInstaller : Installer
    {
        public override void InstallBindings()
        {
            PluginConfig config = Container.Resolve<PluginConfig>();

            if (config.Multiplier.Enabled) BindPanelModifier<ScoreMultiplierPanelModifier>();
            if (config.Energy.Enabled) BindPanelModifier<EnergyBarPanelModifier>();
            if (config.Combo.Enabled) BindPanelModifier<ComboPanelModifier>();
            if (config.Progress.Enabled) BindPanelModifier<SongProgressPanelModifier>();

            BindPanelModifier<LegacyPanelModifier>();
#if DEBUG
            Container.Bind<LegacyPanelModifier.AprilFools>().FromNewComponentOn(new GameObject("LMAO")).AsSingle().NonLazy();
#else
            if (Plugin.isAprilFools && config.AprilFools)
            {
                Container.Bind<LegacyPanelModifier.AprilFools>().FromNewComponentOn(new GameObject("LMAO")).AsSingle().NonLazy();
            }
#endif
        }

        /// <summary>
        /// Shorthand function for binding <see cref="PanelModifier"/>'s. Is this necessary? Probably not...
        /// <br></br><typeparamref name="T"/> is <see cref="PanelModifier"/> or any other class that inherits <see cref="MonoBehaviour"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void BindPanelModifier<T>() where T : PanelModifier
        {
            Container.Bind<T>().FromNewComponentOn(new GameObject("PanelModifier")).AsSingle().NonLazy();
        }
    }
}
