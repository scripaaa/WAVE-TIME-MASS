using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelStats : MonoBehaviour
{
    public int totalCoins; // Общее количество монет на уровне
    public int collectedCoins = 0; // Собранные монеты
    public float levelTime = 0f; // Время прохождения уровня
    public float targetTime; // Целевое время для получения 2 звезды

    public GameObject statsPanel; // Панель статистики
    public Text coinsText; // Текст для отображения монет
    public Text timeText; // Текст для отображения времени
    public Image[] stars; // Массив изображений звезд
    public Text timerText; // Текстовый элемент для отображения таймера
    public GameObject panel; // Финальный экран

    private bool levelCompleted = false;

    void Update()
    {
        if (!levelCompleted)
        {
            levelTime += Time.deltaTime; // Обновляем время
            UpdateTimerUI(); // Обновляем текстовый элемент таймера
        }
    }

    public void CollectCoin()
    {
        collectedCoins++;
    }

    public void CompleteLevel()
    {
        levelCompleted = true;
        ShowStats();
    }

    void ShowStats()
    {
        for (int i = 0; i < 3; i++)
        {
            if (stars[i] != null)
            {
                stars[i].color = Color.gray; 
            }
        }

        if (!statsPanel.activeSelf) // Проверяем, что панель еще не активна
        {
            statsPanel.SetActive(true); // Активируем панель статистики

            // Отображаем количество собранных монет
            if (coinsText != null)
            {
                coinsText.text = collectedCoins + " / " + totalCoins;
            }
            else
            {
                Debug.LogError("coinsText не назначен в инспекторе!");
            }

            // Отображаем время прохождения уровня
            if (timeText != null)
            {
                timeText.text = levelTime.ToString("F1") + "s";
            }
            else
            {
                Debug.LogError("timeText не назначен в инспекторе!");
            }

            // Рассчитываем количество звезд
            int starsEarned = 0;

            // 1 звезда за прохождение уровня
            starsEarned++;

            // 2 звезда за уложенное время
            if (levelTime <= targetTime)
            {
                starsEarned++;
            }

            // 3 звезда за все собранные монеты
            if (collectedCoins >= totalCoins)
            {
                starsEarned++;
            }

            // Активируем звезды
            for (int i = 0; i < starsEarned; i++)
            {
                if (stars[i] != null)
                {
                    stars[i].color = Color.yellow; // Подсвечиваем звезды
                }
            }
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            // Отображаем время в секундах с одной десятичной дробью
            timerText.text = levelTime.ToString("F1") + "с";
        }
        else
        {
            Debug.LogError("timerText не назначен в инспекторе!");
        }
    }

    public void RestartLevel()
    {
        // Удаляем последний артефакт из инвентаря
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.RemoveArtifact(SceneManager.GetActiveScene().buildIndex - 2);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        TimeManager.UnfreezeTime();
        TimeManager.ResetFreezeCount(); // Сбросить заморозку времени
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            statsPanel.SetActive(false);
            panel.SetActive(true);
            TimeManager.FreezeTime(); 
        }
        else
        {
            TimeManager.UnfreezeTime();
            TimeManager.ResetFreezeCount(); // Сбросить заморозку времени
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}