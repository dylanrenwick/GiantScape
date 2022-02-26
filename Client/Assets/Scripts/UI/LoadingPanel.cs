using UnityEngine;
using UnityEngine.UI;

namespace GiantScape.Client.UI
{
    public class LoadingPanel : MonoBehaviour
    {
        [SerializeField]
        private Text loadingText;
        [SerializeField]
        private Slider loadingBar;

        public void Show() => SetVisibility(true);
        public void Hide() => SetVisibility(false);
        public void SetVisibility(bool vis)
        {
            if (loadingText != null) loadingText.gameObject.SetActive(vis);
            if (loadingBar != null) loadingBar.gameObject.SetActive(vis);
        }

        public void SetState(string text, float barValue)
        {
            if (loadingText != null) loadingText.text = text;
            if (loadingBar != null) loadingBar.value = barValue;
        }
    }
}
