using UnityEngine;
using System.Collections;

public class HeadBob : MonoBehaviour
{
    [SerializeField] private float bobFrequency = 5.0f;
    [SerializeField] private float bobAmplitude = 0.05f;
    [SerializeField] private float bobSmoothing = 5f;

    [Header("Dip Settings")]
    [SerializeField] private float dipAmount = 0.1f; // Kolik jednotek má kamera klesnout
    [SerializeField] private float dipDuration = 0.2f; // Jak dlouho má trvat dip

    private Vector3 startPosition;
    private float timer = 0.0f;
    private Coroutine headBobCoroutine;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    public void StartHeadBob()
    {
        if (headBobCoroutine == null)
            headBobCoroutine = StartCoroutine(HeadBobRoutine());
    }

    public void StopHeadBob()
    {
        if (headBobCoroutine != null)
        {
            StopCoroutine(headBobCoroutine);
            headBobCoroutine = null;
            transform.localPosition = startPosition;
        }
    }

    private IEnumerator HeadBobRoutine()
    {
        while (true)
        {
            timer += Time.deltaTime * bobFrequency;
            float bobOffset = Mathf.Sin(timer) * bobAmplitude;
            Vector3 targetPosition = startPosition + new Vector3(0, bobOffset, 0);

            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, bobSmoothing * Time.deltaTime);

            yield return null;
        }
    }

    public void ApplyDip()
    {
        StartCoroutine(DipRoutine());
    }

    private IEnumerator DipRoutine()
    {
        // Pokles kamery dolů
        Vector3 dipPosition = startPosition + Vector3.down * dipAmount;
        float elapsedTime = 0f;

        while (elapsedTime < dipDuration)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, dipPosition, elapsedTime / dipDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Návrat kamery zpět
        elapsedTime = 0f;
        while (elapsedTime < dipDuration)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, elapsedTime / dipDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = startPosition;
    }
}
