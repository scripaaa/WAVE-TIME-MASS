using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public GameObject panel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Player") && (SceneManager.GetActiveScene().buildIndex < 3))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            panel.SetActive(true); // Отображаем финальный экран
            TimeManager.FreezeTime(); // Замораживает игру
        }

        Destroy(gameObject);
    }
}


