using BeatSaberMarkupLanguage;
using HMUI;
using Zenject;

namespace UITweaks.UI
{
    internal class ModFlowCoordinator : FlowCoordinator
    {
        private MainFlowCoordinator MainFlowCoordinator = null!;
        private ModSettingsViewController ModSettings = null!;
        private ModInfoViewController ModInfo = null!;
        private ObjectPreviewViewController PreviewController = null!;

        [Inject] internal void Construct(MainFlowCoordinator mfc, ModSettingsViewController msvc, ModInfoViewController mivc, ObjectPreviewViewController opvc)
        {
            MainFlowCoordinator = mfc;
            ModSettings = msvc;
            ModInfo = mivc;
            PreviewController = opvc;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation)
            {
                SetTitle("UITweaks");
                showBackButton = true;
                ProvideInitialViewControllers(ModSettings, ModInfo, PreviewController);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            base.BackButtonWasPressed(topViewController);
            MainFlowCoordinator.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Vertical, false);
        }
    }
}
