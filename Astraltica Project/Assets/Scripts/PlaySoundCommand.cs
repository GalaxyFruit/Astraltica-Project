using System.Collections;
using UnityEngine;

public class PlaySoundCommand : IAudioCommand
{
    private readonly AudioData audioData;
    private readonly AudioSource audioSource;

    public PlaySoundCommand(AudioData data, AudioSource source)
    {
        audioData = data;
        audioSource = source;
    }

    public void Execute()
    {
        ConfigureAudioSource();
        PlaySound();
    }

    private void ConfigureAudioSource()
    {
        audioSource.clip = audioData.clip;
        audioSource.volume = audioData.volume;
        audioSource.pitch = Random.Range(audioData.pitchMin, audioData.pitchMax);
        audioSource.loop = audioData.loop;
        audioSource.spatialBlend = audioData.spatialBlend;
        audioSource.minDistance = audioData.minDistance;
        audioSource.maxDistance = audioData.maxDistance;
    }

    private void PlaySound()
    {
        audioSource.Play();
        if (!audioData.loop)
        {
            AudioManager.Instance.StartCoroutine(ReleaseAfterPlay(audioData.clip.length));
        }
    }

    private IEnumerator ReleaseAfterPlay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Release();
    }

    public void Release()
    {
        if (audioSource != null)
        {
            AudioSourcePool.Instance.Release(audioSource);
        }
    }
}
