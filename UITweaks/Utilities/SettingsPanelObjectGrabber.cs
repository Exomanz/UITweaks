using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using SiraUtil.Logging;

namespace UITweaks.Utilities
{
    internal class SettingsPanelObjectGrabber : MonoBehaviour
    {
        public bool IsCompleted { get; private set; } = false;
        public GameObject MultiplierPanel { get; private set; } 
        public GameObject EnergyPanel { get; private set; }
        public GameObject ComboPanel { get; private set; }
        public GameObject ProgressPanel { get; private set; }
        public GameObject ImmediateRankPanel { get; private set; }
        
        [Inject] private readonly SiraLog logger;
        private readonly List<MonoBehaviour> panelControllers = new List<MonoBehaviour>();

        public IEnumerator GetPanels()
        {
            try
            {
                AsyncOperationHandle<SceneInstance> defaultSceneInstanceLoader = Addressables.LoadSceneAsync("DefaultEnvironment", LoadSceneMode.Additive);

                while (!defaultSceneInstanceLoader.IsDone)
                    yield return new WaitForSecondsRealtime(0.1f);

                // I would use Object.FindObjectsOfType<>() but it returns null since the objects aren't active.
                panelControllers.Add(Resources.FindObjectsOfTypeAll<ScoreMultiplierUIController>().FirstOrDefault());
                panelControllers.Add(Resources.FindObjectsOfTypeAll<GameEnergyUIPanel>().FirstOrDefault());
                panelControllers.Add(Resources.FindObjectsOfTypeAll<ComboUIController>().FirstOrDefault());
                panelControllers.Add(Resources.FindObjectsOfTypeAll<SongProgressUIController>().FirstOrDefault());
                panelControllers.Add(Resources.FindObjectsOfTypeAll<ImmediateRankUIPanel>().FirstOrDefault());

                MultiplierPanel = FinalizePanel(panelControllers[0]);
                EnergyPanel = FinalizePanel(panelControllers[1]);
                ComboPanel = FinalizePanel(panelControllers[2]);
                ProgressPanel = FinalizePanel(panelControllers[3]);
                ImmediateRankPanel = FinalizePanel(panelControllers[4]);

                MultiplierPanel.name = "Preview_MultiplierPanel";
                EnergyPanel.name = "Preview_EnergyPanel";
                ComboPanel.name = "Preview_ComboPanel";
                ProgressPanel.name = "Preview_ProgressPanel";
                ImmediateRankPanel.name = "Preview_ScoreRankPanel";

                if (MultiplierPanel && EnergyPanel && ComboPanel && ProgressPanel && ImmediateRankPanel)
                    IsCompleted = true;
                else
                    throw new System.NullReferenceException("One or more PanelModifiers is null. Unable to load the Object Previewer.");

                SceneManager.UnloadSceneAsync("DefaultEnvironment");
            }
            finally { }

            yield break;
        }

        private GameObject FinalizePanel(MonoBehaviour controller)
        {
            logger.Debug("Finalizing Preview Panel : " + controller.name);
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
