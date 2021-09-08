using SiraUtil;
using UITweaks.UI;
using UnityEngine;
using Zenject;

namespace UITweaks.Installers
{
    public class MenuManagerInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<UIFlowCoordinator>().FromNewComponentOnNewGameObject("UITweaksFlowCoordinator").AsSingle();
            Container.Bind<ModSettingsViewController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<ObjectPreviewViewController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<ExtraSettingsViewController>().FromNewComponentAsViewController().AsSingle();

            Container.BindInterfacesAndSelfTo<UIMenuButtonManager>().AsSingle();
        }
    }
}