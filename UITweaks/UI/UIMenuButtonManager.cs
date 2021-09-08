using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using System;
using Zenject;

namespace UITweaks.UI
{
    public class UIMenuButtonManager : IInitializable, IDisposable
    {
        MainFlowCoordinator mainFlow;
        UIFlowCoordinator modFlow;
        MenuButton menuButton;

        public UIMenuButtonManager(MainFlowCoordinator m, UIFlowCoordinator u)
        {
            mainFlow = m;
            modFlow = u;
            menuButton = new MenuButton("UI Tweaks", SummonFlowCoordinator);
        }

        public void Initialize() =>
            MenuButtons.instance.RegisterButton(menuButton);

        private void SummonFlowCoordinator() => mainFlow.PresentFlowCoordinator(modFlow, null, HMUI.ViewController.AnimationDirection.Vertical);

        public void Dispose()
        {
            if (BSMLParser.IsSingletonAvailable && MenuButtons.IsSingletonAvailable)
                MenuButtons.instance.UnregisterButton(menuButton);
        }
    }
}
