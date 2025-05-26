using UnityEngine;

public class CompassController : MonoBehaviour
{
    [SerializeField] private RectTransform compassImage; // ten dlouhý Image
    [SerializeField] private Transform player; // player nebo kamera
    [SerializeField] private float maxRotation = 360f; // kolik stupňů pokrývá textura
    [SerializeField] private float compassWidth = 1920f; // šířka kompasu v pixelech
    [SerializeField] private RectTransform viewportRect; // assign your mask panel here
    private float viewportWidth => viewportRect.rect.width;


    private float halfCompassWidth;

    void Start()
    {
        Debug.Log("Aktivace comapssu");

        halfCompassWidth = compassWidth / 2f;
    }

    void Update()
    {
        float playerYaw = player.eulerAngles.y;

        // Normalized position (0..1)
        float normalizedYaw = playerYaw / maxRotation;

        // Calculate the pixel offset for the compass image
        float x = -normalizedYaw * (compassWidth - viewportWidth);

        // Loop the compass image for seamless scrolling
        x = Mathf.Repeat(x, compassWidth - viewportWidth);

        // Set the anchored position
        compassImage.anchoredPosition = new Vector2(x, compassImage.anchoredPosition.y);
    }
}
