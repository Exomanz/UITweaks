using IPA.Utilities;
using SiraUtil.Tools;
using System.Collections;
using System.Collections.Generic;
using UITweaks.Configuration;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UITweaks.Colorers
{
    public class EnergyBarColorer : MonoBehaviour
    {
        [Inject] private IGameEnergyCounter energyCounter;
        [Inject] private GameEnergyUIPanel energyPanel;
        [Inject] private GameplayModifiers mods;
        [Inject] private EnergyConfig config;
        [Inject] private SiraLog log;
        private Image mainImage;

        public void Start()
        {
            log.Logger.Debug("EnergyBarColorer:Start()");
            transform.SetParent(energyPanel.transform);
            StartCoroutine(PrepareColorForEnergyType(mods.energyType));
        }

        public IEnumerator PrepareColorForEnergyType(GameplayModifiers.EnergyType type)
        {
            yield return new WaitUntil(() => energyPanel != null);

            if (type == GameplayModifiers.EnergyType.Battery)
            {
                List<Image> segments = energyPanel.GetField<List<Image>, GameEnergyUIPanel>("_batteryLifeSegments");
                segments[0].color = config.Low;
                segments[1].color = HSBColor.Lerp(HSBColor.FromColor(config.Low), HSBColor.FromColor(config.Mid), 0.34f).ToColor();
                segments[2].color = HSBColor.Lerp(HSBColor.FromColor(config.Mid), HSBColor.FromColor(config.High), 0.66f).ToColor();
                segments[3].color = config.High;
                yield break;
            }

            else if (type == GameplayModifiers.EnergyType.Bar)
            {
                if (mods.instaFail)
                {
                    mainImage = energyPanel.transform.Find("BatteryLifeSegment(Clone)").GetComponent<Image>();
                    mainImage.color = config.High;
                }
                else
                {
                    mainImage = energyPanel.transform.Find("EnergyBarWrapper/EnergyBar").GetComponent<Image>();
                    mainImage.color = config.Mid;
                }
            }

            energyCounter.gameEnergyDidChangeEvent += HandleEnergyDidChange;
        }

        private void HandleEnergyDidChange(float energy)
        {
            if (energy == 0.5f) mainImage.color = config.Mid;
            if (energy > 0.5f) mainImage.color = HSBColor.Lerp(
                HSBColor.FromColor(config.Mid), 
                HSBColor.FromColor(config.High),
                (energy - 0.5f) * 2).ToColor();
            if (energy < 0.5f) mainImage.color = HSBColor.Lerp(
                HSBColor.FromColor(config.Low), 
                HSBColor.FromColor(config.Mid), 
                energy * 2).ToColor();
        }

        public void Update()
        {
            if (mainImage is null) return;

            if (config.RainbowFull)
            {
                if (mods.instaFail || (energyCounter.energy == 1f && mods.energyType == GameplayModifiers.EnergyType.Bar))
                    mainImage.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1), 1, 1));
            }
        }
    }
}
