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
                if (Environment.GetCommandLineArgs().Any(x => x.ToLower() == "--uitweaks-aprilfools"))
                    return true;

                DateTime time = Utils.CurrentTime();
                return time.Month == 4 && time.Day == 1;
            }
        }

        [Init]
        public Plugin(IPALogger logger, IPAConfig config, Zenjector zenject)
        {
            zenject.UseLogger(logger);
            zenject.UseMetadataBinder<Plugin>();

            // Singleplayer and Campaign
            zenject.Expose<CoreGameHUDController>("Environment");

            // Multiplayer
            zenject.Expose<CoreGameHUDController>("IsActiveObjects");
            zenject.Expose<MultiplayerPositionHUDController>("IsActiveObjects");

            zenject.Install<TweaksAppInstaller>(Location.App, config.Generated<PluginConfig>());
            zenject.Install<TweaksMenuInstaller>(Location.Menu);
            zenject.Install<TweaksPanelDecoratorInstaller>(Location.Player);
        }
    }
}
