using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource mainMusicSource;
    [SerializeField] private AudioSource pauseMusicSource;

    [Header("Current Level Music")]
    public AudioClip mainTrack;
    public AudioClip pauseTrack;

    // Изменено с public field на property с вызовом UpdateVolume
    private float _volume = 1f;
    public float volume
    {
        get => _volume;
        set
        {
            _volume = Mathf.Clamp01(value);
            UpdateVolume();
        }
    }

    private bool isPaused = false;

    private void Awake()
    {
        // Настройка AudioSources
        mainMusicSource.loop = true;
        pauseMusicSource.loop = true;

        // Загружаем сохраненную громкость
        volume = PlayerPrefs.GetFloat("volumePreference", 0.7f);
    }

    private void Start()
    {
        PlayMusic();
    }

    void UpdateVolume()
    {
        mainMusicSource.volume = _volume;
        pauseMusicSource.volume = _volume;
    }

    public void PlayMusic()
    {
        mainMusicSource.clip = mainTrack;
        pauseMusicSource.clip = pauseTrack;

        UpdateVolume();

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