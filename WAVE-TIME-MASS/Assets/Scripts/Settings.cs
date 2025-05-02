using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [Header("UI References")]
    public Dropdown resolutionDropdown;
    public GameObject settingsMenu;
    public GameObject HUD;
    [SerializeField] private AudioSource audioScr;
    [SerializeField] private Slider sliderVolume;

    private Resolution[] resolutions;
    private bool isInitialized = false;

    void Awake()
    {
        // Загружаем настройки сразу при старте
        LoadAllSettings();

        // Если это главное меню, делаем объект постоянным
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        InitializeUIComponents();
        isInitialized = true;
    }

    void LoadAllSettings()
    {
        // Загружаем громкость (даже если меню не активно)
        float savedVolume = PlayerPrefs.GetFloat("volumePreference", 1f);
        ApplyVolumeSettings(savedVolume);

        // Применяем разрешение экрана
        Resolution currentResolution = Screen.currentResolution;
        int savedResolutionIndex = PlayerPrefs.GetInt("resolutionScreenPreference", -1);
        if (savedResolutionIndex != -1 && savedResolutionIndex < Screen.resolutions.Length)
        {
            Resolution savedRes = Screen.resolutions[savedResolutionIndex];
            Screen.SetResolution(savedRes.width, savedRes.height, Screen.fullScreen);
        }

        // Применяем полноэкранный режим
        bool fullscreen = PlayerPrefs.GetInt("fullScreenPreference", 1) == 1;
        Screen.fullScreen = fullscreen;
    }

    void InitializeUIComponents()
    {
        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();
            resolutions = Screen.resolutions;

            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                options.Add($"{resolutions[i].width}x{resolutions[i].height} {resolutions[i].refreshRateRatio}Hz");
                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = PlayerPrefs.GetInt("resolutionScreenPreference", currentResolutionIndex);
            resolutionDropdown.RefreshShownValue();
        }

        if (sliderVolume != null)
        {
            sliderVolume.onValueChanged.RemoveAllListeners();
            sliderVolume.value = PlayerPrefs.GetFloat("volumePreference", 1f);
            sliderVolume.onValueChanged.AddListener(SetVolume);
        }
    }

    public void SetVolume(float volume)
    {
        if (!isInitialized) return;

        ApplyVolumeSettings(volume);
        PlayerPrefs.SetFloat("volumePreference", volume);
    }

    void ApplyVolumeSettings(float volume)
    {
        // Устанавливаем общую громкость
        AudioListener.volume = volume;

        // Устанавливаем громкость для музыки (если источник есть)
        if (audioScr != null)
        {
            audioScr.volume = volume;
        }
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("fullScreenPreference", isFullScreen ? 1 : 0);
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex < 0 || resolutionIndex >= resolutions.Length) return;

        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolutionScreenPreference", resolutionIndex);
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
        HUD.SetActive(false);

        // Обновляем значения UI при открытии
        if (sliderVolume != null)
        {
            sliderVolume.value = PlayerPrefs.GetFloat("volumePreference", 1f);
        }
    }

    public void ExitSettings()
    {
        settingsMenu.SetActive(false);
        HUD.SetActive(true);
        PlayerPrefs.Save(); // Явно сохраняем настройки
    }

    public void SaveSettings()
    {
        PlayerPrefs.Save();
        Debug.Log("Settings saved: " +
                 $"Volume={PlayerPrefs.GetFloat("volumePreference")}, " +
                 $"Resolution={PlayerPrefs.GetInt("resolutionScreenPreference")}, " +
                 $"Fullscreen={PlayerPrefs.GetInt("fullScreenPreference")}");
    }
}