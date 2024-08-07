﻿using UITweaks.UI;
using UnityEngine;
using Zenject;

namespace UITweaks.Installers
{
    public class TweaksMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<ModSettingsViewController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<ModInfoViewController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<ObjectPreviewViewController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<SettingsPanelObjectGrabber>().FromNewComponentOn(new GameObject("SettingsPanelObjectGrabber")).AsSingle();

            Container.Bind<UITweaksFlowCoordinator>().FromNewComponentOn(new GameObject("UITweaksFlowCoordinator")).AsSingle();
            Container.BindInterfacesAndSelfTo<MenuButtonManager>().AsSingle();
        }
    }
}
