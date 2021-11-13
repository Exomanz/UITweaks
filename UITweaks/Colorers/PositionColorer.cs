using SiraUtil.Tools;
using TMPro;
using UITweaks.Configuration;
using UnityEngine;
using Zenject;

namespace UITweaks.Colorers
{
    public class PositionColorer : MonoBehaviour
    {
        [Inject] private MultiplayerPositionHUDController positionPanel;
        [Inject] private PositionConfig config;
        [Inject] private SiraLog log;
        private TextMeshProUGUI[] positionTexts;
        private TextMeshProUGUI firstPlaceObject;

        public void Start()
        {
            log.Logger.Debug("PositionColorer:Start()");
            transform.SetParent(positionPanel.transform);

            positionTexts = positionPanel.transform.GetComponentsInChildren<TextMeshProUGUI>();
            firstPlaceObject = positionPanel.transform.Find("DynamicPanel/1stPosition").gameObject
                .GetComponent<TextMeshProUGUI>();

            if (config.HideFirstPlace) firstPlaceObject.gameObject.SetActive(false);
        }

        public void Update()
        {
            switch (positionTexts[1].text)
            {
                case "1":
                    positionTexts[1].color = config.First;
                    firstPlaceObject.color = config.First;
                    break;
                case "2":
                    positionTexts[1].color = config.Second;
                    break;
                case "3":
                    positionTexts[1].color = config.Third;
                    break;
                case "4":
                    positionTexts[1].color = config.Fourth;
                    break;
                case "5":
                    positionTexts[1].color = config.Fifth;
                    break;
            }

            positionTexts[0].color = positionTexts[1].color.ColorWithAlpha(0.25f);
        }
    }
}
