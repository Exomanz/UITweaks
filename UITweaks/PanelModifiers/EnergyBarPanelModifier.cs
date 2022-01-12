using HMUI;
using IPA.Utilities;
using System.Collections;
using System.Collections.Generic;
using UITweaks.Models;
using UnityEngine;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class EnergyBarPanelModifier : PanelModifier
    {
        private IGameEnergyCounter EnergyCounter = null!;
        private GameEnergyUIPanel EnergyPanel = null!;
        private GameplayModifiers Modifiers = null!;
        private Config.Energy Config = null!;
        private ImageView EnergyBar = null!;

        [Inject] public void ModifierInit(IGameEnergyCounter igec, GameEnergyUIPanel geuip, GameplayModifiers gm, Config.Energy c)
        {
            Logger.Logger.Debug("EnergyBarPanelModifier:Init()");
            EnergyCounter = igec;
            EnergyPanel = geuip;
            Modifiers = gm;
            Config = c;

            transform.SetParent(geuip.transform);
            ModPanel();
        }

        protected override void ModPanel()
        {
            StartCoroutine(PrepareColorsForEnergyType(Modifiers.energyType));
        }

        private IEnumerator PrepareColorsForEnergyType(GameplayModifiers.EnergyType type)
        {
            yield return new WaitUntil(() => EnergyPanel != null);

            if (type == GameplayModifiers.EnergyType.Battery)
            {
                List<ImageView> batterySegments = EnergyPanel.GetField<List<ImageView>, GameEnergyUIPanel>("_batteryLifeSegments");

                batterySegments[0].color = Config.Low;
                batterySegments[1].color = HSBColor.Lerp(HSBColor.FromColor(Config.Low), HSBColor.FromColor(Config.Mid), 0.34f).ToColor();
                batterySegments[2].color = HSBColor.Lerp(HSBColor.FromColor(Config.Mid), HSBColor.FromColor(Config.High), 0.66f).ToColor();
                batterySegments[3].color = Config.High;

                yield break;
            }

            else if (type == GameplayModifiers.EnergyType.Bar)
            {
                if (Modifiers.instaFail)
                {
                    EnergyBar = EnergyPanel.transform.Find("BatteryLifeSegment(Clone)").GetComponent<ImageView>();
                    EnergyBar.color = Config.High;
                }
                else
                {
                    EnergyBar = EnergyPanel.transform.Find("EnergyBarWrapper/EnergyBar").GetComponent<ImageView>();
                    EnergyBar.color = Config.Mid;
                }
            }

            EnergyCounter.gameEnergyDidChangeEvent += HandleEnergyDidChange;
        }

        private void HandleEnergyDidChange(float energy)
        {
            if (energy == 0.5f) EnergyBar.color = Config.Mid;
            else if (energy > 0.5f) EnergyBar.color = HSBColor.Lerp(
                HSBColor.FromColor(Config.Mid),
                HSBColor.FromColor(Config.High),
                (energy - 0.5f) * 2).ToColor();
            else if (energy < 0.5f) EnergyBar.color = HSBColor.Lerp(
                HSBColor.FromColor(Config.Low),
                HSBColor.FromColor(Config.Mid),
                energy * 2).ToColor();
        }

        public void Update()
        {
            if (!EnergyBar) return;

            if (Config.RainbowOnFullEnergy)
            {
                if (Modifiers.instaFail || (EnergyCounter.energy == 1 && Modifiers.energyType == GameplayModifiers.EnergyType.Bar))
                    EnergyBar.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1), 1, 1));
            }
        }

        protected override void OnDestroy()
        {
            EnergyCounter.gameEnergyDidChangeEvent -= HandleEnergyDidChange;

            EnergyCounter = null!;
            EnergyPanel = null!;
            Modifiers = null!;
            Config = null!;
            EnergyBar = null!;
        }
    }
}
