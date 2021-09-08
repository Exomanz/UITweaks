using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using Zenject;

namespace UITweaks.UI
{
    [ViewDefinition("UITweaks.Views.preview.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\preview.bsml")]
    public class ObjectPreviewViewController : BSMLAutomaticViewController
    {
#pragma warning disable CS0649
        [Inject] MainConfig config;

        [UIValue("enable-previews")] protected bool EnablePreviews
        {
            get => config.AllowPreviews;
            set => config.AllowPreviews = value;
        }
    }
#pragma warning restore CS0649
}
