using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    public static AudioSourcePool Instance { get; private set; }

    [SerializeField] private int initialPoolSize = 20;
    private Queue<AudioSource> audioSourcePool;
    private Transform poolContainer;

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
        audioSourcePool = new Queue<AudioSource>();
        poolContainer = new GameObject("AudioSourcePool").transform;
        poolContainer.parent = transform;

        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewAudioSource();
        }
    }

    private void CreateNewAudioSource()
    {
        var go = new GameObject("AudioSource");
        go.transform.parent = poolContainer;
        var source = go.AddComponent<AudioSource>();
        source.playOnAwake = false;
        audioSourcePool.Enqueue(source);
        go.SetActive(false);
    }

    public AudioSource Get()
    {
        if (audioSourcePool.Count == 0)
            CreateNewAudioSource();

        var source = audioSourcePool.Dequeue();
        source.gameObject.SetActive(true);
        return source;
    }

    public void Release(AudioSource source)
    {
        source.Stop();
        source.clip = null;
        source.transform.parent = poolContainer;
        source.gameObject.SetActive(false);
        audioSourcePool.Enqueue(source);
    }
}
