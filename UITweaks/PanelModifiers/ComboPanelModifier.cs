using HMUI;
using UITweaks.Config;
using UITweaks.Models;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class ComboPanelModifier : PanelModifierBase
    {
        [Inject] private readonly ComboConfig comboConfig;

        private ComboUIController comboUIController;

        [Inject] protected override void Init()
        {
            base.Init();
            comboUIController = base.gameHUDController.GetComponentInChildren<ComboUIController>();

            base.parentPanel = comboUIController.gameObject;
            base.config = comboConfig;

            this.transform.SetParent(parentPanel.transform);
            this.ModPanel();
        }

        protected override void ModPanel()
        {
            base.ModPanel();
            ImageView[] fcLines;

            if (comboUIController.isActiveAndEnabled)
            {
                fcLines = comboUIController.GetComponentsInChildren<ImageView>();

                if (comboConfig.UseGradient)
                {
                    fcLines[0].gradient = true;
                    fcLines[1].gradient = true;

                    fcLines[0].color0 = comboConfig.TopLeft;
                    fcLines[0].color1 = comboConfig.TopRight;

                    if (comboConfig.MirrorBottomLine)
                    {
                        fcLines[1].color0 = comboConfig.TopRight;
                        fcLines[1].color1 = comboConfig.TopLeft;
                    }
                    else
                    {
                        fcLines[1].color0 = comboConfig.BottomLeft;
                        fcLines[1].color1 = comboConfig.BottomRight;
                    }
                }
                else
                {
                    fcLines[0].color = comboConfig.TopLine;
                    fcLines[1].color = comboConfig.BottomLine;
                }
            }
        }

        protected override void OnDestroy() => base.OnDestroy();
    }
}
