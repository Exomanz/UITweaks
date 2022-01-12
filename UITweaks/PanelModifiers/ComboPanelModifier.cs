using HMUI;
using UITweaks.Models;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class ComboPanelModifier : PanelModifier
    {
        private ComboUIController ComboController = null!;
        private Config.Combo Config = null!;
        private ImageView[] FCLines = null!;

        [Inject] public void ModifierInit(ComboUIController cuic, Config.Combo c)
        {
            Logger.Logger.Debug("ComboModifier:ModifierInit()");
            ComboController = cuic;
            Config = c;

            transform.SetParent(cuic.transform);
            ModPanel();
        }

        protected override void ModPanel()
        { 
            if (ComboController.isActiveAndEnabled)
            {
                FCLines = ComboController.GetComponentsInChildren<ImageView>();

                if (Config.UseGradient)
                {
                    FCLines[0].gradient = true;
                    FCLines[1].gradient = true;

                    FCLines[0].color0 = Config.TopLeft;
                    FCLines[0].color1 = Config.TopRight;

                    if (Config.MirrorBottomLine)
                    {
                        FCLines[1].color0 = Config.TopRight;
                        FCLines[1].color1 = Config.TopLeft;
                    }
                    else
                    {
                        FCLines[1].color0 = Config.BottomLeft;
                        FCLines[1].color1 = Config.BottomRight;
                    }
                }
                else
                {
                    FCLines[0].color = Config.TopLine;
                    FCLines[1].color = Config.BottomLine;
                }
            }
        }

        protected override void OnDestroy()
        {
            ComboController = null!;
            Config = null!;
            FCLines = null!;
        }
    }
}
