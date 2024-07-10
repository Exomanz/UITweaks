using IPA;
using IPA.Config.Stores;
using IPA.Utilities;
using SiraUtil.Zenject;
using System;
using System.Linq;
using UITweaks.Installers;
using IPAConfig = IPA.Config.Config;
using IPALogger = IPA.Logging.Logger;

namespace UITweaks
{
    [Plugin(RuntimeOptions.DynamicInit), NoEnableDisable]
    public class Plugin
    {
        public static bool APRIL_FOOLS 
        { 
            get
            {
                if (Environment.GetCommandLineArgs().Any(x => x.ToLower() == "--uitweaks.aprilfools"))
                    return true;

                DateTime time = Utils.CanUseDateTimeNowSafely ? DateTime.Now : DateTime.UtcNow;
                return time.Month == 4 && time.Day == 1;
            } 
        }

        [Init]
        public Plugin(IPALogger logger, IPAConfig config, Zenjector zenject)
        {
            zenject.UseLogger(logger);
            zenject.UseMetadataBinder<Plugin>();

            zenject.Expose<CoreGameHUDController>("Environment");

            zenject.Install<TweaksAppInstaller>(Location.App, config.Generated<PluginConfig>());
            zenject.Install<TweaksMenuInstaller>(Location.Menu);
            zenject.Install<PanelModifierInstaller>(Location.StandardPlayer | Location.CampaignPlayer);
        }
    }
}
