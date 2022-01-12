using HMUI;
using IPA.Utilities;
using TMPro;
using UITweaks.Models;
using UnityEngine;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class LegacyPanelModifier : PanelModifier
    {
        private ComboUIController ComboController = null!;
        private ImmediateRankUIPanel RankPanel = null!;
        private Config.Miscellaneous Config = null!;

        [Inject] internal void ModifierInit(ComboUIController cuic, ImmediateRankUIPanel iruip, Config.Miscellaneous c)
        {
            Logger.Logger.Debug("LegacyPanelModifier:ModifierInit()");
            ComboController = cuic;
            RankPanel = iruip;
            Config = c;

            transform.SetParent(iruip.transform);
            ModPanel();
        }

        protected override void ModPanel()
        {
            if (Config.ItalicizeComboPanel)
            {
                var comboText = ComboController.transform.Find("ComboText").GetComponent<CurvedTextMeshPro>();
                comboText.fontStyle = FontStyles.Italic;
                comboText.text = "COMBO";

                var num = ComboController.GetField<TextMeshProUGUI, ComboUIController>("_comboText");
                num.fontStyle = FontStyles.Italic;
                num.transform.localPosition = new Vector3(-2.5f, 4);
            }

            if (Config.ItalicizeScore)
            {
                var scoreText = RankPanel.transform.Find("ScoreText").GetComponent<CurvedTextMeshPro>();
                scoreText.fontStyle = FontStyles.Italic;
                scoreText.transform.localPosition = new Vector3(-1, 20);
            }

            if (Config.ItalicizeImmediateRank)
            {
                var immediateRankText = RankPanel.GetField<TextMeshProUGUI, ImmediateRankUIPanel>("_rankText");
                immediateRankText.fontStyle = FontStyles.Italic;
                immediateRankText.transform.localPosition = new Vector3(-3, -0.5f);

                var relativeScoreText = RankPanel.GetField<TextMeshProUGUI, ImmediateRankUIPanel>("_relativeScoreText");
                relativeScoreText.fontStyle = FontStyles.Italic;
            }
        }

        protected override void OnDestroy()
        {
            ComboController = null!;
            RankPanel = null!;
            Config = null!;
        }
    }
}
