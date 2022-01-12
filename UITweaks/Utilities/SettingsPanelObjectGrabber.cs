using IPA.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UITweaks.Utilities
{
    internal class SettingsPanelObjectGrabber : MonoBehaviour
    {
        public bool isCompleted { get; private set; } = false;

        public GameObject MultiplierPanel { get; private set; } = null!;
        public GameObject EnergyPanel { get; private set; } = null!;
        public GameObject ComboPanel { get;private set; } = null!;
        public GameObject ProgressPanel { get; private set; } = null!;
        public GameObject ImmediateRankPanel { get; private set; } = null!;

        private List<MonoBehaviour> Controllers = null!;

        public void Start()
        {
            isCompleted = false;
            StartCoroutine(GetPanels());
        }

        private IEnumerator GetPanels()
        {
            bool sceneIsLoaded = false;
            try
            {
                AsyncOperation loadScene = SceneManager.LoadSceneAsync("DefaultEnvironment", LoadSceneMode.Additive);
                while (!loadScene.isDone) yield return null;

                sceneIsLoaded = true;
                yield return new WaitForSecondsRealtime(0.1f); // Allow objects to fully load

                // I would use Object.FindObjectsOfType<>() but it returns null since the objects aren't active.
                Controllers = new List<MonoBehaviour>()
                {
                    Resources.FindObjectsOfTypeAll<ScoreMultiplierUIController>().FirstOrDefault(),
                    Resources.FindObjectsOfTypeAll<GameEnergyUIPanel>().FirstOrDefault(),
                    Resources.FindObjectsOfTypeAll<ComboUIController>().FirstOrDefault(),
                    Resources.FindObjectsOfTypeAll<SongProgressUIController>().FirstOrDefault(),
                    Resources.FindObjectsOfTypeAll<ImmediateRankUIPanel>().FirstOrDefault(),
                };

                MultiplierPanel = FinalizePanel(Controllers[0]);
                EnergyPanel = FinalizePanel(Controllers[1]);
                ComboPanel = FinalizePanel(Controllers[2]);
                ProgressPanel = FinalizePanel(Controllers[3]);
                ImmediateRankPanel = FinalizePanel(Controllers[4]);

                if (MultiplierPanel && EnergyPanel && ComboPanel && ProgressPanel && ImmediateRankPanel) isCompleted = true;

                MultiplierPanel.name = "Preview_MultiplierPanel";
                EnergyPanel.name = "Preview_EnergyPanel";
                ComboPanel.name = "Preview_ComboPanel";
                ProgressPanel.name = "Preview_ProgressPanel";
                ImmediateRankPanel.name = "Preview_ScoreRankPanel";

                loadScene = null;
            }
            finally
            {
                if (sceneIsLoaded)
                    SceneManager.UnloadSceneAsync("DefaultEnvironment");
            }

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
