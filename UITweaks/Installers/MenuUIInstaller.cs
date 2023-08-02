using UITweaks.UI;
using UnityEngine;
using Zenject;

namespace UITweaks.Installers
{
    public class MenuUIInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<ModSettingsViewController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<ModInfoViewController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<ObjectPreviewViewController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<ModFlowCoordinator>().FromNewComponentOn(new GameObject("UITweaksFlowCoordinator")).AsSingle();
            Container.BindInterfacesAndSelfTo<MenuButtonManager>().AsSingle();
        }
    }
}
