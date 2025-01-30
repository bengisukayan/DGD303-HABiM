using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music Tracks")]
    public AudioClip backgroundMusic;
    public AudioClip[] waveMusic;

    [Header("Sound Effects")]
    public AudioClip monsterDeathSFX;
    public AudioClip respawnSFX;
    public AudioClip shootingSFX;
    public AudioClip checkpointSFX;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        PlayMusic(backgroundMusic);
    }

    public void PlayWaveMusic(int waveIndex)
    {
        if (waveIndex >= 0 && waveIndex < waveMusic.Length)
        {
            PlayMusic(waveMusic[waveIndex]);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMonsterDeath()
    {
        PlaySFX(monsterDeathSFX);
    }
    
    public void PlayRespawn()
    {
        PlaySFX(respawnSFX);
    }

    public void PlayCheckpoint()
    {
        PlaySFX(checkpointSFX);
    }

    public void PlayShooting()
    {
        PlaySFX(shootingSFX);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();
    }
}