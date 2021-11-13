using UITweaks.Colorers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UITweaks.Installers
{
    public class PanelModifierInstaller : Installer
    {
        public override void InstallBindings()
        {
            bool isMultiplayer = SceneManager.GetSceneByName("MultiplayerGameplay").isLoaded;
            MainConfig main = Plugin.MainConfig;

            BindPanelModifier<MultiplierColorer>(main.MultiConfig.Enabled);
            BindPanelModifier<EnergyBarColorer>(main.EnergyConfig.Enabled);
            BindPanelModifier<ComboColorer>(main.ComboConfig.Enabled);
            BindPanelModifier<ProgressColorer>(main.ProgressConfig.Enabled);

            if (isMultiplayer)
                BindPanelModifier<PositionColorer>(main.PositionConfig.Enabled);

            BindPanelModifier<ImmediateRankPanelModifier>(main.MiscConfig.RestoreRankPanelItalics);
        }

        public void BindPanelModifier<T>(bool enabled) where T : MonoBehaviour
        {
            if (!enabled) return;
            Container.Bind<T>().FromNewComponentOn(new GameObject("Colorer")).AsSingle().NonLazy();
        }
    }
}
