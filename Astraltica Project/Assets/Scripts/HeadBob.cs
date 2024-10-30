using UnityEngine;
using System.Collections;

public class HeadBob : MonoBehaviour
{
    [SerializeField] private float bobFrequency = 5.0f;
    [SerializeField] private float bobAmplitude = 0.05f;
    [SerializeField] private float bobSmoothing = 5f;

    private Vector3 startPosition;
    private float timer = 0.0f;
    private Coroutine headBobCoroutine;

    public static HeadBob Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
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

}
