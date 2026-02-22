using UnityEngine;
using UnityEngine.UI;

namespace PirateGame.UI
{
    public class UINotification : MonoBehaviour
    {
        [SerializeField] private Text notificationText;
        [SerializeField] private float displayDuration = 3f;
        [SerializeField] private float fadeDuration = 0.5f;

        private CanvasGroup canvasGroup;
        private float timer = 0f;
        private bool isShowing = false;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            // Start hidden
            canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!isShowing) return;

            timer += Time.deltaTime;

            if (timer <= fadeDuration)
            {
                // Fade in
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            }
            else if (timer >= displayDuration - fadeDuration)
            {
                // Fade out
                float fadeOutTime = timer - (displayDuration - fadeDuration);
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeOutTime / fadeDuration);
            }

            if (timer >= displayDuration)
            {
                HideNotification();
            }
        }

        public void ShowNotification(string message)
        {
            if (notificationText != null)
            {
                notificationText.text = message;
            }

            gameObject.SetActive(true);
            isShowing = true;
            timer = 0f;
            canvasGroup.alpha = 0f;
        }

        private void HideNotification()
        {
            isShowing = false;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Set the Text component for displaying notifications
        /// </summary>
        /// <param name="textComponent">The Text component for notifications</param>
        public void SetNotificationText(Text textComponent)
        {
            notificationText = textComponent;
        }

        /// <summary>
        /// Set how long the notification should be displayed (in seconds)
        /// </summary>
        /// <param name="duration">Display duration in seconds</param>
        public void SetDisplayDuration(float duration)
        {
            displayDuration = duration;
        }

        /// <summary>
        /// Set how long the fade in/out should take (in seconds)
        /// </summary>
        /// <param name="duration">Fade duration in seconds</param>
        public void SetFadeDuration(float duration)
        {
            fadeDuration = duration;
        }
    }
}
