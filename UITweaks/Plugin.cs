using BeatSaberMarkupLanguage.GameplaySetup;
using IPA;
using IPA.Config.Stores;
using IPA.Logging;
using SiraUtil.Zenject;
using UITweaks.Installers;
using IPAConfig = IPA.Config.Config;

namespace UITweaks
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        [Init]
        public Plugin(Logger logger, IPAConfig config, Zenjector zenj)
        {
            PluginConfig pConf = config.Generated<PluginConfig>();

            zenj.OnApp<UIAppInstaller>().WithParameters(logger, pConf);
            zenj.OnMenu<UIMenuInstaller>();

            //Standard, Campaign and Party Modes
            zenj.OnGame<UICoreInstaller>().Expose<ScoreMultiplierUIController>().Expose<GameEnergyUIPanel>()
                .Expose<ComboUIController>().Expose<SongProgressUIController>().ShortCircuitForTutorial().ShortCircuitForMultiplayer();

            //Multiplayer Specific
            zenj.OnGame<UICoreInstaller>(false).Expose<ScoreMultiplierUIController>().Expose<GameEnergyUIPanel>()
                .Expose<ComboUIController>().Expose<SongProgressUIController>().Expose<MultiplayerPositionHUDController>().OnlyForMultiplayer();
        }

        [OnEnable]
        public void Enable() { }

        [OnDisable]
        public void Disable() { }
    }
}
