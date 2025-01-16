using UnityEngine;

public class SkyDomeController : MonoBehaviour
{
    [Header("Sky Dome Settings")]
    public Material skyDomeMaterial; 
    public Color stormColorTop = new Color(0.29f, 0.33f, 0.41f);
    public Color stormColorBottom = new Color(0.76f, 0.72f, 0.64f);
    public float stormFalloff = 0.7f;

    private Color originalColorTop;
    private Color originalColorBottom;
    private float originalFalloff;

    private void Start()
    {
        if (skyDomeMaterial != null)
        {
            originalColorTop = skyDomeMaterial.GetColor("_ColorTop");
            originalColorBottom = skyDomeMaterial.GetColor("_ColorBottom");
            originalFalloff = skyDomeMaterial.GetFloat("_Falloff");
        }
    }

    public void SetStormSky()
    {
        if (skyDomeMaterial != null)
        {
            skyDomeMaterial.SetColor("_ColorTop", stormColorTop);
            skyDomeMaterial.SetColor("_ColorBottom", stormColorBottom);
            skyDomeMaterial.SetFloat("_Falloff", stormFalloff);
        }
    }

    public void ResetSky()
    {
        if (skyDomeMaterial != null)
        {
            skyDomeMaterial.SetColor("_ColorTop", originalColorTop);
            skyDomeMaterial.SetColor("_ColorBottom", originalColorBottom);
            skyDomeMaterial.SetFloat("_Falloff", originalFalloff);
        }
    }
}
