using System.Collections.Generic;
using UnityEngine;

public static class AudioManager
{
    private static AudioSettings audioSettings;

    private static readonly Dictionary<AudioEvents, AudioSettings.AudioEventData> audioEventDict = new();
    private static readonly Dictionary<AudioEvents, AudioSource> playingSources = new();

    private static GameObject rootObject;
    private static bool initialized = false;

    private static void Init()
    {
        if (initialized) return;
        initialized = true;

        // Resources'dan AudioSettings assetini bul
        audioSettings = Resources.Load<AudioSettings>("AudioSettings");
        if (audioSettings == null)
        {
            Debug.LogError("AudioSettings asset bulunamadı! Assets/Resources/AudioSettings.asset dosyasını oluşturun.");
            return;
        }

        audioEventDict.Clear();
        foreach (var data in audioSettings.audioEvents)
        {
            if (System.Enum.TryParse<AudioEvents>(data.eventName, out var evt))
            {
                audioEventDict[evt] = data;
            }
        }

        rootObject = new GameObject("[AudioManager]");
        Object.DontDestroyOnLoad(rootObject);
    }

    public static void Play(AudioEvents evt)
    {
        Init();

        if (!audioEventDict.TryGetValue(evt, out var data) || data.clip == null)
        {
            Debug.LogWarning($"Ses bulunamadı: {evt}");
            return;
        }

        if (playingSources.ContainsKey(evt) && playingSources[evt] != null)
        {
            if (playingSources[evt].isPlaying) return;
        }

        var source = rootObject.AddComponent<AudioSource>();
        source.clip = data.clip;
        source.loop = data.loop;
        source.volume = data.volume;
        source.Play();

        playingSources[evt] = source;

        if (!data.loop)
        {
            DelayedDestroy(source, data.clip.length, evt);
        }
    }

    private static async void DelayedDestroy(AudioSource source, float delay, AudioEvents evt)
    {
        await System.Threading.Tasks.Task.Delay((int)(delay * 1000));
        if (source != null)
        {
            Object.Destroy(source);
            playingSources.Remove(evt);
        }
    }

    public static void Stop(AudioEvents evt)
    {
        Init();

        if (playingSources.TryGetValue(evt, out var source) && source != null)
        {
            source.Stop();
            Object.Destroy(source);
            playingSources.Remove(evt);
        }
    }

    public static void StopAll()
    {
        Init();

        foreach (var source in playingSources.Values)
        {
            if (source != null)
            {
                source.Stop();
                Object.Destroy(source);
            }
        }
        playingSources.Clear();
    }
}
