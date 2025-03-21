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

    private readonly string[] originalName = {"Color_F09C8EBB", "Color_C387573F", "Vector1_F42CB728" };

    private void Start()
    {
        if (skyDomeMaterial != null)
        {
            //Debug.Log("Has TopColour: " + skyDomeMaterial.HasProperty(originalName[0]));
            //Debug.Log("Has BottomColour: " + skyDomeMaterial.HasProperty(originalName[1]));
            //Debug.Log("Has GradientPower: " + skyDomeMaterial.HasProperty(originalName[2]));

            originalColorTop = skyDomeMaterial.GetColor(originalName[0]);
            originalColorBottom = skyDomeMaterial.GetColor(originalName[1]);
            originalFalloff = skyDomeMaterial.GetFloat(originalName[2]);
        }
    }

    public void SetStormSky()
    {
        if (skyDomeMaterial != null)
        {
            skyDomeMaterial.SetColor(originalName[0], stormColorTop);
            skyDomeMaterial.SetColor(originalName[1], stormColorBottom);
            skyDomeMaterial.SetFloat(originalName[2], stormFalloff);
        }
    }

    public void ResetSky()
    {
        if (skyDomeMaterial != null)
        {
            skyDomeMaterial.SetColor(originalName[0], originalColorTop);
            skyDomeMaterial.SetColor(originalName[1], originalColorBottom);
            skyDomeMaterial.SetFloat(originalName[2], originalFalloff);
        }
    }
}
