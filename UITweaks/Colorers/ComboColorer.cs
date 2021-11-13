using HMUI;
using IPA.Utilities;
using SiraUtil.Tools;
using UITweaks.Configuration;
using UnityEngine;
using Zenject;

namespace UITweaks.Colorers
{
    public class ComboColorer : MonoBehaviour
    {
        [Inject] private ComboUIController comboPanel;
        [Inject] private ComboConfig config;
        [Inject] private SiraLog log;
        private ImageView[] fcLines;

        public void Start()
        {
            log.Logger.Debug("ComboColorer:Start()");
            transform.SetParent(comboPanel.transform);

            Setup();
        }

        private void Setup()
        {
            if (comboPanel.isActiveAndEnabled)
            {
                fcLines = comboPanel.GetComponentsInChildren<ImageView>();

                if (config.UseGradient)
                {
                    fcLines[0].SetField("_gradient", true);
                    fcLines[1].SetField("_gradient", true);

                    fcLines[0].color0 = config.TopLeft;
                    fcLines[0].color1 = config.TopRight;

                    if (config.MirrorOnBottom)
                    {
                        fcLines[1].color0 = config.TopRight;
                        fcLines[1].color1 = config.TopLeft;
                    }

                    else
                    {
                        fcLines[1].color0 = config.BottomLeft;
                        fcLines[1].color1 = config.BottomRight;
                    }
                }

                else
                {
                    fcLines[0].color = config.TopLine;
                    fcLines[1].color = config.BottomLine;
                }
            }
        }
    }
}
