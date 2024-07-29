using HMUI;
using UITweaks.Config;
using UITweaks.Models;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class ComboPanelDecorator : PanelDecoratorBase
    {
        [Inject] private readonly ComboConfig comboConfig;

        private ComboUIController comboUIController;

        [Inject] protected override void Init()
        {
            comboUIController = base.gameHUDController.GetComponentInChildren<ComboUIController>();
            ParentPanel = comboUIController.gameObject;
            Config = comboConfig;
            transform.SetParent(ParentPanel?.transform);

            ModPanel(this);
        }

        protected override bool ModPanel(in PanelDecoratorBase decorator)
        {
            if (!base.ModPanel(this)) return false;

            ImageView[] fcLines;
            fcLines = ParentPanel.GetComponentsInChildren<ImageView>();

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

            return true;
        }

        protected override void OnDestroy() => base.OnDestroy();
    }
}
