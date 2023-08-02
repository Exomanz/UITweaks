using SiraUtil.Logging;
using System;
using UnityEngine;
using Zenject;

namespace UITweaks.Models
{
    public abstract class PanelModifier : MonoBehaviour
    {
        [Inject] protected SiraLog logger = null!;
        public ConfigBase config = null;
        public GameObject parentPanel = null;

        [Inject] protected abstract void Init(); 

        protected virtual void ModPanel()
        {
            try
            {
                if (parentPanel == null)
                    throw new NullReferenceException("'parentPanel' cannot be null when creating an object of type 'PanelModifier'");
                else if (config == null)
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
            this.logger = null!;
            this.config = null!;
        }
    }
}
