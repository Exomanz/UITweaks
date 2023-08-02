using HMUI;
using IPA.Utilities;
using System.Collections;
using System.Collections.Generic;
using UITweaks.Config;
using UITweaks.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class EnergyBarPanelModifier : PanelModifier
    {
        [Inject] private readonly IGameEnergyCounter gameEnergyCounter;
        [Inject] private readonly GameEnergyUIPanel gameEnergyUIPanel;
        [Inject] private readonly GameplayModifiers gameplayModifiers;
        [Inject] private readonly EnergyConfig energyConfig;

        private ImageView energyBar = null!;

        [Inject] protected override void Init()
        {
            logger.Debug("EnergyBarPanelModifier::Init()");
            base.parentPanel = gameEnergyUIPanel.gameObject;
            base.config = energyConfig;

            this.transform.SetParent(gameEnergyUIPanel.transform);
            this.ModPanel();
        }

        protected override void ModPanel()
        {
            base.ModPanel();
            StartCoroutine(PrepareColorsForEnergyType(gameplayModifiers.energyType));
        }

        private IEnumerator PrepareColorsForEnergyType(GameplayModifiers.EnergyType type)
        {
            yield return new WaitUntil(() => gameEnergyUIPanel != null);

            if (type == GameplayModifiers.EnergyType.Battery)
            {
                List<Image> batterySegments = gameEnergyUIPanel.GetField<List<Image>, GameEnergyUIPanel>("_batteryLifeSegments");

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
            else if (energy > 0.5f) energyBar.color = HSBColor.Lerp(
                HSBColor.FromColor(energyConfig.Mid),
                HSBColor.FromColor(energyConfig.High),
                (energy - 0.5f) * 2).ToColor();
            else if (energy < 0.5f) energyBar.color = HSBColor.Lerp(
                HSBColor.FromColor(energyConfig.Low),
                HSBColor.FromColor(energyConfig.Mid),
                energy * 2).ToColor();
        }

        public void Update()
        {
            if (!energyBar) return;

            if (energyConfig.RainbowOnFullEnergy)
            {
                if (gameplayModifiers.instaFail || (gameEnergyCounter.energy == 1 && gameplayModifiers.energyType == GameplayModifiers.EnergyType.Bar))
                    energyBar.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1), 1, 1));
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            gameEnergyCounter.gameEnergyDidChangeEvent -= HandleEnergyDidChange;
        }
    }
}
