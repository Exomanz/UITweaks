using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using UnityEngine;

namespace UITweaks.UI
{
#pragma warning disable CS0649, CS0169
    [ViewDefinition("UITweaks.Views.extra.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\extra.bsml")]
    public class ExtraSettingsViewController : BSMLAutomaticViewController
    {
        [UIAction("open-gh")]
        protected void OpenGitHubPage() =>
            Application.OpenURL("https://github.com/Exomanz/UITweaks#readme");

        [UIAction("open-issues")]
        protected void OpenIssuesPage() =>
            Application.OpenURL("https://github.com/Exomanz/UITweaks/issues");
    }
#pragma warning restore CS0649, CS0169
}
