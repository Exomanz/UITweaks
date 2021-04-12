using UITweaks.Services;
using Zenject;

namespace UITweaks.Installers
{
    public class UIGameInstaller : Installer<PluginConfig, UIGameInstaller>
    {
        readonly PluginConfig.ComboConfig _combo;
        readonly PluginConfig.EnergyBarConfig _energy;
        readonly PluginConfig.MultiplierConfig _multi;

        public UIGameInstaller(PluginConfig.ComboConfig combo, 
            PluginConfig.EnergyBarConfig energy, PluginConfig.MultiplierConfig multi)
        {
            _combo = combo;
            _energy = energy;
            _multi = multi;
        }

        public override void InstallBindings()
        {
            if (_multi.Enabled) 
                Container.BindInterfacesAndSelfTo<MultiplierColorer>().AsSingle();

            if (_energy.Enabled) 
                Container.BindInterfacesAndSelfTo<EnergyBarColorer>().AsSingle();

            if (_combo.Enabled) 
                Container.BindInterfacesAndSelfTo<ComboColorer>().AsSingle();
        }
    }
}
