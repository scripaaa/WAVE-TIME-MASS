using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public LevelStats levelStats; // Ссылка на LevelStats

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TimeManager.FreezeTime(); // Замораживаем игру
            levelStats.CompleteLevel(); // Показываем статистику

            Destroy(gameObject);
        }
    }
}