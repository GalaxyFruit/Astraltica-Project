using UnityEngine;
using UnityEngine.Playables;

public class StargateTrigger : MonoBehaviour
{
    [SerializeField] private Stargate _stargate;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private bool playOnce = true;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && _stargate.evacuationReady)
        {
            if (playOnce && hasPlayed) return;

            hasPlayed = true;
            director.Play();

            Debug.Log("Stargate sequence started.");
        }
    }
}
