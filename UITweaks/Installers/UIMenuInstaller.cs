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

            Container.Bind<UISettingsController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<ExtraSettingsController>().FromNewComponentAsViewController().AsSingle();

            Container.BindInterfacesAndSelfTo<UIMenuManager>().AsSingle();
        }
    }
}
