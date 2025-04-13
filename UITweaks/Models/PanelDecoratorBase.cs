using SiraUtil.Logging;
using System;
using UITweaks.Utilities;
using UnityEngine;
using Zenject;

namespace UITweaks.Models
{
    /// <summary>
    /// Helper class which contains shared resources for all UITweaks PanelDecorators, as well as helper methods to verify proper setup and disposal.
    /// </summary>
    public abstract class PanelDecoratorBase : MonoBehaviour
    {
        [Inject] private readonly GameplayCoreSceneSetupData gameplayCoreSceneSetupData;
        [Inject] private readonly RainbowEffectManager rainbowEffectManager;
        [Inject] protected readonly SiraLog logger;
        [Inject] protected readonly CoreGameHUDController gameHUDController;

        /// <summary>
        /// Determines whether a PanelDecorator is safe to use in the given context.
        /// </summary>
        /// <remarks>Always evaluates to <see langword="false"/> if the base configuration has the PanelDecorator disabled.</remarks>
        public bool CanBeUsedSafely { get; protected set; } = true;
        public Color RainbowColor => rainbowEffectManager.Rainbow;

        public UITweaksConfigBase Config;
        public GameObject ParentPanel;

        protected abstract void Init();

        protected virtual bool ModPanel(in PanelDecoratorBase callingDecorator)
        {
            string callingTypeName = callingDecorator.GetType().Name;
            logger.Logger.Debug($"Setting up new PanelDecorator of type {callingTypeName}");

            try
            {
                if (callingDecorator.ParentPanel == null)
                    throw new NullReferenceException($"Field 'parentPanel' cannot be null when creating an object of type {callingTypeName}");

                if (callingDecorator.Config == null)
                    throw new NullReferenceException($"Field 'config' cannot be null when creating an object of type {callingTypeName}");
            }
            catch (NullReferenceException ex)
            {
                logger.Logger.Error($"PanelDecorator of type {callingTypeName} cannot be properly initialized.");
                logger.Logger.Error(ex);
                callingDecorator.CanBeUsedSafely = false;
                return false;
            }

            if (gameplayCoreSceneSetupData.playerSpecificSettings.noTextsAndHuds || gameplayCoreSceneSetupData.gameplayModifiers.zenMode)
            {
                callingDecorator.CanBeUsedSafely = false;
                logger.Logger.Debug($"No Texts/HUDs OR Zen Mode are enabled. {callingTypeName} will not be initialized.");
                return false;
            }

            if (!callingDecorator.Config.Enabled)
            {
                callingDecorator.CanBeUsedSafely = false;
                logger.Logger.Debug($"PanelDecorator of type {callingTypeName} is disabled and will not be initialized.");
                return false;
            }

            logger.Logger.Debug($"Successfully initialized new PanelDecorator of type {callingTypeName}");
            callingDecorator.CanBeUsedSafely = true;
            return true;
        }

        protected virtual void OnDestroy()
        {
            ParentPanel = null;
            Config = null;
        }
    }
}
