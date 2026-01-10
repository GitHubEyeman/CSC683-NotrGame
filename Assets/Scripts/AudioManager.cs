using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;          // Name used to play the sound
        public AudioSource source;   // AudioSource assigned in Inspector
    }

    public static AudioManager Instance;

    [Header("Sounds")]
    public Sound[] sounds; // Assign AudioSources here

    private void Awake()
    {
        // Singleton pattern (optional but recommended)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Play a sound by name
    /// </summary>
    public void Play(string soundName)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == soundName)
            {
                s.source.Play();
                return;
            }
        }

        Debug.LogWarning("Sound not found: " + soundName);
    }

    /// <summary>
    /// Stop a sound by name
    /// </summary>
    public void Stop(string soundName)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == soundName)
            {
                s.source.Stop();
                return;
            }
        }
    }

    /// <summary>
    /// Play a sound by array index
    /// </summary>
    public void Play(int index)
    {
        if (index < 0 || index >= sounds.Length)
        {
            Debug.LogWarning("Sound index out of range: " + index);
            return;
        }

        sounds[index].source.Play();
    }
}
