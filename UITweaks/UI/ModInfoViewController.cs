using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using IPA.Loader;
using SiraUtil.Logging;
using SiraUtil.Zenject;
using UnityEngine;
using Zenject;

namespace UITweaks.UI
{
    [ViewDefinition("UITweaks.Views.Info.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\Info.bsml")]
    public class ModInfoViewController : BSMLAutomaticViewController
    {
        [Inject] private readonly SiraLog logger;
        private PluginMetadata meta;

        [Inject] internal void Construct(UBinder<Plugin, PluginMetadata> metadata)
        {
            meta = metadata.Value;
        }

        [UIValue("version-text")] private string Version
        {
            get => $"Version  :  <color=#00BBFF>{meta.HVersion}</color>";
        }

        [UIAction("open-gh-source")] 
        internal void OpenSourceLink() => Application.OpenURL("https://github.com/Exomanz/UITweaks");

        [UIAction("open-kofi")] 
        internal void OpenDonateLink() => Application.OpenURL("https://ko-fi.com/exo_manz");

        [UIAction("open-changelog")]
        internal void OpenChangelogLink() => Application.OpenURL("https://github.com/Exomanz/UITweaks/commits/");
    }
}
