using UITweaks.Colorers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UITweaks.Installers
{
    public class GameInstaller : Installer
    {
        public override void InstallBindings()
        {
            bool isMultiplayer = SceneManager.GetSceneByName("MultiplayerGameplay").isLoaded;
            MainConfig main = Plugin.MainConfig;

            if (main.MultiConfig.Enabled)
                Container.Bind<MultiplierColorer>().FromNewComponentOn(
                    new GameObject("Colorer")).AsSingle().NonLazy();

            if (main.EnergyConfig.Enabled)
                Container.Bind<EnergyBarColorer>().FromNewComponentOn(
                    new GameObject("Colorer")).AsSingle().NonLazy();

            if (main.ComboConfig.Enabled)
                Container.Bind<ComboColorer>().FromNewComponentOn(
                    new GameObject("Colorer")).AsSingle().NonLazy();

            if (main.ProgressConfig.Enabled)
                Container.Bind<ProgressColorer>().FromNewComponentOn(
                    new GameObject("Colorer")).AsSingle().NonLazy();

            if (main.PositionConfig.Enabled && isMultiplayer)
                Container.Bind<PositionColorer>().FromNewComponentOn(
                    new GameObject("Colorer")).AsSingle().NonLazy();
        }
    }
}
