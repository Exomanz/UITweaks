using SiraUtil.Logging;
using System;
using System.Runtime.CompilerServices;
using UITweaks.Utilities;
using UnityEngine;
using Zenject;

namespace UITweaks.Models
{
    public abstract class PanelModifierBase : MonoBehaviour
    {
        [Inject] private readonly RainbowEffectManager rainbowEffectManager;
        [Inject] protected readonly SiraLog logger;
        [Inject] protected readonly CoreGameHUDController gameHUDController;

        public Color RainbowColor { get; private set; }
        public UITweaksConfigBase config;
        public GameObject parentPanel;

        [Inject] protected virtual void Init([CallerMemberName] string caller = "PanelModifierBase")
        {
            logger.Logger.Debug($"{caller}::Init()");
        } 

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
            RainbowColor = rainbowEffectManager.Rainbow;
        }

        protected virtual void OnDestroy()
        {
            this.parentPanel = null;
            this.config = null;
        }
    }
}
