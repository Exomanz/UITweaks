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
        public Plugin(Logger logger, IPAConfig config, Zenjector zen)
        {
            PluginConfig pConf = config.Generated<PluginConfig>();

            zen.OnApp<UIAppInstaller>().WithParameters(logger, pConf);
            zen.OnMenu<UIMenuInstaller>();

            //Standard And Campaign
            zen.OnGame<UIGameInstaller>().Expose<ComboUIController>().Expose<GameEnergyUIPanel>()
                .Expose<ScoreMultiplierUIController>().ShortCircuitForTutorial().ShortCircuitForMultiplayer();

            //Multiplayer
            zen.OnGame<UIGameInstaller>(false).Expose<ComboUIController>().Expose<GameEnergyUIPanel>()
                .Expose<ScoreMultiplierUIController>().OnlyForMultiplayer();
        }

        [OnEnable]
        public void OnEnable() { }

        [OnDisable]
        public void OnDisable() { }
    }
}
