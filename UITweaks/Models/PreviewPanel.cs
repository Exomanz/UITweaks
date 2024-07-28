using UnityEngine;

namespace UITweaks.Models
{
    public class PreviewPanel
    {
        public GameObject Panel { get; set; }
        public int ActiveTab { get; set; }

        public PreviewPanel(int activeTab)
        {
            ActiveTab = activeTab;
        }

        public PreviewPanel(int activeTab, GameObject panel)
        {
            ActiveTab = activeTab;
            Panel = panel;
        }
    }
}
