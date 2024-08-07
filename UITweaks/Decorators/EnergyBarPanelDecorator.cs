﻿using HMUI;
using System.Collections;
using System.Collections.Generic;
using UITweaks.Config;
using UITweaks.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class EnergyBarPanelDecorator : PanelDecoratorBase
    {
        [InjectOptional] internal readonly MultiplayerController mpController;
        [Inject] private readonly IGameEnergyCounter gameEnergyCounter;
        [Inject] private readonly GameplayModifiers gameplayModifiers;
        [Inject] private readonly EnergyConfig energyConfig;

        private GameEnergyUIPanel gameEnergyUIPanel;
        private ImageView energyBar;

        [Inject]
        protected override void Init()
        {
            gameEnergyUIPanel = base.gameHUDController.GetComponentInChildren<GameEnergyUIPanel>();
            ParentPanel = gameEnergyUIPanel?.gameObject;
            Config = energyConfig;
            transform.SetParent(ParentPanel?.transform);

            ModPanel(this);
        }

        protected override bool ModPanel(in PanelDecoratorBase decorator)
        {
            if (!base.ModPanel(this)) return false;

            PrepareColorsForEnergyType(gameplayModifiers.energyType);

            return true;
        }

        private void PrepareColorsForEnergyType(GameplayModifiers.EnergyType type)
        {
            if (!CanBeUsedSafely) return;

            if (type == GameplayModifiers.EnergyType.Bar && !gameplayModifiers.instaFail)
            {
                energyBar = gameEnergyUIPanel.transform.Find("EnergyBarWrapper/EnergyBar").GetComponent<ImageView>();
                energyBar.color = energyConfig.Mid;
                gameEnergyCounter.gameEnergyDidChangeEvent += HandleEnergyDidChange;

                return;
            }

            if (mpController?.gameObject == null)
                base.StartCoroutine(BatteryEnergyAndOneLifeSetup(type));
        }

        internal IEnumerator BatteryEnergyAndOneLifeSetup(GameplayModifiers.EnergyType energyType)
        {
            yield return new WaitUntil(() => gameEnergyUIPanel.isActiveAndEnabled);

            if (energyType == GameplayModifiers.EnergyType.Battery)
            {
                List<Image> batterySegments = gameEnergyUIPanel._batteryLifeSegments;

                batterySegments[0].color = energyConfig.Low;
                batterySegments[1].color = HSBColor.Lerp(HSBColor.FromColor(energyConfig.Low), HSBColor.FromColor(energyConfig.Mid), 0.5f).ToColor();
                batterySegments[2].color = HSBColor.Lerp(HSBColor.FromColor(energyConfig.Mid), HSBColor.FromColor(energyConfig.High), 0.5f).ToColor();
                batterySegments[3].color = energyConfig.High;

                yield break;
            }

            else if (energyType == GameplayModifiers.EnergyType.Bar && gameplayModifiers.instaFail)
            {
                energyBar = gameEnergyUIPanel.transform.Find("BatteryLifeSegment(Clone)").GetComponent<ImageView>();
                energyBar.color = energyConfig.High;
            }

            else yield break;
        }

        private void HandleEnergyDidChange(float energy)
        {
            if (energy == 0.5f) energyBar.color = energyConfig.Mid;

            else if (energy > 0.5f)
                energyBar.color = HSBColor.Lerp(
                    HSBColor.FromColor(energyConfig.Mid),
                    HSBColor.FromColor(energyConfig.High),
                    (energy - 0.5f) * 2).ToColor();

            else if (energy < 0.5f)
                energyBar.color = HSBColor.Lerp(
                    HSBColor.FromColor(energyConfig.Low),
                    HSBColor.FromColor(energyConfig.Mid),
                    energy * 2).ToColor();
        }

        public void Update()
        {
            if (!CanBeUsedSafely || !gameEnergyUIPanel.isActiveAndEnabled || energyBar?.gameObject == null) return;

            if (energyConfig.RainbowOnFullEnergy)
            {
                if (gameplayModifiers.instaFail || (gameEnergyCounter.energy == 1 && gameplayModifiers.energyType == GameplayModifiers.EnergyType.Bar))
                    energyBar.color = base.RainbowColor;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            gameEnergyCounter.gameEnergyDidChangeEvent -= HandleEnergyDidChange;
        }
    }
}
