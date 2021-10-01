using HMUI;
using UnityEngine;
using UnityEngine.UI;

namespace UITweaks.Utilities
{
    /// <summary>
    /// Class for recreating the base-game position panel in multiplayer. This is not an exact copy!
    /// </summary>
    public class MockPositionPanel : MonoBehaviour
    {
        [SerializeField] public CurvedTextMeshPro positionText;
        [SerializeField] public CurvedTextMeshPro playersText;
        [SerializeField] public CurvedTextMeshPro dividerSlash;
        [SerializeField] public bool isDoneSettingUp = false;

        public void Start()
        {
            gameObject.SetActive(false);

            positionText = new GameObject("PositionText").AddComponent<CurvedTextMeshPro>();
            positionText.transform.SetParent(transform, false);
            positionText.alignment = TMPro.TextAlignmentOptions.Center;
            positionText.fontStyle = TMPro.FontStyles.Italic;
            positionText.transform.localPosition = new Vector3(-20, 0, 0);

            playersText = new GameObject("PlayersText").AddComponent<CurvedTextMeshPro>();
            playersText.transform.SetParent(transform, false);
            playersText.alignment = TMPro.TextAlignmentOptions.Center;
            playersText.fontStyle = TMPro.FontStyles.Italic;
            playersText.text = "5";
            playersText.transform.localPosition = new Vector3(20, 0, 0);

            dividerSlash = new GameObject("Divider").AddComponent<CurvedTextMeshPro>();
            dividerSlash.transform.SetParent(transform, false);
            dividerSlash.alignment = TMPro.TextAlignmentOptions.Center;
            dividerSlash.fontStyle = TMPro.FontStyles.Italic;
            dividerSlash.text = "/";

            gameObject.SetActive(true);
            gameObject.AddComponent<Canvas>();
            transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            transform.localPosition = new Vector3(-0.08f, 0, 0);

            isDoneSettingUp = true;
        }
    }
}
