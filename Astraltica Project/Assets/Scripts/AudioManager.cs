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
            DontDestroyOnLoad(gameObject);
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

    public void PlaySound(string id, Vector3? position = null, Transform follow = null)
    {
        if (audioDataMap.TryGetValue(id, out AudioData data))
        {
            var command = new PlaySoundCommand(data, position, follow);
            command.Execute();
            if (data.loop)
                activeCommands.Add(command);
        }
    }

    public void StopAll()
    {
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