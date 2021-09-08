using IPA;
using IPA.Config.Stores;
using IPAConfig = IPA.Config.Config;
using IPALogger = IPA.Logging.Logger;
using SiraUtil;
using SiraUtil.Zenject;
using UITweaks.Installers;

namespace UITweaks
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static MainConfig MainConfig { get; private set; }

        [Init] public Plugin(IPALogger logger, IPAConfig config, Zenjector zenject)
        {
            MainConfig = config.Generated<MainConfig>();

            zenject.On<PCAppInit>().Pseudo(Container => Container.BindLoggerAsSiraLogger(logger));
            zenject.OnApp<ConfigInstaller>();
            zenject.OnMenu<MenuManagerInstaller>();

            // Single-player
            zenject.OnGame<GameInstaller>().Expose<ScoreMultiplierUIController>()
                .Expose<GameEnergyUIPanel>()
                .Expose<ComboUIController>()
                .Expose<SongProgressUIController>().ShortCircuitForTutorial().ShortCircuitForMultiplayer();

            // Multi-player
            zenject.OnGame<GameInstaller>(false).Expose<ScoreMultiplierUIController>()
                .Expose<GameEnergyUIPanel>()
                .Expose<ComboUIController>()
                .Expose<SongProgressUIController>()
                .Expose<MultiplayerPositionHUDController>().OnlyForMultiplayer();
        }

        [OnEnable] public void Enable() { }

        [OnDisable] public void Disable() { }
    }
}
