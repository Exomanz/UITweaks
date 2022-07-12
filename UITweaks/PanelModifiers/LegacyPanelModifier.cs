using HMUI;
using IPA.Utilities;
using System.Collections;
using TMPro;
using UITweaks.Models;
using UnityEngine;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class LegacyPanelModifier : PanelModifier
    {
        internal class AprilFools : MonoBehaviour
        {
            [Inject] private readonly CoreGameHUDController gameHUDController;
            [InjectOptional] private readonly StandardGameplaySceneSetupData data;

            public void Start()
            {
                if (data.beatmapCharacteristic.containsRotationEvents)
                {
                    var container = gameHUDController.transform.Find("Container");
                    container.transform.Rotate(0, 0, 180);
                    return;
                }
                
                var leftPanel = gameHUDController.transform.Find("LeftPanel");
                leftPanel.position = new Vector3(3.2f, 2.2f, 7);
                leftPanel.Rotate(0, 0, 180);

                var rightPanel = gameHUDController.transform.Find("RightPanel");
                rightPanel.position = new Vector3(-3.2f, 2.2f, 7);
                rightPanel.Rotate(0, 0, 180);

                var energyPanel = gameHUDController.transform.Find("EnergyPanel/EnergyBarWrapper");
                energyPanel.Rotate(0, 0, 180);
            }
        }

        private ComboUIController ComboController = null!;
        private ImmediateRankUIPanel RankPanel = null!;
        private Config.MiscConfig Config = null!;

        [Inject] internal void ModifierInit(ComboUIController cuic, ImmediateRankUIPanel iruip, Config.MiscConfig c)
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
