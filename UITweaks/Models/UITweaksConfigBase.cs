namespace UITweaks.Models
{
    /// <summary>
    /// Helper class that allows for easy Zenject-ification.
    /// <br></br>All configs inherit this class.
    /// </summary>
    public abstract class UITweaksConfigBase
    {
        /// <summary>
        /// Controls whether a PanelDecorator is active in the given context.
        /// </summary>
        public virtual bool Enabled { get; set; }
    }
}
