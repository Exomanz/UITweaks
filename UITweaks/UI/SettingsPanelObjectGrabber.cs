using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UITweaks.Models;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace UITweaks.UI
{
    internal class SettingsPanelObjectGrabber : MonoBehaviour
    {
        public bool IsCompleted { get; private set; } = false;
        public List<PreviewPanel> PreviewPanels { get; private set; } = new List<PreviewPanel>();

        private readonly WaitForEndOfFrame DELAY = new WaitForEndOfFrame();
        private PreviewPanel multiplierPanel;
        private PreviewPanel energyPanel;
        private PreviewPanel comboPanel;
        private PreviewPanel progressPanel;
        private PreviewPanel positionPanel;
        private PreviewPanel immediateRankPanel;

        public IEnumerator GetPanels()
        {
            AsyncOperationHandle<SceneInstance> defaultSceneInstanceLoader = Addressables.LoadSceneAsync("DefaultEnvironment", LoadSceneMode.Additive);

            while (!defaultSceneInstanceLoader.IsDone)
                yield return DELAY;

            CoreGameHUDController gameHudController = Resources.FindObjectsOfTypeAll<CoreGameHUDController>().FirstOrDefault();
            GameObject currentPanel;

            ScoreMultiplierUIController scoreMultiplierUIController = gameHudController.transform.Find("RightPanel/MultiplierCanvas").GetComponent<ScoreMultiplierUIController>();
            currentPanel = FinalizePanel(scoreMultiplierUIController);
            multiplierPanel = new PreviewPanel(0, currentPanel);
            PreviewPanels.Add(multiplierPanel);

            GameEnergyUIPanel gameEnergyUIPanel = gameHudController.transform.Find("EnergyPanel").GetComponent<GameEnergyUIPanel>();
            currentPanel = FinalizePanel(gameEnergyUIPanel);
            energyPanel = new PreviewPanel(1, currentPanel);
            PreviewPanels.Add(energyPanel);

            ComboUIController comboController = gameHudController.transform.Find("LeftPanel/ComboPanel").GetComponent<ComboUIController>();
            currentPanel = FinalizePanel(comboController);
            comboPanel = new PreviewPanel(2, currentPanel);
            PreviewPanels.Add(comboPanel);

            SongProgressUIController progressController = gameHudController.transform.Find("RightPanel/SongProgressCanvas").GetComponent<SongProgressUIController>();
            currentPanel = FinalizePanel(progressController);
            progressPanel = new PreviewPanel(3, currentPanel);
            PreviewPanels.Add(progressPanel);

            MockMultiplayerPositionPanel mockMultiplayerPositionPanel = new GameObject("UIT_Preview-MockMultipliayerPositionPanel").AddComponent<MockMultiplayerPositionPanel>();
            while (!mockMultiplayerPositionPanel.IsSetup)
                yield return DELAY;
            mockMultiplayerPositionPanel.transform.SetParent(transform, false);
            mockMultiplayerPositionPanel.transform.localPosition = Vector3.zero;
            mockMultiplayerPositionPanel.transform.localRotation = Quaternion.identity;
            currentPanel = mockMultiplayerPositionPanel.gameObject;
            positionPanel = new PreviewPanel(4, currentPanel);
            PreviewPanels.Add(positionPanel);

            ImmediateRankUIPanel rankController = gameHudController.transform.Find("LeftPanel/ScoreCanvas").GetComponent<ImmediateRankUIPanel>();
            Destroy(rankController.transform.Find("ScoreText").GetComponent<ScoreUIController>()); // Fixes score randomization in menu not working on first open
            currentPanel = FinalizePanel(rankController);
            immediateRankPanel = new PreviewPanel(5, currentPanel);
            PreviewPanels.Add(immediateRankPanel);

            SceneManager.UnloadSceneAsync("DefaultEnvironment");

            IsCompleted = true;

            yield break;
        }

        private GameObject FinalizePanel(MonoBehaviour controller)
        {
            try
            {
                GameObject go = Instantiate(controller.gameObject);
                Destroy(go.GetComponent(controller.GetType()));
                go.transform.SetParent(transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.name = "UIT_Preview-" + controller.GetType().Name;
                return go;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
                return null;
            }
        }
    }
}
