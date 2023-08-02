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
        [Inject] private readonly SiraLog logger;
        private PluginMetadata meta = null!;

        [Inject] internal void Construct(UBinder<Plugin, PluginMetadata> metadata)
        {
            logger.Debug("ModInfoViewController:Construct()");
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
    }
}
