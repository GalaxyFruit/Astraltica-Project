using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioData[] audioData;
    private Dictionary<string, AudioData> audioDataMap;
    private List<IAudioCommand> activeCommands;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        audioDataMap = new Dictionary<string, AudioData>();
        activeCommands = new List<IAudioCommand>();

        foreach (var data in audioData)
        {
            audioDataMap[data.id] = data;
        }
    }

    public void PlaySound(string id, Vector3 position)
    {
        if (audioDataMap.TryGetValue(id, out AudioData data))
        {
            AudioSource source = AudioSourcePool.Instance.Get();
            source.transform.position = position;

            var command = new PlaySoundCommand(data, source);
            command.Execute();

            // activeCommands.Add(command);
        }
        else
        {
            Debug.LogError($"Sound ID '{id}' not found in audio data.");
        }
    }



    public void StopAll()
    {
        Debug.Log("Stopping all audio commands...");
        foreach (var command in activeCommands)
        {
            command.Release();
        }
        activeCommands.Clear();
    }

    private void OnDestroy()
    {
        StopAll();
    }
}