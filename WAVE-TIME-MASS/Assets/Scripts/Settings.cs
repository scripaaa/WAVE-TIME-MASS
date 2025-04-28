using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public GameObject settingsMenu;
    public GameObject HUD;

    [SerializeField] private AudioSource audioScr;
    [SerializeField] private Slider sliderVolume;
    float musicVolume = 1f;

    Resolution[] resolutions;
    void Start()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRateRatio + "hz");
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height ==  Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        LoadSetting(currentResolutionIndex);
    }

    // Полноэкранный режим
    public void SetFullScreen(bool IsFullScreen)
    {
        Screen.fullScreen = IsFullScreen;
    }
    
    // Установка разрешения
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void Exit()
    {
        settingsMenu.SetActive(false);
        HUD.SetActive(true);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("resolutionScreenPreference", resolutionDropdown.value);
        PlayerPrefs.SetInt("fullScreenPreference", System.Convert.ToInt32(Screen.fullScreen));
    }

    public void LoadSetting(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("resolutionScreenPreference"))
        {
            resolutionDropdown.value = PlayerPrefs.GetInt("resolutionScreenPreference");
        }
        else
        {
            resolutionDropdown.value = currentResolutionIndex;
        }
        if (PlayerPrefs.HasKey("fullScreenPreference"))
        {
            Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("fullScreenPreference"));
        }
        else
        {
            Screen.fullScreen = true;
        }
    }
    private void Update()
    {
        audioScr.volume = musicVolume;
    }
    public void SetVolume(float vol)
    {
        musicVolume = vol;
    }

    public void ChangeVolume()
    {
        AudioListener.volume = sliderVolume.value;
    }

}
