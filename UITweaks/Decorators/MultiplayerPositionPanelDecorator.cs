using UITweaks.Models;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class MultiplayerPositionPanelDecorator : PanelDecoratorBase
    {
        [Inject] protected override void Init() 
        {
            ModPanel(this);
        }

        protected override bool ModPanel(in PanelDecoratorBase decorator)
        {
            if (!base.ModPanel(this)) return false;
            return true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
