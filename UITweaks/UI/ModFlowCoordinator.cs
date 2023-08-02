using BeatSaberMarkupLanguage;
using HMUI;
using Zenject;

namespace UITweaks.UI
{
    internal class ModFlowCoordinator : FlowCoordinator
    {
        [Inject] private MainFlowCoordinator mainFlowCoordinator;
        [Inject] private ModSettingsViewController settingsView;
        [Inject] private ModInfoViewController infoView;
        [Inject] private ObjectPreviewViewController previewView;

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation && addedToHierarchy)
            {
                SetTitle("UITweaks");
                ProvideInitialViewControllers(settingsView, infoView, previewView);
                showBackButton = true;
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            base.BackButtonWasPressed(topViewController);
            mainFlowCoordinator.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Vertical, false);
        }
    }
}
