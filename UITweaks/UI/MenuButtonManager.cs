using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using HMUI;
using System;
using Zenject;

namespace UITweaks.UI
{
    internal class MenuButtonManager : IInitializable, IDisposable
    {
        private MainFlowCoordinator MainFlowCoordinator = null!;
        private ModFlowCoordinator ModFlowCoordinator = null!;
        private MenuButton Button;

        public MenuButtonManager(MainFlowCoordinator mfc, ModFlowCoordinator mofc)
        {
            MainFlowCoordinator = mfc;
            ModFlowCoordinator = mofc;
            Button = new("UI Tweaks", SummonFlowCoordinator);
        }

        public void Initialize()
        {
            MenuButtons.instance.RegisterButton(Button);
        }

        private void SummonFlowCoordinator()
        {
            MainFlowCoordinator.PresentFlowCoordinator(ModFlowCoordinator, null, ViewController.AnimationDirection.Vertical);
        }

        public void Dispose()
        {
            if (BSMLParser.IsSingletonAvailable && MenuButtons.IsSingletonAvailable)
            {
                MenuButtons.instance.UnregisterButton(Button);
            }
        }
    }
}
