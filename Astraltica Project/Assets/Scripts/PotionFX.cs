using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PotionFX : MonoBehaviour
{
    [SerializeField] private Image fxImage;
    [SerializeField] private PotionCraftingManager craftingManager;

    private Coroutine fxCoroutine;

    void Start()
    {
        if (craftingManager != null)
        {
            // Optionally subscribe to an event or poll for crafting state
        }
    }

    public void PlayCraftingFX()
    {
        if (fxCoroutine != null)
            StopCoroutine(fxCoroutine);

        fxCoroutine = StartCoroutine(CraftingFXCoroutine());
    }

    private IEnumerator CraftingFXCoroutine()
    {
        float duration = craftingManager.CraftingDuration;
        float phaseDuration = duration * 0.8f / 2f;

        fxImage.fillOrigin = (int)Image.OriginHorizontal.Right;
        fxImage.fillAmount = 0f;
        float t = 0f;
        while (t < phaseDuration)
        {
            fxImage.fillAmount = Mathf.Lerp(0f, 1f, t / phaseDuration);
            t += Time.deltaTime;
            yield return null;
        }
        fxImage.fillAmount = 1f;

        // Phase 2: Fill from 1 to 0, origin left
        fxImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        t = 0f;
        while (t < phaseDuration)
        {
            fxImage.fillAmount = Mathf.Lerp(1f, 0f, t / phaseDuration);
            t += Time.deltaTime;
            yield return null;
        }
        fxImage.fillAmount = 0f;

        // Reset
        fxImage.fillOrigin = (int)Image.OriginHorizontal.Right;
    }
}
