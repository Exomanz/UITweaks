using UITweaks.Services;
using UnityEngine.SceneManagement;
using Zenject;

namespace UITweaks.Installers
{
    public class UICoreInstaller : Installer<PluginConfig, UICoreInstaller>
    {
        readonly PluginConfig.ComboConfig _combo;
        readonly PluginConfig.EnergyBarConfig _energy;
        readonly PluginConfig.MultiplierConfig _multi;
        readonly PluginConfig.ProgressConfig _progress;
        readonly PluginConfig.PositionConfig _position;

        public UICoreInstaller(PluginConfig.ComboConfig combo, 
            PluginConfig.EnergyBarConfig energy, PluginConfig.MultiplierConfig multi, 
            PluginConfig.ProgressConfig progress, PluginConfig.PositionConfig position)
        {
            _combo = combo;
            _energy = energy;
            _multi = multi;
            _progress = progress;
            _position = position;
        }

        public override void InstallBindings()
        {
            bool isMultiplayer = false;
            isMultiplayer = SceneManager.GetSceneByName("MultiplayerGameplay").isLoaded;

            if (_multi.Enabled) 
                Container.BindInterfacesAndSelfTo<MultiplierColorer>().AsSingle();

            if (_energy.Enabled) 
                Container.BindInterfacesAndSelfTo<EnergyBarColorer>().AsSingle();

            if (_combo.Enabled) 
                Container.BindInterfacesAndSelfTo<ComboColorer>().AsSingle();

            if (_progress.Enabled)
                Container.BindInterfacesAndSelfTo<ProgressColorer>().AsSingle();

            if (_position.Enabled && isMultiplayer)
                Container.BindInterfacesAndSelfTo<PositionColorer>().AsSingle();
        }
    }
}
