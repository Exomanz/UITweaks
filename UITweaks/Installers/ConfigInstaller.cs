using UITweaks.Configuration;
using Zenject;

namespace UITweaks.Installers
{
    public class ConfigInstaller : Installer
    {
        public override void InstallBindings()
        {
            MainConfig main = Plugin.MainConfig;

            Container.Bind<MainConfig>().FromInstance(main).AsCached();

            Container.Bind<MultiplierConfig>().FromInstance(main.MultiConfig).AsCached();
            Container.Bind<EnergyConfig>().FromInstance(main.EnergyConfig).AsCached();
            Container.Bind<ComboConfig>().FromInstance(main.ComboConfig).AsCached();
            Container.Bind<ProgressConfig>().FromInstance(main.ProgressConfig).AsCached();
            Container.Bind<PositionConfig>().FromInstance(main.PositionConfig).AsCached();
        }
    }
}
