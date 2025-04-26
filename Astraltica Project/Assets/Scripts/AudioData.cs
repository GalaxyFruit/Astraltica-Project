using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioData", menuName = "Audio/AudioData")]
public class AudioData : ScriptableObject
{
    public string id;                   // Například "footstep"
    public AudioClip clip;              // Zvukový soubor
    public float volume = 1f;           // Hlasitost
    public float pitchMin = 1f;         // Rozsah náhodné výšky
    public float pitchMax = 1f;
    public bool loop;
    public float spatialBlend = 0f;     // 0 = 2D, 1 = 3D
    public float minDistance = 1f;
    public float maxDistance = 500f;
}
