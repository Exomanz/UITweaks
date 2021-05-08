using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UITweaks.Utilities
{
    /// <summary>
    /// Utility for getting the HUD elements and displaying them in the preview
    /// </summary>
    public class PanelGrabber : MonoBehaviour
    {
        public static bool isCompleted { get; private set; } = false;

        public static GameObject MultiplierPanel = null;
        public static GameObject EnergyBar = null;
        public static GameObject ComboPanel = null;
        public static GameObject ProgressBar = null;
        public static GameObject PositionPanel = null;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            if (!isCompleted) StartCoroutine(GrabThosePanels());
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private void OnActiveSceneChanged(Scene current, Scene next)
        {
            if (next.name == "PCInit")
            {
                if (MultiplierPanel != null) Destroy(MultiplierPanel);

                isCompleted = false;
            }
        }

        private IEnumerator GrabThosePanels()
        {
            bool sceneLoaded = false;
            try
            {
                string sceneName = "DefaultEnvironment";
                AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                while (!loadScene.isDone) yield return null;

                sceneName = "MultiplayerGameplay";
                loadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                while (!loadScene.isDone) yield return null;

                sceneLoaded = true;
                yield return new WaitForSecondsRealtime(0.1f);

                #region Multiplier
                ScoreMultiplierUIController score = Resources.FindObjectsOfTypeAll<ScoreMultiplierUIController>().FirstOrDefault();
                
                MultiplierPanel = Instantiate(score).gameObject;
                DestroyImmediate(MultiplierPanel.GetComponent<ScoreMultiplierUIController>());
                DontDestroyOnLoad(MultiplierPanel);
                MultiplierPanel.transform.SetParent(transform);
                MultiplierPanel.gameObject.name = "PreviewMultiplierPanel";
                MultiplierPanel.transform.localPosition = Vector3.zero;
                MultiplierPanel.transform.localRotation = Quaternion.identity;
                #endregion
                #region Energy Bar
                GameEnergyUIPanel energy = Resources.FindObjectsOfTypeAll<GameEnergyUIPanel>().FirstOrDefault();

                EnergyBar = Instantiate(energy).gameObject;
                DestroyImmediate(MultiplierPanel.GetComponent<GameEnergyUIPanel>());
                DontDestroyOnLoad(EnergyBar);
                EnergyBar.transform.SetParent(transform);
                EnergyBar.gameObject.name = "PreviewEnergyBar";
                EnergyBar.transform.localPosition = Vector3.zero;
                EnergyBar.transform.localRotation = Quaternion.identity;
                #endregion
                #region Combo Panel
                ComboUIController combo = Resources.FindObjectsOfTypeAll<ComboUIController>().FirstOrDefault();

                ComboPanel = Instantiate(combo).gameObject;
                DestroyImmediate(ComboPanel.GetComponent<ComboUIController>());
                DontDestroyOnLoad(ComboPanel);
                ComboPanel.transform.SetParent(transform);
                ComboPanel.gameObject.name = "PreviewComboPanel";
                ComboPanel.transform.localPosition = Vector3.zero;
                ComboPanel.transform.localRotation = Quaternion.identity;
                #endregion
                #region Progress Panel
                SongProgressUIController progress = Resources.FindObjectsOfTypeAll<SongProgressUIController>().FirstOrDefault();

                ProgressBar = Instantiate(progress).gameObject;
                DestroyImmediate(ProgressBar.GetComponent<SongProgressUIController>());
                DontDestroyOnLoad(ProgressBar);
                ProgressBar.transform.SetParent(transform);
                ProgressBar.gameObject.name = "PreviewProgressBar";
                ProgressBar.transform.localPosition = Vector3.zero;
                ProgressBar.transform.localRotation = Quaternion.identity;
                #endregion
                #region Position Panel
                MultiplayerPositionHUDController pos = Resources.FindObjectsOfTypeAll<MultiplayerPositionHUDController>().FirstOrDefault();

                PositionPanel = Instantiate(pos).gameObject;
                DestroyImmediate(PositionPanel.GetComponent<MultiplayerPositionHUDController>());
                DontDestroyOnLoad(PositionPanel);
                PositionPanel.transform.SetParent(transform);
                PositionPanel.gameObject.name = "PreviewPositionPanel";
                PositionPanel.transform.localPosition = Vector3.zero;
                PositionPanel.transform.localRotation = Quaternion.identity;
                #endregion

                if (MultiplierPanel && EnergyBar && ComboPanel && ProgressBar && PositionPanel)
                {
                    MultiplierPanel.SetActive(false);
                    EnergyBar.SetActive(false);
                    ComboPanel.SetActive(false);
                    ProgressBar.SetActive(false);
                    PositionPanel.SetActive(false);
                    
                    isCompleted = true;
                }

                loadScene = null;
            }
            finally
            {
                if (sceneLoaded)
                {
                    string sceneName = "DefaultEnvironment";
                    SceneManager.UnloadSceneAsync(sceneName);

                    sceneName = "MultiplayerGameplay";
                    SceneManager.UnloadSceneAsync(sceneName);
                }
            }
            yield break;
        }
    }
}
