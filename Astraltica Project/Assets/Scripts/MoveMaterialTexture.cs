using UnityEngine;
using System.Collections;

public class ScrollTexture : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private float speed = 1f; // 🔧 rychlost scrollování

    private Vector2 currentOffset;
    private Vector2 scrollDirection; // normalizovaný směr
    private MaterialPropertyBlock propBlock;

    private const float maxOffsetX = 0.08f;
    private const float maxOffsetY = 0.08f;

    void Start()
    {
        propBlock = new MaterialPropertyBlock();
        currentOffset = new Vector2(
            Random.Range(-maxOffsetX, maxOffsetX),
            Random.Range(-maxOffsetY, maxOffsetY)
        );
        GenerateNewScrollDirection();
        StartCoroutine(ScrollLoop());
    }

    IEnumerator ScrollLoop()
    {
        while (true)
        {
            Vector2 movement = scrollDirection * speed * Time.deltaTime;
            currentOffset += movement;

            if (Mathf.Abs(currentOffset.x) >= maxOffsetX)
            {
                currentOffset.x = Mathf.Sign(currentOffset.x) * maxOffsetX;
                scrollDirection.x *= -1f;
            }

            if (Mathf.Abs(currentOffset.y) >= maxOffsetY)
            {
                currentOffset.y = Mathf.Sign(currentOffset.y) * maxOffsetY;
                scrollDirection.y *= -1f;
            }

            ApplyOffset(currentOffset);
            yield return null;
        }
    }

    private void GenerateNewScrollDirection()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);

        // Zaručit minimální odchylku a normalizovat směr
        Vector2 dir = new Vector2(x, y);
        if (dir.magnitude < 0.1f)
            dir = new Vector2(0.1f, 0.1f);

        scrollDirection = dir.normalized;
    }

    private void ApplyOffset(Vector2 offset)
    {
        targetRenderer.GetPropertyBlock(propBlock);
        propBlock.SetVector("_BaseMap_ST", new Vector4(1, 1, offset.x, offset.y));
        targetRenderer.SetPropertyBlock(propBlock);
    }
}
