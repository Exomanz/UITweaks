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

            Container.Bind<MainSettingsController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<ObjectPreviewPanelController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<ExtraSettingsController>().FromNewComponentAsViewController().AsSingle();

            Container.BindInterfacesAndSelfTo<UIMenuManager>().AsSingle();
        }
    }
}
