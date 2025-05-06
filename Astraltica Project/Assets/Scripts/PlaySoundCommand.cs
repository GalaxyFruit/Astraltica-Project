using System.Collections;
using UnityEngine;

public class PlaySoundCommand : IAudioCommand
{
    private readonly AudioData audioData;
    private AudioSource audioSource;
    private readonly Vector3? position;
    private readonly Transform follow;

    public PlaySoundCommand(AudioData data, Vector3? pos = null, Transform followTransform = null)
    {
        audioData = data;
        position = pos;
        follow = followTransform;
    }

    public void Execute()
    {
        //Debug.Log($"Playing sound: {audioData.clip.name} at position: {position}");
        audioSource = AudioSourcePool.Instance.Get();
        if (audioSource != null)
        {
            ConfigureAudioSource();
            PlaySound();
        }
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

        if (position.HasValue)
            audioSource.transform.position = position.Value;
        if (follow != null)
            audioSource.transform.parent = follow;
    }

    private void PlaySound()
    {
        //Debug.Log($"Playing sound at PlaySound(): {audioData.clip.name}");
        audioSource.Play();
        if (!audioData.loop)
        {
            float clipLength = audioData.clip.length;
            AudioManager.Instance.StartCoroutine(ReleaseAfterPlay(clipLength));
        }
    }

    private IEnumerator ReleaseAfterPlay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Debug.Log($"Releasing sound: {audioData.clip.name}");
        Release();
    }

    public void Release()
    {
        if (audioSource != null)
        {
            AudioSourcePool.Instance.Release(audioSource);
            audioSource = null;
            //Debug.Log($"Released sound: {audioData.clip.name}");
        }
    }
}
