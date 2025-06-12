using System.Collections.Generic;
using Unity.Collections;
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

    public void PlaySound(string id, Vector3 position)
    {
        if (audioDataMap.TryGetValue(id, out AudioData data))
        {
            AudioSource source = AudioSourcePool.Instance.Get();
            source.transform.position = position;

            var command = new PlaySoundCommand(data, source, RemoveCommand);
            command.Execute();

            activeCommands.Add(command);
            Debug.Log($"Playing sound '{id}' at position {position} with volume {data.volume} and pitch range {data.pitchMin}-{data.pitchMax}.");
        }
        else
        {
            Debug.LogError($"Sound ID '{id}' not found in audio data.");
        }
    }

    private void RemoveCommand(PlaySoundCommand command)
    {
        activeCommands.Remove(command);
    }


    public void StopAll(string excludedSoundId)
    {
        Debug.Log("Stopping all audio commands...");

        if (activeCommands == null) return;

        foreach (var command in new List<IAudioCommand>(activeCommands))
        {
            if (command is PlaySoundCommand playCommand && playCommand.Data.id == excludedSoundId)
                continue;

            command.Release();
            activeCommands.Remove(command);
        }
    }





    private void OnDestroy()
    {
        StopAll("");
    }
}