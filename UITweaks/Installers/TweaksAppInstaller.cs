using UITweaks.Models;
using UITweaks.Utilities;
using UITweaks.Utilities.SettableSettings;
using IPA.Loader;
using Zenject;

namespace UITweaks.Installers
{
    public class TweaksAppInstaller : Installer
    {
        private readonly PluginConfig config;

        [Inject]
        public TweaksAppInstaller(PluginConfig pluginConfig)
        {
            config = pluginConfig;
        }

        public override void InstallBindings()
        {
            Container.Bind<PluginConfig>().FromInstance(config).AsCached();
            BindConfig(config.Multiplier);
            BindConfig(config.Energy);
            BindConfig(config.Combo);
            BindConfig(config.Progress);
            BindConfig(config.Position);
            BindConfig(config.Misc);

            Container.BindInterfacesAndSelfTo<RainbowEffectManager>().AsSingle();

#if HECK
            if (PluginManager.GetPlugin("Heck") != null)
            {
                Container.BindInterfacesAndSelfTo<UITweaksSettableSettings>().AsSingle().NonLazy();
            }
#endif
        }

        /// <summary>
        /// Shorthand function for quickly binding configs in Zenject.<br></br>
        /// </summary>
        /// <typeparam name="T">Any class which derives from <see cref="UITweaksConfigBase"/></typeparam>
        /// <param name="instance">An instance of a <see cref="UITweaksConfigBase"/>. These are usually defined inside of <see cref="PluginConfig"/></param>
        private void BindConfig<T>(T instance) where T : UITweaksConfigBase
        {
            Container.Bind<T>().FromInstance(instance).AsCached();
            Container.Bind<UITweaksConfigBase>().To<T>().FromInstance(instance).AsCached();
        }
    }
}
