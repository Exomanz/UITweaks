using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using System;
using Zenject;

namespace UITweaks.Settings
{
    public class UIMenuManager : IInitializable, IDisposable
    {
        readonly MainFlowCoordinator _mainFlow;
        readonly UIFlowCoordinator _flowCoord;
        readonly MenuButton _menuButton;

        public UIMenuManager(MainFlowCoordinator mainFlow, UIFlowCoordinator flowCoord)
        {
            _mainFlow = mainFlow;
            _flowCoord = flowCoord;
            _menuButton = new MenuButton("UI Tweaks", SummonFlowCoordinator);
        }

        public void Initialize() =>
            MenuButtons.instance.RegisterButton(_menuButton);

        public void Dispose()
        {
            if (BSMLParser.IsSingletonAvailable && MenuButtons.IsSingletonAvailable)
                MenuButtons.instance.UnregisterButton(_menuButton);
        }

        void SummonFlowCoordinator() => _mainFlow.PresentFlowCoordinator(_flowCoord);
    }
}
