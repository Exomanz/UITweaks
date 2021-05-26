using System.Collections;
using System.Linq;
using UITweaks.Settings;
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
        //public static GameObject PositionPanel = null;

        private void Awake()
        {
            isCompleted = false;
            StartCoroutine(GrabThosePanels());
        }

        private IEnumerator GrabThosePanels()
        {
            bool sceneLoaded = false;
            try
            {
                string sceneName = "DefaultEnvironment";
                AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                while (!loadScene.isDone) yield return null;

                /*sceneName = "MultiplayerGameplay";
                loadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                while (!loadScene.isDone) yield return null;*/

                sceneLoaded = true;
                yield return new WaitForSecondsRealtime(0.1f);

                #region Multiplier
                ScoreMultiplierUIController score = Resources.FindObjectsOfTypeAll<ScoreMultiplierUIController>().FirstOrDefault();
                
                MultiplierPanel = Instantiate(score).gameObject;
                DestroyImmediate(MultiplierPanel.GetComponent<ScoreMultiplierUIController>());
                MultiplierPanel.gameObject.name = "MultiPanelPreview";
                MultiplierPanel.transform.SetParent(transform);
                MultiplierPanel.transform.localPosition = Vector3.zero;
                MultiplierPanel.transform.localRotation = Quaternion.identity;
                #endregion
                #region Energy Bar
                GameEnergyUIPanel energy = Resources.FindObjectsOfTypeAll<GameEnergyUIPanel>().FirstOrDefault();

                EnergyBar = Instantiate(energy).gameObject;
                DestroyImmediate(EnergyBar.GetComponent<GameEnergyUIPanel>());
                EnergyBar.gameObject.name = "EnergyBarPreview";
                EnergyBar.transform.SetParent(transform);
                EnergyBar.transform.localPosition = Vector3.zero;
                EnergyBar.transform.localRotation = Quaternion.identity;
                #endregion
                #region Combo Panel
                ComboUIController combo = Resources.FindObjectsOfTypeAll<ComboUIController>().FirstOrDefault();

                ComboPanel = Instantiate(combo).gameObject;
                DestroyImmediate(ComboPanel.GetComponent<ComboUIController>());
                ComboPanel.gameObject.name = "ComboPanelPreview";
                ComboPanel.transform.SetParent(transform);
                ComboPanel.transform.localPosition = Vector3.zero;
                ComboPanel.transform.localRotation = Quaternion.identity;
                #endregion
                #region Progress Panel
                SongProgressUIController progress = Resources.FindObjectsOfTypeAll<SongProgressUIController>().FirstOrDefault();

                ProgressBar = Instantiate(progress).gameObject;
                DestroyImmediate(ProgressBar.GetComponent<SongProgressUIController>());
                ProgressBar.gameObject.name = "ProgressBarPreview";
                ProgressBar.transform.SetParent(transform);
                ProgressBar.transform.localPosition = Vector3.zero;
                ProgressBar.transform.localRotation = Quaternion.identity;
                #endregion
                /*#region Position Panel
                MultiplayerPositionHUDController pos = Resources.FindObjectsOfTypeAll<MultiplayerPositionHUDController>().FirstOrDefault();

                PositionPanel = Instantiate(pos).gameObject;
                DestroyImmediate(PositionPanel.GetComponent<MultiplayerPositionHUDController>());
                PositionPanel.gameObject.name = "PositionPanelPreview";
                PositionPanel.transform.SetParent(transform);
                PositionPanel.transform.localPosition = Vector3.zero;
                PositionPanel.transform.localRotation = Quaternion.identity;
                #endregion*/

                if (MultiplierPanel && EnergyBar && ComboPanel && ProgressBar/* && PositionPanel*/) isCompleted = true;
                loadScene = null;
            }
            finally
            {
                if (sceneLoaded)
                {
                    string sceneName = "DefaultEnvironment";
                    SceneManager.UnloadSceneAsync(sceneName);/*

                    sceneName = "MultiplayerGameplay";
                    SceneManager.UnloadSceneAsync(sceneName);*/
                }
            }
            yield break;
        }
    }
}
