using UITweaks.Models;
using UITweaks.Utilities.SettableSettings;
using IPA.Loader;
using Zenject;

namespace UITweaks.Installers
{
    public class AppConfigInstaller : Installer
    {
        private readonly PluginConfig config;

        [Inject] public AppConfigInstaller(PluginConfig pluginConfig)
        {
            this.config = pluginConfig;
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

#if HECK
            if (PluginManager.GetPlugin("Heck") != null)
            {
                Container.BindInterfacesAndSelfTo<UITweaksSettableSettings>().AsSingle().NonLazy();
            }
#endif
        }

        /// <summary>
        /// Shorthand function for quickly binding configs in Zenject.<br></br>
        /// <typeparamref name="T"/> is <see cref="UITweaksConfigBase"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        private void BindConfig<T>(T instance) where T : UITweaksConfigBase
        {
            Container.Bind<T>().FromInstance(instance).AsCached();
            Container.Bind<UITweaksConfigBase>().To<T>().FromInstance(instance).AsCached();
        }
    }
}
