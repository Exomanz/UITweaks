using HMUI;
using IPA.Utilities;
using System.Collections;
using TMPro;
using UITweaks.Config;
using UITweaks.Models;
using UnityEngine;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class LegacyPanelModifier : PanelModifierBase
    {
        internal class AprilFools : MonoBehaviour
        {
            [InjectOptional] private readonly StandardGameplaySceneSetupData data;
            [Inject] private readonly CoreGameHUDController gameHUDController;

            public void Start()
            {
                if (data?.beatmapKey.beatmapCharacteristic.containsRotationEvents == true)
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

        [Inject] private readonly MiscConfig miscConfig;

        private ComboUIController comboUIController;
        private ImmediateRankUIPanel immediateRankUIPanel;

        [Inject] protected override void Init()
        {
            logger.Debug("LegacyPanelModifier::Init()");
            this.comboUIController = base.gameHUDController.GetComponentInChildren<ComboUIController>();
            this.immediateRankUIPanel = base.gameHUDController.GetComponentInChildren<ImmediateRankUIPanel>();

            base.parentPanel = immediateRankUIPanel.gameObject;
            base.config = miscConfig;

            this.transform.SetParent(parentPanel.transform);
            this.ModPanel();
        }

        protected override void ModPanel()
        {
            base.ModPanel();

            if (miscConfig.ItalicizeComboPanel)
            {
                var comboText = comboUIController.transform.Find("ComboText").GetComponent<CurvedTextMeshPro>();
                comboText.fontStyle = FontStyles.Italic | FontStyles.UpperCase;

                var num = comboUIController._comboText;
                num.fontStyle = FontStyles.Italic;
                num.transform.localPosition = new Vector3(-2.5f, 4);
            }

            if (miscConfig.ItalicizeScore)
            {
                var scoreText = immediateRankUIPanel.transform.Find("ScoreText").GetComponent<CurvedTextMeshPro>();
                scoreText.fontStyle = FontStyles.Italic;
                scoreText.transform.localPosition = new Vector3(-1, 20);
            }

            if (miscConfig.ItalicizeImmediateRank)
            {
                var immediateRankText = immediateRankUIPanel._rankText;
                immediateRankText.fontStyle = FontStyles.Italic;
                immediateRankText.transform.localPosition = new Vector3(-3, -0.5f);

                var relativeScoreText = immediateRankUIPanel._relativeScoreText;
                relativeScoreText.fontStyle = FontStyles.Italic;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
