using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public GameObject textTheEnd;
    private Hero playerController;

    void Start()
    {
        // Ищем игрока
        playerController = FindObjectOfType<Hero>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Player") && (SceneManager.GetActiveScene().buildIndex < 2))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            textTheEnd.SetActive(true); // Отображаем финальный экран
            playerController.canControl = false; // Блокируем управление
        }

        Destroy(gameObject);
    }
}
