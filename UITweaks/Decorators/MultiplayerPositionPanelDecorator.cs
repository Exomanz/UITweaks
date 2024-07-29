using HMUI;
using System.Collections.Generic;
using TMPro;
using UITweaks.Config;
using UITweaks.Models;
using UnityEngine;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class MultiplayerPositionPanelDecorator : PanelDecoratorBase
    {
        [Inject] private readonly PositionConfig positionConfig;
        [Inject] private readonly MultiplayerScoreProvider scoreProvider;

        private MultiplayerPositionHUDController positionHUDController;
        private Dictionary<int, Color> playerCountToConfigValuesMap;
        private MultiplayerScoreProvider.RankedPlayer localConnectedPlayer;
        private TextMeshProUGUI playerCountText;
        private TextMeshProUGUI dynamicPositionText;

        [Inject] protected override void Init() 
        {
            positionHUDController = base.gameHUDController.transform.parent.GetComponentInChildren<MultiplayerPositionHUDController>();
            ParentPanel = positionHUDController.gameObject;
            Config = positionConfig;
            transform.SetParent(ParentPanel?.transform);

            playerCountToConfigValuesMap = new Dictionary<int, Color>()
            {
                { 0, Color.white },
                { 1, positionConfig.First },
                { 2, positionConfig.Second },
                { 3, positionConfig.Third },
                { 4, positionConfig.Fourth },
                { 5, positionConfig.Fifth }
            };

            ModPanel(this);
        }

        protected override bool ModPanel(in PanelDecoratorBase decorator)
        {
            if (!base.ModPanel(this)) return false;

            playerCountText = positionHUDController._playerCountText;
            dynamicPositionText = positionHUDController._positionText;

            return true;
        }

        public void Start()
        {
            if (!CanBeUsedSafely) return;

            int localPlayerIdx = scoreProvider._rankedPlayers.FindIndex(player => player.isMe);
            localConnectedPlayer = scoreProvider.rankedPlayers[localPlayerIdx];

            dynamicPositionText.color = playerCountToConfigValuesMap[localPlayerIdx + 1];
            playerCountText.color = positionConfig.UseStaticColorForStaticPanel ? positionConfig.StaticPanelColor.ColorWithAlpha(0.25f) : playerCountToConfigValuesMap[scoreProvider.rankedPlayers.Count].ColorWithAlpha(0.25f);

            if (positionConfig.HideFirstPlaceAnimation)
                positionHUDController._firstPlayerAnimationGo.transform.Rotate(0, 180, 0); // Text can't render upside-down
        }

        public void Update()
        {
            if (!CanBeUsedSafely) return;

            int position = scoreProvider._rankedPlayers.IndexOf(localConnectedPlayer) + 1;
            if (positionHUDController._prevPosition != position)
            {
                dynamicPositionText.color = playerCountToConfigValuesMap[position];

                if (!positionConfig.UseStaticColorForStaticPanel)
                    playerCountText.color = playerCountToConfigValuesMap[position].ColorWithAlpha(0.25f);
            }

            if (position == 1 && positionConfig.RainbowOnFirstPlace)
            {
                dynamicPositionText.color = base.RainbowColor;
                if (positionHUDController._firstPlayerAnimationGo?.gameObject != null)
                    positionHUDController._firstPlayerAnimationGo.GetComponent<CurvedTextMeshPro>().color = base.RainbowColor;

                if (!positionConfig.UseStaticColorForStaticPanel)
                    playerCountText.color = base.RainbowColor.ColorWithAlpha(0.25f);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
