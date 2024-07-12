using HMUI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UITweaks.Config;
using UITweaks.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class EnergyBarPanelDecorator : PanelDecoratorBase
    {
        [Inject] private readonly IGameEnergyCounter gameEnergyCounter;
        [Inject] private readonly GameplayModifiers gameplayModifiers;
        [Inject] private readonly EnergyConfig energyConfig;

        private GameEnergyUIPanel gameEnergyUIPanel;
        private ImageView energyBar;

        [Inject] protected override void Init()
        {
            gameEnergyUIPanel = base.gameHUDController.GetComponentInChildren<GameEnergyUIPanel>();
            parentPanel = gameEnergyUIPanel.gameObject;
            config = energyConfig;
            transform.SetParent(parentPanel?.transform);

            ModPanel(this);
        }

        protected override bool ModPanel(in PanelDecoratorBase decorator)
        {
            if (!base.ModPanel(this)) return false;

            StartCoroutine(PrepareColorsForEnergyType(gameplayModifiers.energyType));

            return true;
        }

        private IEnumerator PrepareColorsForEnergyType(GameplayModifiers.EnergyType type)
        {
            if (!CanBeUsedSafely) yield break;

            yield return new WaitUntil(() => gameEnergyUIPanel.gameObject != null);

            if (type == GameplayModifiers.EnergyType.Battery)
            {
                List<Image> batterySegments = gameEnergyUIPanel._batteryLifeSegments;

                batterySegments[0].color = energyConfig.Low;
                batterySegments[1].color = HSBColor.Lerp(HSBColor.FromColor(energyConfig.Low), HSBColor.FromColor(energyConfig.Mid), 0.34f).ToColor();
                batterySegments[2].color = HSBColor.Lerp(HSBColor.FromColor(energyConfig.Mid), HSBColor.FromColor(energyConfig.High), 0.66f).ToColor();
                batterySegments[3].color = energyConfig.High;

                yield break;
            }

            else if (type == GameplayModifiers.EnergyType.Bar)
            {
                if (gameplayModifiers.instaFail)
                {
                    energyBar = gameEnergyUIPanel.transform.Find("BatteryLifeSegment(Clone)").GetComponent<ImageView>();
                    energyBar.color = energyConfig.High;
                }
                else
                {
                    energyBar = gameEnergyUIPanel.transform.Find("EnergyBarWrapper/EnergyBar").GetComponent<ImageView>();
                    energyBar.color = energyConfig.Mid;
                }
            }

            gameEnergyCounter.gameEnergyDidChangeEvent += HandleEnergyDidChange;
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
            if (!gameEnergyUIPanel.isActiveAndEnabled || !CanBeUsedSafely) return;

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
