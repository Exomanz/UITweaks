using IPA;
using IPA.Config.Stores;
using IPAConfig = IPA.Config.Config;
using IPALogger = IPA.Logging.Logger;
using SiraUtil.Zenject;
using System;
using UITweaks.Installers;

namespace UITweaks
{
    [Plugin(RuntimeOptions.DynamicInit), NoEnableDisable]
    public class Plugin
    {
        public static Plugin Instance { get; private set; }
        public static bool isAprilFools { get; private set; }

        [Init]
        public Plugin(IPALogger logger, IPAConfig config, Zenjector zenject)
        {
            Instance = this;

            DateTime time = DateTime.UtcNow;
            isAprilFools = time.Month == 4 && time.Day == 1;

            zenject.UseLogger(logger);
            zenject.UseMetadataBinder<Plugin>();
#if DEBUG
            zenject.Expose<CoreGameHUDController>("Environment");
#else
            if (isAprilFools) zenject.Expose<CoreGameHUDController>("Environment");
#endif
            zenject.Expose<GameEnergyUIPanel>("Environment");
            zenject.Expose<ComboUIController>("Environment");
            zenject.Expose<ScoreMultiplierUIController>("Environment");
            zenject.Expose<SongProgressUIController>("Environment");
            zenject.Expose<ImmediateRankUIPanel>("Environment");

            // Multiplayer support broke with Sira3... maybe someday I'll look into it.
            zenject.Install<AppConfigInstaller>(Location.App, config.Generated<PluginConfig>());
            zenject.Install<MenuUIInstaller>(Location.Menu);
            zenject.Install<PanelModifierInstaller>(Location.StandardPlayer | Location.CampaignPlayer);
        }
    }
}
