using UITweaks.Config;
using UITweaks.Models;
using UnityEngine.UI;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class ScoreMultiplierPanelDecorator : PanelDecoratorBase
    {
        [Inject] private readonly MultiplierConfig multiplierConfig;
        [Inject] private readonly IScoreController scoreController;

        private ScoreMultiplierUIController scoreMultiplierUIController;
        private Image bg;
        private Image fg;
        private int currentMultiplier = 0;

        [Inject]
        protected override void Init()
        {
            scoreMultiplierUIController = base.gameHUDController.GetComponentInChildren<ScoreMultiplierUIController>();
            ParentPanel = scoreMultiplierUIController?.gameObject;
            Config = multiplierConfig;

            transform.SetParent(ParentPanel?.transform);

            ModPanel(this);
        }

        protected override bool ModPanel(in PanelDecoratorBase decorator)
        {
            if (!base.ModPanel(this)) return false;

            scoreController.multiplierDidChangeEvent += HandleMultiplierDidChange;

            bg = ParentPanel.transform.Find("BGCircle").GetComponent<Image>();
            fg = ParentPanel.transform.Find("FGCircle").GetComponent<Image>();
            HandleMultiplierDidChange(1, 0);

            return true;
        }

        private void HandleMultiplierDidChange(int multiplier, float progress)
        {
            currentMultiplier = multiplier;

            if (multiplier == 1)
            {
                bg.color = multiplierConfig.One.ColorWithAlpha(0.25f);
                fg.color = multiplierConfig.One;
            }
            else if (multiplier == 2)
            {
                bg.color = multiplierConfig.Two.ColorWithAlpha(0.25f);
                fg.color = multiplierConfig.Two;
            }
            else if (multiplier == 4)
            {
                bg.color = multiplierConfig.Four.ColorWithAlpha(0.25f);
                fg.color = multiplierConfig.Four;
            }
            else if (multiplier == 8 && !multiplierConfig.RainbowOnMaxMultiplier)
                fg.color = multiplierConfig.Eight.ColorWithAlpha(0.25f);
        }

        public void Update()
        {
            if (!CanBeUsedSafely || !scoreMultiplierUIController.isActiveAndEnabled) return;

            if (multiplierConfig.SmoothTransition)
            {
                if (currentMultiplier == 1)
                {
                    bg.color = HSBColor.Lerp(
                        HSBColor.FromColor(multiplierConfig.One),
                        HSBColor.FromColor(multiplierConfig.Two),
                        fg.fillAmount)
                        .ToColor().ColorWithAlpha(0.25f);

                    fg.color = HSBColor.Lerp(
                        HSBColor.FromColor(multiplierConfig.One),
                        HSBColor.FromColor(multiplierConfig.Two),
                        fg.fillAmount)
                        .ToColor();
                }
                else if (currentMultiplier == 2)
                {
                    bg.color = HSBColor.Lerp(
                        HSBColor.FromColor(multiplierConfig.Two),
                        HSBColor.FromColor(multiplierConfig.Four),
                        fg.fillAmount)
                        .ToColor().ColorWithAlpha(0.25f);

                    fg.color = HSBColor.Lerp(
                        HSBColor.FromColor(multiplierConfig.Two),
                        HSBColor.FromColor(multiplierConfig.Four),
                        fg.fillAmount)
                        .ToColor();
                }
                else if (currentMultiplier == 4)
                {
                    bg.color = HSBColor.Lerp(
                        HSBColor.FromColor(multiplierConfig.Four),
                        HSBColor.FromColor(multiplierConfig.Eight),
                        fg.fillAmount)
                        .ToColor().ColorWithAlpha(0.25f);

                    fg.color = HSBColor.Lerp(
                        HSBColor.FromColor(multiplierConfig.Four),
                        HSBColor.FromColor(multiplierConfig.Eight),
                        fg.fillAmount)
                        .ToColor();
                }
            }

            if (currentMultiplier == 8 && multiplierConfig.RainbowOnMaxMultiplier)
            {
                bg.color = base.RainbowColor;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            scoreController.multiplierDidChangeEvent -= HandleMultiplierDidChange;
        }
    }
}
