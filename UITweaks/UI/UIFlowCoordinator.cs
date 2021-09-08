using BeatSaberMarkupLanguage;
using HMUI;
using Zenject;

namespace UITweaks.UI
{
    public class UIFlowCoordinator : FlowCoordinator
    {
        MainFlowCoordinator mainFlow;
        ModSettingsViewController modView;
        ExtraSettingsViewController extraView;
        ObjectPreviewViewController previewView;
        
        [Inject] public void Construct(MainFlowCoordinator m, ModSettingsViewController s,
            ExtraSettingsViewController e, ObjectPreviewViewController o)
        {
            mainFlow = m;
            modView = s;
            extraView = e;
            previewView = o;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation)
            {
                SetTitle("UI Tweaks", ViewController.AnimationType.In);
                showBackButton = true;
                ProvideInitialViewControllers(modView, extraView, previewView);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            base.BackButtonWasPressed(topViewController);
            mainFlow.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Vertical, false);
        }
    }
}
