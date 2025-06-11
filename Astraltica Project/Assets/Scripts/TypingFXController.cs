using TMPro;
using UnityEngine;
using System.Collections;

public class TypingFXController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private float typingSpeedWaitForSeconds = 0.02f;

    [Tooltip("The full text to display when typing starts.")]
    public string fullText;

    private Coroutine typingCoroutine;

    public void StartTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        textComponent.text = "";
        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeedWaitForSeconds);
        }
    }

    public void SetNewText(string newText)
    {
        fullText = newText;
        StartTyping();
    }

    public void StopTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        textComponent.text = fullText;
    }

    public void ClearText()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        textComponent.text = "";
    }
}
