using UITweaks.Config;
using UITweaks.Models;
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
        }

        /// <summary>
        /// Shorthand function for quickly binding configs in Zenject.<br></br>
        /// <typeparamref name="T"/> must derive from either <see cref="ConfigBase"/> or some other POCO.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        private void BindConfig<T>(T instance) where T : ConfigBase
        {
            Container.Bind<T>().FromInstance(instance).AsCached();
            //UnityEngine.Debug.Log("Bound " + typeof(T));
        }
    }
}
