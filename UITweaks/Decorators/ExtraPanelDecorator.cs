using HMUI;
using TMPro;
using UITweaks.Config;
using UITweaks.Models;
using UnityEngine;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class ExtraPanelDecorator : PanelDecoratorBase
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
        [Inject] private readonly IScoreController scoreController;
        [Inject] private readonly RelativeScoreAndImmediateRankCounter rankCounter;

        private ComboUIController comboUIController;
        private ImmediateRankUIPanel immediateRankUIPanel;
        private TextMeshProUGUI rankText;

        [Inject] protected override void Init()
        {
            comboUIController = base.gameHUDController.GetComponentInChildren<ComboUIController>();
            immediateRankUIPanel = base.gameHUDController.GetComponentInChildren<ImmediateRankUIPanel>();
            parentPanel = immediateRankUIPanel.gameObject;
            config = miscConfig;
            transform.SetParent(parentPanel?.transform);

            ModPanel(this);
        }

        protected override bool ModPanel(in PanelDecoratorBase decorator)
        {
            if (!base.ModPanel(this)) return false;

            rankText = immediateRankUIPanel._rankText;
            if (miscConfig.AllowRankColoring)
            {
                rankCounter.relativeScoreOrImmediateRankDidChangeEvent += UpdateRankColorsOnScoreChanged;
                //scoreController.scoringForNoteFinishedEvent += UpdateRankColorsOnScoreChanged;
                UpdateRankColorsOnScoreChanged();
            }

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
                rankText.fontStyle = FontStyles.Italic;
                rankText.transform.localPosition = new Vector3(-3, -0.5f);

                immediateRankUIPanel._relativeScoreText.fontStyle = FontStyles.Italic;
            }

            return true;
        }

        public void Update()
        {
            if (!immediateRankUIPanel.isActiveAndEnabled || !miscConfig.AllowRankColoring || !CanBeUsedSafely) return;

            if (miscConfig.RainbowOnSSRank && rankCounter.immediateRank == RankModel.Rank.SS)
            {
                rankText.color = base.RainbowColor;
            }
        }

        private void UpdateRankColorsOnScoreChanged()
        {
            rankText.color = rankCounter.immediateRank switch
            {
                RankModel.Rank.SSS or RankModel.Rank.SS => miscConfig.RankSSColor,
                RankModel.Rank.S => miscConfig.RankSColor,
                RankModel.Rank.A => miscConfig.RankAColor,
                RankModel.Rank.B => miscConfig.RankBColor,
                RankModel.Rank.C => miscConfig.RankCColor,
                RankModel.Rank.D => miscConfig.RankDColor,
                RankModel.Rank.E => miscConfig.RankEColor,
                _ => Color.white.ColorWithAlpha(0.5f),
            };
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            rankCounter.relativeScoreOrImmediateRankDidChangeEvent -= UpdateRankColorsOnScoreChanged;
            //scoreController.scoringForNoteFinishedEvent -= UpdateRankColorsOnScoreChanged;
        }
    }
}
