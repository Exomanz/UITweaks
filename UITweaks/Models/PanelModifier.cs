using SiraUtil.Logging;
using UnityEngine;
using Zenject;

namespace UITweaks.Models
{
    public abstract class PanelModifier : MonoBehaviour
    {
        public SiraLog Logger = null!;

        [Inject] public void __Init(SiraLog logger)
        {
            Logger = logger;
        }

        protected abstract void ModPanel();

        protected abstract void OnDestroy();
    }
}
