using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public Renderer targetRenderer;
    public Vector2 scrollSpeed = new Vector2(0.5f, 0f);
    private Vector2 currentOffset;

    void Update()
    {
        currentOffset += scrollSpeed * Time.deltaTime;

        currentOffset.x %= 0.05f;
        currentOffset.y %= 0.08f;

        targetRenderer.material.SetTextureOffset("_BaseMap", currentOffset);


    }
}
