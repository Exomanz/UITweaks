using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UITweaks.Models;
using UnityEngine;

namespace UITweaks.Config
{
    public class Progress : ConfigBase
    {
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
        /// If set to <see langword="true"/>, the Progress Bar will fade from one color to another as the song progresses.
        /// </summary>
        public virtual bool UseFadeDisplayType { get; set; } = false;

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
