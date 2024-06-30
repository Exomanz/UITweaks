using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UITweaks.Models;
using UnityEngine;

namespace UITweaks.Config
{
    public class PositionConfig : UITweaksConfigBase
    {
        public override bool Enabled { get; set; } = true;

        /// <summary>
        /// If set to <see langword="true"/>, the 1st place animation object will be hidden.
        /// </summary>
        public virtual bool HideFirstPlaceAnimation { get; set; } = false;

        /// <summary>
        /// If set to <see langword="true"/>, the player count panel will use a static color regardless of your position.
        /// </summary>
        public virtual bool UseStaticColorForStaticPanel { get; set; } = false;

        /// <summary>
        /// The <see cref="Color"/> of the player count section of the Position Panel.
        /// <br></br>This will only take effect if <see cref="UseStaticColorForPlayerCount"/> is <see langword="true"/>.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color StaticPanelColor { get; set; } = Color.white;

        /// <summary>
        /// The <see cref="Color"/> of the Position Panel when you are in first place.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))] 
        public virtual Color First { get; set; } = Color.cyan;

        /// <summary>
        /// The <see cref="Color"/> of the Position Panel when you are in second place.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))] 
        public virtual Color Second { get; set; } = Color.green;

        /// <summary>
        /// The <see cref="Color"/> of the Position Panel when you are in third place.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))] 
        public virtual Color Third { get; set; } = Color.yellow;

        /// <summary>
        /// The <see cref="Color"/> of the Position Panel when you are in fourth place.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))] 
        public virtual Color Fourth { get; set; } = new Color(1f, 0.5f, 0f);

        /// <summary>
        /// The <see cref="Color"/> of the Position Panel when you are in fifth place.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))] 
        public virtual Color Fifth { get; set; } = Color.red;
    }
}
