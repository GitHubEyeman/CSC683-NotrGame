using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;

        [HideInInspector]
        public AudioSource source;
    }

    public static AudioManager Instance;

    [Header("Sounds")]
    public Sound[] sounds;

    private void Awake()
    {
        // Singleton protection
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeAudioSources();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeAudioSources();
    }

    private void InitializeAudioSources()
    {
        foreach (Sound s in sounds)
        {
            if (s.clip == null)
            {
                Debug.LogWarning($"AudioManager: Clip missing for sound '{s.name}'");
                continue;
            }

            if (s.source == null)
            {
                s.source = gameObject.AddComponent<AudioSource>();
            }

            s.source.clip = s.clip;
            s.source.playOnAwake = false;
            s.source.loop = false;
        }
    }

    public void Play(string soundName)
    {
        Sound s = GetSound(soundName);
        if (s == null || s.source == null)
            return;

        s.source.Stop(); // prevents overlapping bug
        s.source.Play();
    }

    public void Stop(string soundName)
    {
        Sound s = GetSound(soundName);
        if (s == null || s.source == null)
            return;

        s.source.Stop();
    }

    public void Play(int index)
    {
        if (index < 0 || index >= sounds.Length)
            return;

        if (sounds[index].source == null)
            return;

        sounds[index].source.Stop();
        sounds[index].source.Play();
    }

    private Sound GetSound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
                return s;
        }

        Debug.LogWarning("Sound not found: " + name);
        return null;
    }
}
