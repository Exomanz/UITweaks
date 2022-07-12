using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UITweaks.Models;
using UnityEngine;

namespace UITweaks.Config
{
    public class ComboConfig : ConfigBase
    {
        public override bool Enabled { get; set; } = true;

        /// <summary>
        /// The <see cref="Color"/> of the top Combo line.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))] 
        public virtual Color TopLine { get; set; } = Color.cyan;

        /// <summary>
        /// The <see cref="Color"/> of the bottom Combo line.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color BottomLine { get; set; } = Color.cyan;

        /// <summary>
        /// If set to <see langword="true"/>, each Combo line will have a gradient. 
        /// </summary>
        public virtual bool UseGradient { get; set; } = true;

        /// <summary>
        /// If set to <see langword="true"/>, the bottom Combo line will reflect the top Combo line horizontally.
        /// <br></br>This will only take effect if <see cref="UseGradient"/> is <see langword="true"/>
        /// </summary>
        public virtual bool MirrorBottomLine { get; set; } = true;

        /// <summary>
        /// The left <see cref="Color"/> of the top Combo line. 
        /// <br></br>This will only take effect if <see cref="UseGradient"/> is <see langword="true"/>.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color TopLeft { get; set; } = Color.red;

        /// <summary>
        /// The right <see cref="Color"/> of the top Combo line. 
        /// <br></br>This will only take effect if <see cref="UseGradient"/> is <see langword="true"/>.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color TopRight { get; set; } = Color.yellow;

        /// <summary>
        /// The left <see cref="Color"/> of the bottom Combo line. 
        /// <br></br>This will only take effect if <see cref="UseGradient"/> is <see langword="true"/> and <see cref="MirrorBottomLine"/> is <see langword="false"/>.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color BottomLeft { get; set; } = Color.white;

        /// <summary>
        /// The right <see cref="Color"/> of the bottom Combo line. 
        /// <br></br>This will only take effect if <see cref="UseGradient"/> is <see langword="true"/> and <see cref="MirrorBottomLine"/> is <see langword="false"/>.
        /// </summary>
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color BottomRight { get; set; } = Color.white;
    }
}
