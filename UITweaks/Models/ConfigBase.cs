namespace UITweaks.Models
{
    /// <summary>
    /// Helper class that allows for easy Zenject-ification.
    /// <br></br>All configs inherit this class.
    /// </summary>
    public abstract class ConfigBase
    {
        /// <summary>
        /// Determines whether a panel modifier is active or not.
        /// </summary>
        public virtual bool Enabled { get; set; }
    }
}
