using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    public GameObject deathMenu;
    public Image fadeImage; // ссылка на фото

    public GameObject HUD1;
    public GameObject HUD2;

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
            if (HUD1 != null)
                HUD1.SetActive(false);

            if (HUD2 != null)
                HUD2.SetActive(false);

            if (fadeCoroutine == null)
            {
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
        if (HUD2 != null)
            HUD2.SetActive(true);

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Перезагрузить сцену

        TimeManager.UnfreezeTime(); // Сбросить заморозку времени

        if (slowdown != null)
            slowdown.NotActivateSlowdown();
    }

    public void MainMenu()
    {
        TimeManager.UnfreezeTime(); // Сбросить заморозку времени

        if (HUD2 != null)
            HUD2.SetActive(true);

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
        
        SceneManager.LoadScene(0); // Загрузить главное меню

        slowdown.NotActivateSlowdown();
    }

    public void Exit()
    {
        TimeManager.UnfreezeTime(); // Сбросить заморозку времени

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