using UnityEngine;
using UnityEngine.Playables;

public class StargateTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerAnimationController playerAnimationController;
    [SerializeField] private Stargate _stargate;

    [SerializeField] private bool playOnce = true;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && _stargate.evacuationReady)
        {
            if (playOnce && hasPlayed) return;

            hasPlayed = true;

            Debug.Log("Stargate sequence started.");

            if (_stargate != null)
            {
                playerAnimationController.StartStargateAnim();
                _stargate.DisableInputs();
            }
            else
            {
                Debug.LogError("Stargate reference is not set.");
            }
        }
    }
}
