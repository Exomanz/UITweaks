using HMUI;
using BeatSaberMarkupLanguage;
using Zenject;

namespace UITweaks.Settings
{
    public class UIFlowCoordinator : FlowCoordinator
    {
        MainFlowCoordinator _mainFlow;
        MultiplierSettingsController _multi;
        EnergyBarSettingsController _energy;
        ComboPanelSettingsController _combo;
        ExtraSettingsController _extra;

        [Inject]
        public void Construct(MainFlowCoordinator mainFlow, MultiplierSettingsController multi,
            EnergyBarSettingsController energy, ComboPanelSettingsController combo, 
            ExtraSettingsController extra)
        {
            _mainFlow = mainFlow;
            _multi = multi;
            _energy = energy;
            _combo = combo;
            _extra = extra;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation)
            {
                SetTitle("UI Tweaks", ViewController.AnimationType.In);
                showBackButton = true;
                ProvideInitialViewControllers(_multi, _energy, _combo, _extra);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            base.BackButtonWasPressed(topViewController);
            _mainFlow.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Horizontal, false);
        }
    }
}
