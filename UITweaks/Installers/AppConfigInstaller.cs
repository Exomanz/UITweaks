using UITweaks.Models;
using UITweaks.Utilities.SettableSettings;
using IPA.Loader;
using Zenject;

namespace UITweaks.Installers
{
    public class AppConfigInstaller : Installer
    {
        private PluginConfig Config = null!;

        public AppConfigInstaller(PluginConfig c) => Config = c;

        public override void InstallBindings()
        {
            Container.Bind<PluginConfig>().FromInstance(Config).AsCached();

            BindConfig(Config.Multiplier);
            BindConfig(Config.Energy);
            BindConfig(Config.Combo);
            BindConfig(Config.Progress);
            BindConfig(Config.Position);
            BindConfig(Config.Misc);

#if HECK
            if (PluginManager.GetPlugin("Heck") != null)
            {
                Container.BindInterfacesAndSelfTo<UITweaksSettableSettings>().AsSingle().NonLazy();
            }
#endif
        }

        /// <summary>
        /// Shorthand function for quickly binding configs in Zenject.<br></br>
        /// <typeparamref name="T"/> must derive from either <see cref="ConfigBase"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        private void BindConfig<T>(T instance) where T : ConfigBase
        {
            Container.Bind<T>().FromInstance(instance).AsCached();
            Container.Bind<ConfigBase>().To<T>().FromInstance(instance).AsCached();
        }
    }
}
