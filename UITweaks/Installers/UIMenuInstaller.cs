using UITweaks.Settings;
using SiraUtil;
using Zenject;

namespace UITweaks.Installers
{
    public class UIMenuInstaller : Installer<UIMenuInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<UIFlowCoord>().FromNewComponentOnNewGameObject(nameof(UIFlowCoord)).AsSingle();

            Container.Bind<MainSettingsController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<ObjectPreviewView>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<ExtraSettingsController>().FromNewComponentAsViewController().AsSingle();

            Container.BindInterfacesAndSelfTo<UIMenuManager>().AsSingle();
        }
    }
}
