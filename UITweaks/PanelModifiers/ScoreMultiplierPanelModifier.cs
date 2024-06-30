using UITweaks.Config;
using UITweaks.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class ScoreMultiplierPanelModifier : PanelModifierBase
    {
        [Inject] private readonly MultiplierConfig multiplierConfig;
        [Inject] private IScoreController scoreController;

        private ScoreMultiplierUIController scoreMultiplierUIController;
        private int currentMultiplier = 0;
        private Image bg = null!;
        private Image fg = null!;

        [Inject] protected override void Init()
        {
            logger.Debug("ScoreMultiplierPanelModifier::Init()");
            this.scoreMultiplierUIController = base.gameHUDController.GetComponentInChildren<ScoreMultiplierUIController>();

            base.parentPanel = scoreMultiplierUIController.gameObject;
            base.config = multiplierConfig;

            this.transform.SetParent(parentPanel.transform);
            this.ModPanel();
        }

        protected override void ModPanel()
        {
            base.ModPanel();

            scoreController.multiplierDidChangeEvent += HandleMultiplierDidChange;

            bg = parentPanel.transform.Find("BGCircle").GetComponent<Image>();
            fg = parentPanel.transform.Find("FGCircle").GetComponent<Image>();
            this.HandleMultiplierDidChange(1, 0);
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
            if (!scoreMultiplierUIController.isActiveAndEnabled) return;

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
                bg.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1), 1, 1));
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            scoreController.multiplierDidChangeEvent -= HandleMultiplierDidChange;
            scoreController = null!;
            bg = null!;
            fg = null!;            
        }
    }
}
