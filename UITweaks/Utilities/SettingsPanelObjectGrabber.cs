using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UITweaks.Utilities
{
    /// <summary>
    /// Utility for getting in-game HUD elements for a preview display.
    /// </summary>
    internal class SettingsPanelObjectGrabber : MonoBehaviour
    {
        public bool isCompleted { get; private set; } = false;
        public bool panelMade { get; private set; } = false;

        public GameObject MultiplierPanel = null;
        public GameObject EnergyPanel = null;
        public GameObject ComboPanel = null;
        public GameObject ProgressPanel = null;
        public Dictionary<string, MonoBehaviour> controllers = null;

        internal void Awake()
        {
            isCompleted = false;
            StartCoroutine(GrabThosePanels());
        }

        private IEnumerator GrabThosePanels()
        {
            bool sceneIsCurrentlyLoaded = false;
            try
            {
                string sceneName = "DefaultEnvironment";
                AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                while (!loadScene.isDone) yield return null;

                sceneIsCurrentlyLoaded = true;
                yield return new WaitForSecondsRealtime(0.1f);

                controllers = new Dictionary<string, MonoBehaviour>()
                {
                    { "Multiplier", Resources.FindObjectsOfTypeAll<ScoreMultiplierUIController>().FirstOrDefault() },
                    { "Energy", Resources.FindObjectsOfTypeAll<GameEnergyUIPanel>().FirstOrDefault() },
                    { "Combo", Resources.FindObjectsOfTypeAll<ComboUIController>().FirstOrDefault() },
                    { "Progress", Resources.FindObjectsOfTypeAll<SongProgressUIController>().FirstOrDefault() },
                };

                MultiplierPanel = SetupPanel(MultiplierPanel, controllers["Multiplier"]);
                yield return new WaitUntil(() => panelMade = true);
                EnergyPanel = SetupPanel(EnergyPanel, controllers["Energy"]);
                yield return new WaitUntil(() => panelMade = true);
                ComboPanel = SetupPanel(ComboPanel, controllers["Combo"]);
                yield return new WaitUntil(() => panelMade = true);
                ProgressPanel = SetupPanel(ProgressPanel, controllers["Progress"]);
                yield return new WaitUntil(() => panelMade = true);

                if (MultiplierPanel && EnergyPanel && ComboPanel && ProgressPanel) isCompleted = true;
                loadScene = null;
            }
            finally
            {
                if (sceneIsCurrentlyLoaded)
                {
                    string sceneName = "DefaultEnvironment";
                    SceneManager.UnloadSceneAsync(sceneName);
                }
            }
            yield break;
        }

        private GameObject SetupPanel(GameObject go, MonoBehaviour controller)
        {
            panelMade = false;
            try
            {
                go = Instantiate(controller.gameObject);
                DestroyImmediate(go.GetComponent(controller.GetType()));
                go.SetActive(false);
                go.transform.SetParent(transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                panelMade = true;
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
