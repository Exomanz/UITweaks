using IPA.Logging;
using SiraUtil;
using Zenject;

namespace UITweaks.Installers
{
    public class UIAppInstaller : Installer<UIAppInstaller>
    {
        readonly PluginConfig _config;
        readonly Logger _logger;

        public UIAppInstaller(PluginConfig config, Logger logger)
        {
            _logger = logger;
            _config = config;
        }

        public override void InstallBindings()
        {
            Container.Bind<PluginConfig.MultiplierConfig>().FromInstance(_config.Multiplier).AsSingle();
            Container.Bind<PluginConfig.EnergyBarConfig>().FromInstance(_config.EnergyBar).AsSingle();
            Container.Bind<PluginConfig.ComboConfig>().FromInstance(_config.Combo).AsSingle();
            Container.Bind<PluginConfig.PositionConfig>().FromInstance(_config.Position).AsSingle();
            Container.Bind<PluginConfig.ProgressConfig>().FromInstance(_config.Progress).AsSingle();

            Container.BindLoggerAsSiraLogger(_logger);
        }
    }
}
