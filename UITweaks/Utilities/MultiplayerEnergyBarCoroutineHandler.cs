using System.Collections;
using UITweaks.PanelModifiers;
using UnityEngine;
using Zenject;

namespace UITweaks.Utilities
{
    internal class MultiplayerEnergyBarCoroutineHandler : MonoBehaviour
    {
        [Inject] private readonly EnergyBarPanelDecorator energyBarPanelDecorator;
        [Inject] private readonly GameplayModifiers gameplayModifiers;

        public void Start()
        {
            transform.SetParent(base.transform);
            gameObject.SetActive(true);
            base.StartCoroutine(PrepareMultiplayerEnergyBarColorsForEnergyType(gameplayModifiers.energyType));
        }

        private IEnumerator PrepareMultiplayerEnergyBarColorsForEnergyType(GameplayModifiers.EnergyType energyType)
        {
            yield return StartCoroutine(energyBarPanelDecorator.BatteryEnergyAndOneLifeSetup(energyType));
            gameObject.SetActive(false);
        }
    }
}
