using SiraUtil.Zenject;
using Zenject;

namespace UITweaks.Installers
{
    public class UIAppInstaller : Installer<UIAppInstaller>
    {
        readonly PluginConfig _config;
        public UIAppInstaller(PluginConfig config) => _config = config;

        public override void InstallBindings()
        {
            Container.Bind<PluginConfig.MultiplierConfig>().FromInstance(_config.Multiplier).AsSingle();
            Container.Bind<PluginConfig.EnergyBarConfig>().FromInstance(_config.EnergyBar).AsSingle();
            Container.Bind<PluginConfig.ComboConfig>().FromInstance(_config.Combo).AsSingle();
        }
    }
}
