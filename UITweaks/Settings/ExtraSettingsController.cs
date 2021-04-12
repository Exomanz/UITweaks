using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using Zenject;

namespace UITweaks.Settings
{
    [ViewDefinition("UITweaks.Settings.Views.extraSettings.bsml")]
    //[HotReload(RelativePathToLayout = @"..\Settings\Views\extraSettings.bsml")]
    public class ExtraSettingsController : BSMLAutomaticViewController
    {
        [Inject]
        public void Construct() { }
    }
}
