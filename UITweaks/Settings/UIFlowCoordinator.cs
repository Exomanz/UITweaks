using HMUI;
using BeatSaberMarkupLanguage;
using Zenject;

namespace UITweaks.Settings
{
    public class UIFlowCoordinator : FlowCoordinator
    {
        MultiplierSettingsController _multi;
        EnergyBarSettingsController _energy;
        ComboPanelSettingsController _combo;
        MainFlowCoordinator _mainFlow;

        [Inject]
        public void Construct(MultiplierSettingsController multi, EnergyBarSettingsController energy, 
            MainFlowCoordinator mainFlow, ComboPanelSettingsController combo)
        {
            _multi = multi;
            _energy = energy;
            _mainFlow = mainFlow;
            _combo = combo;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation)
            {
                SetTitle("UI Tweaks", ViewController.AnimationType.In);
                showBackButton = true;
                ProvideInitialViewControllers(_multi, _energy, _combo);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            base.BackButtonWasPressed(topViewController);
            _mainFlow.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Horizontal, false);
        }
    }
}
