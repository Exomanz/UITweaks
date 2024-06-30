using SiraUtil.Logging;
using System;
using UnityEngine;
using Zenject;

namespace UITweaks.Models
{
    public abstract class PanelModifierBase : MonoBehaviour
    {
        [Inject] protected readonly SiraLog logger = null!;
        [Inject] protected readonly CoreGameHUDController gameHUDController;

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

        protected virtual void OnDestroy()
        {
            this.parentPanel = null!;
            this.config = null!;
        }
    }
}
