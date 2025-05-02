using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public LevelStats levelStats; // Ссылка на LevelStats
    public bool FinishDone= false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (FinishDone) return;
        if (collision.tag == "Player")
        {
            FinishDone = true;
            Hero hero = collision.GetComponent<Hero>();
            hero.FinishAnim();
            
        }
    }
    public void FinishStats()
    {
        TimeManager.FreezeTime(); // Замораживаем игру
        levelStats.CompleteLevel(); // Показываем статистику
        FinishDone = false;

        Destroy(gameObject);
    }
}