using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using IPA.Loader;
using SiraUtil.Logging;
using SiraUtil.Zenject;
using UnityEngine;
using Zenject;

namespace UITweaks.UI
{
    /// <summary>
    /// This class hosts the info panel for the mod, including the Source, Donate, and Changelog buttons.
    /// </summary>
    [ViewDefinition("UITweaks.Views.Info.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\Info.bsml")]
    public class ModInfoViewController : BSMLAutomaticViewController
    {
        private SiraLog Logger = null!;
        private PluginMetadata PluginMetadata = null!;

        [Inject] internal void Construct(SiraLog l, UBinder<Plugin, PluginMetadata> metadata)
        {
            l.Logger.Debug("ModInfoViewController:Construct()");

            Logger = l;
            PluginMetadata = metadata.Value;
        }

        [UIValue("version-text")] private string Version
        {
            get => $"Version  :  <color=#00BBFF>{PluginMetadata.HVersion}</color>";
        }

        [UIAction("open-gh-source")] internal void OpenGitHubSource()
        {
            Application.OpenURL("https://github.com/Exomanz/UITweaks");
        }

        [UIAction("open-kofi")] internal void OpenDonateLink()
        {
            Application.OpenURL("https://ko-fi.com/exo_manz");
        }
    }
}
