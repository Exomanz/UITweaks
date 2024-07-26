using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using IPA.Utilities;

namespace UITweaks.UI
{
    internal class SettingsPanelObjectGrabber : MonoBehaviour
    {
        public bool IsCompleted { get; private set; } = false;
        public GameObject MultiplierPanel { get; private set; }
        public GameObject EnergyPanel { get; private set; }
        public GameObject ComboPanel { get; private set; }
        public GameObject ProgressPanel { get; private set; }
        public GameObject PositionPanel { get; private set; }
        public GameObject ImmediateRankPanel { get; private set; }

        public IEnumerator GetPanels()
        {
            AsyncOperationHandle<SceneInstance> defaultSceneInstanceLoader = Addressables.LoadSceneAsync("DefaultEnvironment", LoadSceneMode.Additive);

            while (!defaultSceneInstanceLoader.IsDone)
                yield return new WaitForSecondsRealtime(0.1f);

            CoreGameHUDController gameHudController = Resources.FindObjectsOfTypeAll<CoreGameHUDController>().FirstOrDefault();

            ScoreMultiplierUIController multiplierController = gameHudController.transform.Find("RightPanel/MultiplierCanvas").GetComponent<ScoreMultiplierUIController>();
            MultiplierPanel = FinalizePanel(multiplierController);

            GameEnergyUIPanel energyPanel = gameHudController.transform.Find("EnergyPanel").GetComponent<GameEnergyUIPanel>();
            EnergyPanel = FinalizePanel(energyPanel);

            ComboUIController comboController = gameHudController.transform.Find("LeftPanel/ComboPanel").GetComponent<ComboUIController>();
            ComboPanel = FinalizePanel(comboController);

            SongProgressUIController progressController = gameHudController.transform.Find("RightPanel/SongProgressCanvas").GetComponent<SongProgressUIController>();
            ProgressPanel = FinalizePanel(progressController);

            ImmediateRankUIPanel rankController = gameHudController.transform.Find("LeftPanel/ScoreCanvas").GetComponent<ImmediateRankUIPanel>();
            Destroy(rankController.transform.Find("ScoreText").GetComponent<ScoreUIController>());
            ImmediateRankPanel = FinalizePanel(rankController);

            SceneManager.UnloadSceneAsync("DefaultEnvironment");

            MockMultiplayerPositionPanel positionPanel = new GameObject("UIT_Preview-MockMultipliayerPositionPanel").AddComponent<MockMultiplayerPositionPanel>();
            while (!positionPanel.IsSetup)
                yield return new WaitForSecondsRealtime(0.1f);
            positionPanel.transform.SetParent(transform, false);
            positionPanel.transform.localPosition = Vector3.zero;
            positionPanel.transform.localRotation = Quaternion.identity;
            PositionPanel = positionPanel.gameObject;
            
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
