using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using SiraUtil.Tools;
using System.Collections;
using UITweaks.Utilities;
using UnityEngine;
using Zenject;

namespace UITweaks.Settings
{ 
    [ViewDefinition("UITweaks.Settings.Views.objectPreviewPanel.bsml")]
    //[HotReload(RelativePathToLayout = @"..\Settings\Views\objectPreviewPanel.bsml")]
    public class ObjectPreviewPanelController : BSMLAutomaticViewController
    {
        public GameObject previewController;
        MainSettingsController _ui;
        SiraLog _log;

        [Inject]
        public void Construct(MainSettingsController ui, SiraLog log)
        {
            _ui = ui;
            _log = log;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            StartCoroutine(StartCoroutinesOnPanelsGot());

            if (!previewController)
            {
                previewController = new GameObject("UITweaksPreviewController");
                DontDestroyOnLoad(previewController);
                previewController.transform.position = new Vector3(3.4f, 1.5f, 1.7f);
                previewController.transform.Rotate(0f, 67f, 0f);
                previewController.AddComponent<PanelGrabber>();
            }
            previewController.transform.position = new Vector3(3.4f, 1.5f, 1.7f);
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
            previewController.transform.position = new Vector3(3.4f, -500f, 1.7f);
        }

        internal IEnumerator StartCoroutinesOnPanelsGot()
        {
            yield return new WaitUntil(() => PanelGrabber.isCompleted);
            try
            {
                _ui.MultiplierPreviewObjectHelper();
                //_ui.PositionPanelPreviewObjectHelper();
            }
            catch (System.Exception ex) { _log.Logger.Error(ex); }

            yield break;
        }
    }
}
