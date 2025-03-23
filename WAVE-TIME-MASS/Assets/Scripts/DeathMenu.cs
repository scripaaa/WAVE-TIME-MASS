using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    public GameObject deathMenu;
    public Image fadeImage; // ссылка на фото

    private float fadeDuration = 0.5f; // длительность
    private Coroutine fadeCoroutine; // ссылка на корутину
    private Slowdown slowdown; // Ссылка на Slowdown

    void Start()
    {
        slowdown = GameObject.FindGameObjectWithTag("Player").GetComponent<Slowdown>(); // Находим скрипт Slowdown на объекте игрока
    }

    private void Update()
    {
        if (deathMenu.activeSelf)
        {
            if (fadeCoroutine == null)
            {
                TimeManager.FreezeTime();
                fadeCoroutine = StartCoroutine(FadeToBlack());
            }
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
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        // Удаляем последний артефакт из инвентаря
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.RemoveArtifact(SceneManager.GetActiveScene().buildIndex - 2);
        }

        deathMenu.SetActive(false); // Скрыть меню смерти
        TimeManager.ResetFreezeCount(); // Сбросить заморозку времени
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Перезагрузить сцену

        slowdown.NotActivateSlowdown();
    }

    public void MainMenu()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        // Очищаем инвентарь перед рестартом
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.ClearInventory();
        }

        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex); // Сохраниение сцены при выходе
        TimeManager.ResetFreezeCount(); // Сбросить заморозку времени
        SceneManager.LoadScene(0); // Загрузить главное меню

        slowdown.NotActivateSlowdown();
    }

    public void Exit()
    {
        // Очищаем инвентарь перед рестартом
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.ClearInventory();
        }

        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex); // Сохраниение сцены при выходе
        Application.Quit();

        slowdown.NotActivateSlowdown();
    }
}