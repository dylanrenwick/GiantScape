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
            loadingText.gameObject.SetActive(vis);
            loadingBar.gameObject.SetActive(vis);
        }

        public void SetState(string text, float barValue)
        {
            loadingText.text = text;
            loadingBar.value = barValue;
        }
    }
}
