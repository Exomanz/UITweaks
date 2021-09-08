using HMUI;
using SiraUtil.Tools;
using UITweaks.Configuration;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UITweaks.Colorers
{
    public class MultiplierColorer : MonoBehaviour
    {
#pragma warning disable CS0169, CS0649
        [Inject] ScoreMultiplierUIController scorePanel;
        [Inject] MultiplierConfig config;
        [Inject] SiraLog log;
#pragma warning restore CS0169, CS0649

        private CurvedTextMeshPro[] texts;
        private Image bg;
        private Image fg;

        public void Start()
        {
            log.Logger.Debug("MultiplierColorer:Start()");
            transform.SetParent(scorePanel.transform);

            texts = scorePanel.transform.Find("TextPanel").GetComponentsInChildren<CurvedTextMeshPro>();
            bg = scorePanel.transform.Find("BGCircle").GetComponent<Image>();
            fg = scorePanel.transform.Find("FGCircle").GetComponent<Image>();
        }

        public void Update()
        {
            if (scorePanel.isActiveAndEnabled)
            {
                switch (texts[1].text)
                {
                    case "1":
                        bg.color = config.One.ColorWithAlpha(0.25f);
                        fg.color = config.One;
                        break;
                    case "2":
                        bg.color = config.Two.ColorWithAlpha(0.25f);
                        fg.color = config.Two;
                        break;
                    case "4":
                        bg.color = config.Four.ColorWithAlpha(0.25f);
                        fg.color = config.Four;
                        break;
                    case "8":
                        if (config.Rainbow8x)
                            bg.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(
                                Time.time * .5f, 1), 1, 1));

                        else bg.color = config.Eight.ColorWithAlpha(0.25f);
                        break;
                }
            }
        }
    }
}
