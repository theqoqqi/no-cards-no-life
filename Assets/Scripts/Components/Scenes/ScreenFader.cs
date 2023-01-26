using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Components.Scenes {
    public class ScreenFader : MonoBehaviour {

        [SerializeField] private Canvas canvas;

        [SerializeField] private Image overlayImage;

        public void Awake() {
            canvas.enabled = false;
        }

        public async Task FadeOut(float duration) {
            canvas.enabled = true;
            
            await FadeAsync(0, 1, duration);
        }

        public async Task FadeIn(float duration) {
            await FadeAsync(1, 0, duration);
            
            canvas.enabled = false;
        }

        private async Task FadeAsync(float from, float to, float duration) {
            StartCoroutine(Fade(from, to, duration));
            
            await DelaySeconds(duration);
        }

        private IEnumerator Fade(float from, float to, float duration) {
            var elapsed = 0f;

            while (elapsed < duration) {
                elapsed += Time.deltaTime;
                SetOpacity(from + (to - from) * elapsed);
                yield return null;
            }
        }

        private void SetOpacity(float opacity) {
            var color = overlayImage.color;

            color.a = opacity;

            overlayImage.color = color;
        }

        private static async Task DelaySeconds(float duration) {
            await Task.Delay(Mathf.FloorToInt(duration * 1000));
        }
    }
}