using IPA;
using IPA.Config.Stores;
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
                if (Environment.GetCommandLineArgs().Any(x => x.ToLower() == "--uit-aprilfools"))
                    return true;

                DateTime time = DateTime.Now;
                return time.Month == 4 && time.Day == 1;
            } 
        }

        [Init]
        public Plugin(IPALogger logger, IPAConfig config, Zenjector zenject)
        {
            zenject.UseLogger(logger);
            zenject.UseMetadataBinder<Plugin>();

            // Basically expose the entire HUD to Zenject.
            // I could just leave it at the CoreGameHUDController and look everything up then, but that's a lot more work than this.
            zenject.Expose<CoreGameHUDController>("Environment");
            zenject.Expose<GameEnergyUIPanel>("Environment");
            zenject.Expose<ComboUIController>("Environment");
            zenject.Expose<ScoreMultiplierUIController>("Environment");
            zenject.Expose<SongProgressUIController>("Environment");
            zenject.Expose<ImmediateRankUIPanel>("Environment");

            // Multiplayer support broke with Sira3... maybe someday I'll look into it.
            zenject.Install<AppConfigInstaller>(Location.App, config.Generated<PluginConfig>());
            zenject.Install<MenuUIInstaller>(Location.Menu);
            zenject.Install<PanelModifierInstaller>(Location.Singleplayer);
        }
    }
}
