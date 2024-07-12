using SiraUtil.Logging;
using System;
using UITweaks.Utilities;
using UnityEngine;
using Zenject;

namespace UITweaks.Models
{
    public abstract class PanelDecoratorBase : MonoBehaviour
    {
        [Inject] private readonly RainbowEffectManager rainbowEffectManager;
        [Inject] protected readonly SiraLog logger;
        [Inject] protected readonly CoreGameHUDController gameHUDController;

        public Color RainbowColor { get; private set; }
        public UITweaksConfigBase config;
        public GameObject parentPanel;

        [Inject] protected abstract void Init();

        protected virtual void ModPanel(in PanelDecoratorBase callingDecorator)
        {
            string callingTypeName = callingDecorator.GetType().Name;
            logger.Logger.Debug($"Initializing new PanelDecorator of type {callingTypeName}");

            try
            {
                if (callingDecorator.parentPanel == null)
                    throw new NullReferenceException($"'parentPanel' cannot be null when creating an object of type {callingTypeName}");

                if (callingDecorator.config == null)
                    throw new NullReferenceException($"'config' cannot be null when creating an object of type {callingTypeName}");
            }
            catch (NullReferenceException ex)
            {
                logger.Error(ex);
            }

            logger.Logger.Debug($"Successfully bound new PanelDecorator of type {callingTypeName}");
        }

        public void LateUpdate()
        {
            RainbowColor = rainbowEffectManager.Rainbow;
        }

        protected virtual void OnDestroy()
        {
            parentPanel = null;
            config = null;
        }
    }
}
