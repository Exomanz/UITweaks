using SiraUtil.Logging;
using System;
using UITweaks.Config;
using UnityEngine;
using Zenject;

namespace UITweaks.Models
{
    public abstract class PanelModifierBase : MonoBehaviour
    {
        [Inject] private readonly RainbowEffectManager rainbowEffectManager;
        [Inject] protected readonly SiraLog logger = null!;
        [Inject] protected readonly CoreGameHUDController gameHUDController;

        public Color RainbowColor { get; private set; }
        public UITweaksConfigBase config = null;
        public GameObject parentPanel = null;

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
