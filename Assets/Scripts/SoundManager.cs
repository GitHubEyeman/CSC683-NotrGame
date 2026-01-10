using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("-------------Audio Sources-------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("-------------Audio Clips-------------")]
    public AudioClip GamebackgroundMusic;
    public AudioClip MenubackgroundMusic;
    public AudioClip ShootkSFX;
    public AudioClip BoomSFX;
    public AudioClip HitSFX;
    public AudioClip JumpSFX;
    public AudioClip BlasterSFX;
    public AudioClip PowerUpSFX;

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}