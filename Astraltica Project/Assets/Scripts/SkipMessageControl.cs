using TMPro;
using UnityEngine;
using System.Collections;

public class SkipMessageControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skipMessageText;
    [SerializeField] private string skipMessage = "Press any key to skip";

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float pingPongSpeed = 2f;
    [SerializeField] private float pingPongMinAlpha = 0.6f;

    [Header("Debug Settings")]
    [SerializeField, Tooltip("skipMessageText.Alpha")]
    private float alpha;

    private Coroutine activeCoroutine;

    private void Awake()
    {
        if (skipMessageText != null)
        {
            skipMessageText.text = skipMessage;
            skipMessageText.alpha = 0f;
            alpha = 0f;
            skipMessageText.gameObject.SetActive(false);
        }
    }

    public void FadeInMessage()
    {
        if (skipMessageText == null) return;

        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = StartCoroutine(AnimateText(true));
    }

    public void FadeOutMessage()
    {
        if (skipMessageText == null) return;

        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = StartCoroutine(AnimateText(false));
    }

    private IEnumerator AnimateText(bool fadeIn)
    {
        skipMessageText.gameObject.SetActive(true);
        float elapsed = 0f;

        if (fadeIn)
        {
            while (elapsed < fadeDuration)
            {
                float currentAlpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
                skipMessageText.alpha = currentAlpha;
                alpha = currentAlpha;
                elapsed += Time.deltaTime;
                yield return null;
            }
            skipMessageText.alpha = 1f;
            alpha = 1f;

            while (true)
            {
                float pingAlpha = Mathf.PingPong(Time.time * pingPongSpeed, 1f - pingPongMinAlpha) + pingPongMinAlpha;
                skipMessageText.alpha = pingAlpha;
                alpha = pingAlpha;
                yield return null;
            }
        }
        else
        {
            float startAlpha = skipMessageText.alpha;
            while (elapsed < fadeDuration)
            {
                float currentAlpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
                skipMessageText.alpha = currentAlpha;
                alpha = currentAlpha;
                elapsed += Time.deltaTime;
                yield return null;
            }

            skipMessageText.alpha = 0f;
            alpha = 0f;
            skipMessageText.gameObject.SetActive(false);
        }
    }
}
