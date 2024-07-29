using BeatSaberMarkupLanguage;
using HMUI;
using Zenject;

namespace UITweaks.UI
{
    internal class UITweaksFlowCoordinator : FlowCoordinator
    {
        [Inject] private readonly MainFlowCoordinator mainFlowCoordinator;
        [Inject] private readonly ModSettingsViewController settingsView;
        [Inject] private readonly ModInfoViewController infoView;
        [Inject] private readonly ObjectPreviewViewController previewView;

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation && addedToHierarchy)
            {
                base.SetTitle("UITweaks");
                base.ProvideInitialViewControllers(settingsView, infoView, previewView);
                base.showBackButton = true;
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            base.BackButtonWasPressed(topViewController);
            mainFlowCoordinator.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Vertical, false);
        }
    }
}
