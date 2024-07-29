using UnityEngine;

namespace UITweaks.Models
{
    /// <summary>
    /// Helper class which contains some useful properties for preview panel iteration and code reusability.
    /// </summary>
    public class PreviewPanel
    {
        /// <summary>
        /// The parent <see cref="GameObject"/> of the panel being previewed.
        /// </summary>
        public GameObject Panel { get; set; }

        /// <summary>
        /// The settings tab index this panel is active on.
        /// </summary>
        public int ActiveTab { get; set; }

        /// <summary>
        /// Creates a new instance of a <see cref="PreviewPanel"/> with a given parent object and settings tab index.
        /// </summary>
        /// <param name="activeTab">The index of the settings page this panel will appear on.</param>
        /// <param name="panel">The parent <see cref="GameObject"/> of the panel being previewed.</param>
        public PreviewPanel(int activeTab, GameObject panel)
        {
            ActiveTab = activeTab;
            Panel = panel;
        }
    }
}
