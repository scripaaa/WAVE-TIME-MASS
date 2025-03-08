using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    public GameObject deathMenu;
    public Image fadeImage; // ссылка на фото
    public float fadeDuration = 2.0f; // длительность
    private void Update()
    {
        if (deathMenu.activeSelf)
        {
            TimeManager.FreezeTime();
            StartCoroutine(FadeToBlack());
        }
        else
        {
            TimeManager.UnfreezeTime();
        }
    }
    // затемнение
    IEnumerator FadeToBlack()
    {
        float counter = 0;
        Color spriteColor = fadeImage.color;

        while (counter < fadeDuration)
        {
            counter += Time.unscaledDeltaTime;
            fadeImage.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, Mathf.Clamp01(counter / fadeDuration));
            yield return null;
        }
    }

    public void Restart()
    {
        TimeManager.UnfreezeTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MainMenu()
    {
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex); // Сохраниение сцены при выходе
        SceneManager.LoadScene(0);
        TimeManager.UnfreezeTime();
    }

    public void Exit()
    {
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex); // Сохраниение сцены при выходе
        Application.Quit();
    }
}
