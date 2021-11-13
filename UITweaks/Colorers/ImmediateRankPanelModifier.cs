using SiraUtil.Tools;
using TMPro;
using UITweaks.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UITweaks.Colorers
{
    public class ImmediateRankPanelModifier : MonoBehaviour
    {
        [Inject] private ImmediateRankUIPanel rankPanel;
        [Inject] private SiraLog log;
        private bool isMultiplayer = false;

        public void Start()
        {
            isMultiplayer = SceneManager.GetSceneByName("MultiplayerGameplay").isLoaded;

            log.Logger.Debug("ImmediateRankPanelModifier:Start()");
            transform.SetParent(rankPanel.transform);
            name = "Modifier";

            ItalicizeUI();
        }

        private void ItalicizeUI()
        {
            TextMeshProUGUI[] allTexts = rankPanel.transform.GetComponentsInChildren<TextMeshProUGUI>();

            foreach (TextMeshProUGUI tmp in allTexts)
                tmp.fontStyle = FontStyles.Italic;

            if (isMultiplayer) return; // This won't look right if we do it...
            allTexts[1].transform.localPosition = new Vector3(-3.2f, -.5f, -0.01f);
            rankPanel.transform.localPosition = new Vector3(-0.05f, 0.4f, 0);
        }
    }
}
