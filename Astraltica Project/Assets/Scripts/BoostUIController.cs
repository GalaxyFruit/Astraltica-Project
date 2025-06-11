using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BoostUIController : MonoBehaviour
{
    [Header("Boost UI Elements")]
    [SerializeField] private Image boostFillImage;
    [SerializeField] private Transform boostFluidTransform;

    public void UpdateBoostLevel(float normalizedValue)
    {
        boostFillImage.fillAmount = normalizedValue;

        boostFluidTransform.DOScale(new Vector3(1, normalizedValue, 1), 0.5f)
            .SetEase(Ease.OutQuad);
    }
}
