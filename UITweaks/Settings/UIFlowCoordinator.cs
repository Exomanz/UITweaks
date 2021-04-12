using HMUI;
using BeatSaberMarkupLanguage;
using Zenject;

namespace UITweaks.Settings
{
    public class UIFlowCoordinator : FlowCoordinator
    {
        MainFlowCoordinator _mainFlow;
        UISettingsController _settings;
        ExtraSettingsController _extra;

        [Inject]
        public void Construct(MainFlowCoordinator mainFlow, UISettingsController settings,
            ExtraSettingsController extra)
        {
            _mainFlow = mainFlow;
            _settings = settings;
            _extra = extra;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation)
            {
                SetTitle("UI Tweaks", ViewController.AnimationType.In);
                showBackButton = true;
                ProvideInitialViewControllers(_settings, null, null, _extra);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            base.BackButtonWasPressed(topViewController);
            _mainFlow.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Horizontal, false);
        }
    }
}
