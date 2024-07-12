﻿using UITweaks.Models;
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
            BindPanelDecorator<ScoreMultiplierPanelDecorator>();
            BindPanelDecorator<EnergyBarPanelDecorator>();
            BindPanelDecorator<ComboPanelDecorator>();
            BindPanelDecorator<SongProgressPanelDecorator>();
            BindPanelDecorator<ExtraPanelDecorator>();

            if (Plugin.APRIL_FOOLS && config.AllowAprilFools)
                Container.Bind<ExtraPanelDecorator.AprilFools>().FromNewComponentOn(new GameObject("UITweaks-AprilFoolsController")).AsSingle().NonLazy();
        }

        /// <summary>
        /// Shorthand function for binding <see cref="PanelDecoratorBase"/> classes.
        /// </summary>
        /// <typeparam name="T">Any class which derives from <see cref="PanelDecoratorBase"/>
        private void BindPanelDecorator<T>() where T : PanelDecoratorBase
        {
            Container.Bind<T>().FromNewComponentOn(new GameObject(typeof(T).Name)).AsSingle().NonLazy();
        }
    }
}