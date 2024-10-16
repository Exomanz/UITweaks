using BeatSaberMarkupLanguage.MenuButtons;
using HMUI;
using System;
using Zenject;

namespace UITweaks.UI
{
    internal class MenuButtonManager : IInitializable, IDisposable
    {
        [Inject] private readonly MainFlowCoordinator mainFlowCoordinator;
        [Inject] private readonly UITweaksFlowCoordinator modFlowCoordinator;
        private MenuButton button;

        public void Initialize()
        {
            button = new MenuButton("UI Tweaks", "Spice up your HUD!", () =>
            {
                mainFlowCoordinator.PresentFlowCoordinator(modFlowCoordinator, null, ViewController.AnimationDirection.Vertical);
            });
            MenuButtons.Instance.RegisterButton(button);
        }

        public void Dispose()
        {
            if (button != null)
                MenuButtons.Instance.UnregisterButton(button);
        }
    }
}
