using SiraUtil.Tools;
using System.Collections.Generic;
using UITweaks.Configuration;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UITweaks.Colorers
{
    public class EnergyBarColorer : MonoBehaviour
    {
#pragma warning disable CS0649, CS0169
        [Inject] IGameEnergyCounter energyCounter;
        [Inject] GameEnergyUIPanel energyPanel;
        [Inject] GameplayModifiers mods;
        [Inject] EnergyConfig config;
        [Inject] SiraLog log;
#pragma warning restore CS0649, CS0169

        Image mainImage;
        List<Image> batterySegments = new List<Image>(3);

        public void Start()
        {
            log.Logger.Debug("EnergyBarColorer:Start()");
            transform.SetParent(energyPanel.transform);

            if (mods.energyType == GameplayModifiers.EnergyType.Bar)
                mainImage = energyPanel.transform.Find("EnergyBarWrapper/EnergyBar").GetComponent<Image>();

            if (mods.energyType == GameplayModifiers.EnergyType.Battery)
            {
                foreach (Image x in energyPanel.transform.GetComponentsInChildren<Image>())
                {
                    if (x.name.Contains("BatteryLifeSegment(Clone)"))
                        batterySegments.Add(x);
                }
            }

            if (mods.instaFail)
                mainImage = energyPanel.transform.Find("BatteryLifeSegment(Clone)").GetComponent<Image>();
        }

        public void Update()
        {
            if (mods.energyType == GameplayModifiers.EnergyType.Bar)
            {
                if (energyCounter.energy == 0.5f)
                    mainImage.color = config.Mid;
                if (energyCounter.energy < 0.5f)
                    mainImage.color = Color.Lerp(config.Low, config.Mid, energyCounter.energy * 2);
                if (energyCounter.energy > 0.5f)
                    mainImage.color = Color.Lerp(config.Mid, config.High, (energyCounter.energy - 0.5f) * 2);
                if (energyCounter.energy == 1 && config.RainbowFull)
                    mainImage.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1), 1, 1));
            }

            if (mods.energyType == GameplayModifiers.EnergyType.Battery)
            {
                batterySegments[0].color = config.Low;
                batterySegments[1].color = Color.Lerp(config.Low, config.Mid, 0.66f);
                batterySegments[2].color = Color.Lerp(config.Mid, config.High, 0.33f);
                batterySegments[3].color = config.High;
            }

            if (mods.instaFail)
            {
                if (config.RainbowFull)
                    mainImage.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1), 1, 1));
                else mainImage.color = config.High;
            }
        }
    }
}
