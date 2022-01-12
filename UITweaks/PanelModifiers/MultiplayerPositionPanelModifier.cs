using UITweaks.Models;
using Zenject;

namespace UITweaks.PanelModifiers
{
    public class MultiplayerPositionPanelModifier : PanelModifier
    {
        private MultiplayerPositionHUDController Controller = null!;
        private Config.Position Config = null!;

        [Inject] internal void ModifierInit()
        {
            // Stuff
            ModPanel();
        }

        protected override void ModPanel()
        {
            
        }

        protected override void OnDestroy()
        {
            Controller = null!;
            Config = null!;
        }
    }
}
