using UnityEngine;

public class MenuSoundsController : MonoBehaviour
{
    public void PlayButtonSound()
    {
        AudioManager.Instance.PlaySound("ButtonClick", transform.position);
    }
}
