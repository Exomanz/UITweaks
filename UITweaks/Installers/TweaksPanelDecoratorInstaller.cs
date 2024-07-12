using UITweaks.Models;
using UITweaks.PanelModifiers;
using UnityEngine;
using Zenject;

namespace UITweaks.Installers
{
    public class TweaksPanelDecoratorInstaller : Installer
    {
        [Inject] private readonly PluginConfig config;

        public override void InstallBindings()
        {
            if (config.Multiplier.Enabled) 
                BindPanelModifier<ScoreMultiplierPanelDecorator>();

            if (config.Energy.Enabled) 
                BindPanelModifier<EnergyBarPanelDecorator>();

            if (config.Combo.Enabled) 
                BindPanelModifier<ComboPanelDecorator>();

            if (config.Progress.Enabled) 
                BindPanelModifier<SongProgressPanelDecorator>();

            BindPanelModifier<ExtraPanelDecorator>();

            if (Plugin.APRIL_FOOLS && config.AllowAprilFools)
                Container.Bind<ExtraPanelDecorator.AprilFools>().FromNewComponentOn(new GameObject("UITweaks-AprilFoolsController")).AsSingle().NonLazy();
        }

        /// <summary>
        /// Shorthand function for binding <see cref="PanelDecoratorBase"/> classes.
        /// <br></br><typeparamref name="T"/> is <see cref="PanelDecoratorBase"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void BindPanelModifier<T>() where T : PanelDecoratorBase
        {
            Container.Bind<T>().FromNewComponentOn(new GameObject(typeof(T).Name)).AsSingle();
        }
    }
}
