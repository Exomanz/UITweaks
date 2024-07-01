using UITweaks.Models;
using UITweaks.PanelModifiers;
using UnityEngine;
using Zenject;

namespace UITweaks.Installers
{
    public class PanelModifierInstaller : Installer
    {
        [Inject] private readonly PluginConfig config;

        public override void InstallBindings()
        {
            if (config.Multiplier.Enabled) 
                BindPanelModifier<ScoreMultiplierPanelModifier>();

            if (config.Energy.Enabled) 
                BindPanelModifier<EnergyBarPanelModifier>();

            if (config.Combo.Enabled) 
                BindPanelModifier<ComboPanelModifier>();

            if (config.Progress.Enabled) 
                BindPanelModifier<SongProgressPanelModifier>();

            BindPanelModifier<ExtraPanelModifiers>();

            if (Plugin.APRIL_FOOLS && config.AllowAprilFools)
                Container.Bind<ExtraPanelModifiers.AprilFools>().FromNewComponentOn(new GameObject("UITweaks-AprilFoolsController")).AsSingle().NonLazy();
        }

        /// <summary>
        /// Shorthand function for binding <see cref="PanelModifierBase"/> classes.
        /// <br></br><typeparamref name="T"/> is <see cref="PanelModifierBase"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void BindPanelModifier<T>() where T : PanelModifierBase
        {
            Container.Bind<T>().FromNewComponentOn(new GameObject(typeof(T).Name)).AsSingle().NonLazy();
        }
    }
}
