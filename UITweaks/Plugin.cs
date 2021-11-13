using IPA;
using IPA.Config.Stores;
using IPAConfig = IPA.Config.Config;
using IPALogger = IPA.Logging.Logger;
using SiraUtil.Zenject;
using UITweaks.Installers;

namespace UITweaks
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static MainConfig MainConfig { get; private set; }
        internal static IPALogger Logger { get; private set; }

        [Init] public Plugin(IPALogger logger, IPAConfig config, Zenjector zenject)
        {
            MainConfig = config.Generated<MainConfig>();
            Logger = logger;

            zenject.OnApp<ConfigInstaller>();
            zenject.OnMenu<MenuManagerUIInstaller>();

            // Single-player
            zenject.OnGame<PanelModifierInstaller>().Expose<ScoreMultiplierUIController>()
                .Expose<GameEnergyUIPanel>()
                .Expose<ComboUIController>()
                .Expose<SongProgressUIController>()
                .Expose<ImmediateRankUIPanel>().ShortCircuitForTutorial().ShortCircuitForMultiplayer();

            // Multi-player
            zenject.OnGame<PanelModifierInstaller>(false).Expose<ScoreMultiplierUIController>()
                .Expose<GameEnergyUIPanel>()
                .Expose<ComboUIController>()
                .Expose<SongProgressUIController>()
                .Expose<MultiplayerPositionHUDController>()
                .Expose<ImmediateRankUIPanel>().OnlyForMultiplayer();
        }

        [OnEnable] public void Enable() { }

        [OnDisable] public void Disable() { }
    }
}
