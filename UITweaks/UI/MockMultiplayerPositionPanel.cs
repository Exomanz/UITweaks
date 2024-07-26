using HMUI;
using UnityEngine;

namespace UITweaks.UI
{
    public class MockMultiplayerPositionPanel : MonoBehaviour
    {
        [SerializeField] public CurvedTextMeshPro positionText;
        [SerializeField] public CurvedTextMeshPro playerCountText;
        [SerializeField] public bool IsSetup { get; private set; } = false;

        public void Start()
        {
            gameObject.SetActive(false);

            var pt = new GameObject("PositionText").AddComponent<CurvedTextMeshPro>();
            pt.transform.SetParent(this.transform, false);
            pt.alignment = TMPro.TextAlignmentOptions.Center;
            pt.fontStyle = TMPro.FontStyles.Italic;
            pt.text = "1";
            pt.transform.localPosition = new Vector3(-20, 0, 0);

            var pct = new GameObject("PlayerCountText").AddComponent<CurvedTextMeshPro>();
            pct.transform.SetParent(this.transform, false);
            pct.alignment = TMPro.TextAlignmentOptions.Center;
            pct.fontStyle = TMPro.FontStyles.Italic;
            pct.text = "/ 5";
            pct.transform.localPosition = new Vector3(10, 0, 0);

            positionText = pt;
            playerCountText = pct;

            gameObject.SetActive(true);
            gameObject.AddComponent<Canvas>();
            transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            transform.localPosition = new Vector3(-0.08f, 0, 0);

            IsSetup = true;
        }
    }
}
