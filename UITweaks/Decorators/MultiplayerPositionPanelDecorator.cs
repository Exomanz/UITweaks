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

        protected override void ModPanel(in PanelDecoratorBase decorator)
        {
            base.ModPanel(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
