using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Range(0f, 1f)]
    public float volume = 1f;

    [Tooltip("Optional: assign the UI Slider to sync value and receive events automatically.")]
    public Slider volumeSlider;

    const string PrefKey = "masterVolume";

    void Awake()
    {
        // simple singleton so volume persists across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // load saved volume (default 1.0)
        float saved = PlayerPrefs.GetFloat(PrefKey, 1f);
        ApplyVolume(saved);

        // if a slider is assigned, sync and subscribe
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value = saved;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    // Public method you can call from a Slider's OnValueChanged event
    public void SetVolume(float v)
    {
        ApplyVolume(v);
        PlayerPrefs.SetFloat(PrefKey, volume);
    }

    void ApplyVolume(float v)
    {
        volume = Mathf.Clamp01(v);
        AudioListener.volume = volume; // global volume
    }

    void OnDestroy()
    {
        if (volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(SetVolume);
    }
}
