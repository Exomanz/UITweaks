using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using SiraUtil.Tools;
using System.Collections;
using System.Collections.Generic;
using UITweaks.Utilities;
using UnityEngine;
using Zenject;

namespace UITweaks.Settings
{ 
    [ViewDefinition("UITweaks.Settings.Views.objectPreviewPanel.bsml")]
    //[HotReload(RelativePathToLayout = @"..\Settings\Views\objectPreviewPanel.bsml")]
    public class ObjectPreviewPanelController : BSMLAutomaticViewController
    {
        public GameObject previewParent;
        public bool generatingPreview;
        public bool doneGeneratingPreview;
        MainSettingsController _ui;
        SiraLog _log;

        public GameObject multiPanel;
        public GameObject energyBar;
        public GameObject comboPanel;
        public GameObject progressBar;
        public GameObject positionPanel;

        [Inject]
        public void Construct(MainSettingsController ui, SiraLog log)
        {
            _ui = ui;
            _log = log;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);

            if (!previewParent)
            {
                previewParent = new GameObject("UITweaksPreviewParent");
                previewParent.transform.position = new Vector3(3.4f, 1.5f, 1.7f);
                previewParent.transform.Rotate(0f, 67f, 0f);
            }
            doneGeneratingPreview = false;
            StartCoroutine(GeneratePanels());
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
            ClearPanels();
        }

        void ClearPanels()
        {
            if (multiPanel || energyBar || comboPanel || progressBar || positionPanel)
            {
                DestroyImmediate(multiPanel);
                DestroyImmediate(energyBar);
                DestroyImmediate(comboPanel);
                DestroyImmediate(progressBar);
                DestroyImmediate(positionPanel);
            }

            multiPanel = null;
            energyBar = null;
            comboPanel = null;
            progressBar = null;
            positionPanel = null;
        }

        IEnumerator GeneratePanels()
        {
            if (!generatingPreview)
            {
                yield return new WaitUntil(() => PanelGrabber.isCompleted);
                try
                {
                    generatingPreview = true;
                    ClearPanels();

                    multiPanel = Instantiate(PanelGrabber.MultiplierPanel, previewParent.transform);
                    multiPanel.transform.localPosition = Vector3.zero;
                    multiPanel.transform.localRotation = Quaternion.identity;
                    multiPanel.transform.localScale = Vector3.one * 0.008f;
                    multiPanel.name = "multiPanel";
                    _ui.MultiplierPreviewObjectHelper();

                    energyBar = Instantiate(PanelGrabber.EnergyBar, previewParent.transform);
                    DestroyImmediate(energyBar.GetComponent<GameEnergyUIPanel>());
                    energyBar.transform.localPosition = Vector3.zero;
                    energyBar.transform.localRotation = Quaternion.identity;
                    energyBar.transform.localScale = Vector3.one * 0.02f;
                    energyBar.name = "energyBar";
                    _ui.EnergyBarPreviewObjectHelper();

                    comboPanel = Instantiate(PanelGrabber.ComboPanel, previewParent.transform);
                    comboPanel.transform.localPosition = Vector3.zero;
                    comboPanel.transform.localRotation = Quaternion.identity;
                    comboPanel.transform.localScale = Vector3.one * 0.008f;
                    comboPanel.name = "comboPanel";
                    _ui.ComboPanelPreviewObjectHelper();

                    progressBar = Instantiate(PanelGrabber.ProgressBar, previewParent.transform);
                    progressBar.transform.localPosition = Vector3.zero;
                    progressBar.transform.localRotation = Quaternion.identity;
                    progressBar.transform.localScale = Vector3.one * 0.02f;
                    progressBar.name = "progressBar";
                    _ui.ProgressBarPreviewObjectHelper();

                    positionPanel = Instantiate(PanelGrabber.PositionPanel, previewParent.transform);
                    positionPanel.transform.localPosition = Vector3.zero;
                    positionPanel.transform.localRotation = Quaternion.identity;
                    positionPanel.transform.localScale = Vector3.one * 0.015f;
                    positionPanel.name = "positionPanel";
                    _ui.PositionPanelPreviewObjectHelper();

                    yield break;
                }
                finally
                {
                    doneGeneratingPreview = true;
                    generatingPreview = false;
                }
            }
            yield break;
        }
    }
}
