using IPA;
using IPA.Config.Stores;
using IPAConfig = IPA.Config.Config;
using IPALogger = IPA.Logging.Logger;
using SiraUtil.Zenject;
using UITweaks.Installers;

namespace UITweaks
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        [Init]
        public Plugin(IPALogger logger, IPAConfig config, Zenjector zen)
        {
            Logger.log = logger;
            PluginConfig pConf = config.Generated<PluginConfig>();

            zen.OnApp<UIAppInstaller>().WithParameters(pConf);
            zen.OnMenu<UIMenuInstaller>();
            zen.OnGame<UIGameInstaller>().OnlyForStandard();
        }

        [OnEnable]
        public void OnEnable() { }

        [OnDisable]
        public void OnDisable() { }
    }
}
