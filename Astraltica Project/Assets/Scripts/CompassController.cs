using UnityEngine;

public class CompassController : MonoBehaviour
{
    [SerializeField] private RectTransform compassContent; // Compass_Content GameObject
    [SerializeField] private Transform player;
    [SerializeField] private float maxRotation = 360f;
    [SerializeField] private float compassWidth = 1920f;
    [SerializeField] private RectTransform viewportRect;
    private float viewportWidth => viewportRect.rect.width;

    void Update()
    {
        float playerYaw = player.eulerAngles.y;
        float normalizedYaw = playerYaw / maxRotation;

        float x = -normalizedYaw * compassWidth + (viewportWidth / 2f);
        if (x < 0) x += compassWidth;

        compassContent.anchoredPosition = new Vector2(x, compassContent.anchoredPosition.y);
    }
}