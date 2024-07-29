using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UITweaks.Models;
using UnityEngine;

namespace UITweaks.Config
{
    public class ProgressConfig : UITweaksConfigBase
    {
        public enum DisplayType
        {
            Fixed = 0,
            Fade = 1,
        }

        public override bool Enabled { get; set; } = true;

        /// <summary>
        /// The <see cref="Color"/> of the Progress Bar's fill section.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color Fill { get; set; } = Color.white;

        /// <summary>
        /// The <see cref="Color"/> of the Progress Bar's background.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color BG { get; set; } = Color.white;

        /// <summary>
        /// The <see cref="Color"/> of the Progress Bar's handle, which is anchored to the Fill section.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color Handle { get; set; } = Color.white;

        /// <summary>
        /// Controls the mode in which the progress bar is decorated.
        /// <list type="bullet">
        /// <item>Fixed: All components will be statically colored for the entire song.</item>
        /// <item>Fade: The Background component of the progress bar will fade between <see cref="StartColor"/> and <see cref="EndColor"/> as the song progresses.</item>
        /// </list>
        /// </summary>
        public virtual DisplayType Mode { get; set; } = DisplayType.Fixed;

        /// <summary>
        /// The <see cref="Color"/> of the progress bar fill section at the start of the song.
        /// <br></br>This will only take effect if <see cref="UseFadeDisplayType"/> is <see langword="true"/>.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color StartColor { get; set; } = Color.red;

        /// <summary>
        /// The <see cref="Color"/> of the Progress Bar fill section at the end of the song.
        /// <br></br>This will only take effect if <see cref="UseFadeDisplayType"/> is <see langword="true"/>.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color EndColor { get; set; } = Color.green;
    }
}
