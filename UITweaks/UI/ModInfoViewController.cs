using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using SiraUtil.Logging;
using System;
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

        [Inject] internal void Construct(SiraLog l)
        {
            l.Logger.Debug("ModInfoViewController:Construct()");

            Logger = l;
        }

        [UIAction("open-gh-source")] internal void OpenGitHubSource()
        {
            Application.OpenURL("https://github.com/Exomanz/UITweaks/tree/sira3/UITweaks");
        }

        [UIAction("open-kofi")] internal void OpenDonateLink()
        {
            Application.OpenURL("https://ko-fi.com/exo_manz");
        }
    }
}
