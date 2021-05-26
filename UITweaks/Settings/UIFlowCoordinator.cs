using HMUI;
using BeatSaberMarkupLanguage;
using UITweaks.Utilities;
using UnityEngine;
using Zenject;

namespace UITweaks.Settings
{
    public class UIFlowCoordinator : FlowCoordinator
    {
        MainFlowCoordinator _mainFlow;
        MainSettingsController _settings;
        ObjectPreviewPanelController _preview;
        ExtraSettingsController _extra;
        public static bool _firstActivation = false;

        [Inject]
        public void Construct(MainFlowCoordinator mainFlow, MainSettingsController settings, ObjectPreviewPanelController preview, ExtraSettingsController extra)
        {
            _mainFlow = mainFlow;
            _settings = settings;
            _preview = preview;
            _extra = extra;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation)
            {
                SetTitle("UI Tweaks", ViewController.AnimationType.Out);
                showBackButton = true;
                ProvideInitialViewControllers(_settings, null, _preview, _extra);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            base.BackButtonWasPressed(topViewController);
            _mainFlow.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Vertical, false);
        }
    }
}
