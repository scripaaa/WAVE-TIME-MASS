using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource mainMusicSource;
    [SerializeField] private AudioSource pauseMusicSource;

    [Header("Current Level Music")]
    public AudioClip mainTrack;
    public AudioClip pauseTrack;
    [Range(0f, 1f)] public float volume = 1f;

    private bool isPaused = false;

    private void Awake()
    {
        // Настройка AudioSources
        mainMusicSource.loop = true;
        pauseMusicSource.loop = true;
    }

    private void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        mainMusicSource.clip = mainTrack;
        pauseMusicSource.clip = pauseTrack;

        mainMusicSource.volume = volume;
        pauseMusicSource.volume = volume;

        if (isPaused)
        {
            mainMusicSource.Stop();
            pauseMusicSource.Play();
        }
        else
        {
            mainMusicSource.Play();
            pauseMusicSource.Stop();
        }
    }

    public void SetPause(bool paused)
    {
        isPaused = paused;

        if (paused)
        {
            mainMusicSource.Pause();
            pauseMusicSource.Play();
        }
        else
        {
            pauseMusicSource.Stop();
            mainMusicSource.UnPause();
        }
    }
}