using SiraUtil.Logging;
using System;
using UITweaks.Config;
using UnityEngine;
using Zenject;

namespace UITweaks.Models
{
    public abstract class PanelModifierBase : MonoBehaviour
    {
        [Inject] private readonly MiscConfig miscConfig;
        [Inject] protected readonly SiraLog logger = null!;
        [Inject] protected readonly CoreGameHUDController gameHUDController;

        public Color RainbowColor { get; private set; } = Color.white;
        public UITweaksConfigBase config = null;
        public GameObject parentPanel = null;

        [Inject] protected abstract void Init(); 

        protected virtual void ModPanel()
        {
            try
            {
                if (parentPanel == null)
                    throw new NullReferenceException("'parentPanel' cannot be null when creating an object of type 'PanelModifier'");

                if (config == null)
                    throw new NullReferenceException("'config' cannot be null when creating an object of type 'PanelModifier'");
            }
            catch (NullReferenceException ex)
            {
                logger.Error(ex);
            }
        }

        public void LateUpdate()
        {
            RainbowColor = new HSBColor(
                Mathf.PingPong(Time.time * miscConfig.GlobalRainbowSpeed, 1),
                1,
                1).ToColor();
        }

        protected virtual void OnDestroy()
        {
            this.parentPanel = null;
            this.config = null;
        }
    }
}
