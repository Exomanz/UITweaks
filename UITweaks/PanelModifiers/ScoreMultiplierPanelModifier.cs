using UITweaks.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class ScoreMultiplierPanelModifier : PanelModifier
    {
        private ScoreMultiplierUIController MultiplierController = null!;
        private Config.MultiplierConfig Config = null!;
        private IScoreController ScoreController = null!;
        private int CurrentMultiplier = 0;
        private Image BG = null!;
        private Image FG = null!;

        [Inject] internal void ModifierInit(ScoreMultiplierUIController smuic, Config.MultiplierConfig config, IScoreController scoreController)
        {
            Logger.Logger.Debug("ScoreMultiplierPanelModifier:ModifierInit()");
            MultiplierController = smuic;
            Config = config;
            ScoreController = scoreController;
            scoreController.multiplierDidChangeEvent += HandleMultiplierDidChange;

            transform.SetParent(smuic.transform);
            ModPanel();
        }

        protected override void ModPanel()
        {
            BG = MultiplierController.transform.Find("BGCircle").GetComponent<Image>();
            FG = MultiplierController.transform.Find("FGCircle").GetComponent<Image>();
            HandleMultiplierDidChange(1, 0);
        }

        private void HandleMultiplierDidChange(int multiplier, float progress)
        {
            CurrentMultiplier = multiplier;

            if (multiplier == 1)
            {
                BG.color = Config.One.ColorWithAlpha(0.25f);
                FG.color = Config.One;
            }
            else if (multiplier == 2)
            {
                BG.color = Config.Two.ColorWithAlpha(0.25f);
                FG.color = Config.Two;
            }
            else if (multiplier == 4)
            {
                BG.color = Config.Four.ColorWithAlpha(0.25f);
                FG.color = Config.Four;
            }
            else if (multiplier == 8 && !Config.RainbowOnMaxMultiplier)
                FG.color = Config.Eight.ColorWithAlpha(0.25f); 
        }

        public void Update()
        {
            if (!MultiplierController.isActiveAndEnabled) return;

            if (Config.SmoothTransition)
            {
                if (CurrentMultiplier == 1)
                {
                    BG.color = HSBColor.Lerp(HSBColor.FromColor(Config.One), HSBColor.FromColor(Config.Two), FG.fillAmount).ToColor().ColorWithAlpha(0.25f);
                    FG.color = HSBColor.Lerp(HSBColor.FromColor(Config.One), HSBColor.FromColor(Config.Two), FG.fillAmount).ToColor();
                }
                else if (CurrentMultiplier == 2)
                {
                    BG.color = HSBColor.Lerp(HSBColor.FromColor(Config.Two), HSBColor.FromColor(Config.Four), FG.fillAmount).ToColor().ColorWithAlpha(0.25f);
                    FG.color = HSBColor.Lerp(HSBColor.FromColor(Config.Two), HSBColor.FromColor(Config.Four), FG.fillAmount).ToColor();
                }
                else if (CurrentMultiplier == 4)
                {
                    BG.color = HSBColor.Lerp(HSBColor.FromColor(Config.Four), HSBColor.FromColor(Config.Eight), FG.fillAmount).ToColor().ColorWithAlpha(0.25f);
                    FG.color = HSBColor.Lerp(HSBColor.FromColor(Config.Four), HSBColor.FromColor(Config.Eight), FG.fillAmount).ToColor();
                }
            }

            if (CurrentMultiplier == 8 && Config.RainbowOnMaxMultiplier)
            {
                BG.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1), 1, 1));
            }
        }

        protected override void OnDestroy()
        {
            ScoreController.multiplierDidChangeEvent -= HandleMultiplierDidChange;

            MultiplierController = null!;
            Config = null!;
            ScoreController = null!;
        }
    }
}
