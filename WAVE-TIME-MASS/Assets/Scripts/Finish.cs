using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public GameObject panel; // Финальный экран
    public LevelStats levelStats; // Ссылка на LevelStats

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (SceneManager.GetActiveScene().buildIndex < 3)
            {
                TimeManager.FreezeTime(); // Замораживаем игру
                levelStats.CompleteLevel(); // Показываем статистику
                // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Переход на следующий уровень
            }
            else
            {
                panel.SetActive(true); // Отображаем финальный экран
                TimeManager.FreezeTime(); // Замораживаем игру
            }

            Destroy(gameObject);
        }
    }
}