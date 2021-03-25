using UITweaks.Settings;
using SiraUtil;
using Zenject;

namespace UITweaks.Installers
{
    public class UIMenuInstaller : Installer<UIMenuInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<UIFlowCoordinator>().FromNewComponentOnNewGameObject(nameof(UIFlowCoordinator)).AsSingle();

            Container.Bind<MultiplierSettingsController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<EnergyBarSettingsController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<ComboPanelSettingsController>().FromNewComponentAsViewController().AsSingle();

            Container.BindInterfacesAndSelfTo<UIMenuManager>().AsSingle();
        }
    }
}
