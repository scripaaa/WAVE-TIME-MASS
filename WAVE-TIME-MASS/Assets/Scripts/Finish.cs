using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public LevelStats levelStats; // ������ �� LevelStats
    public bool FinishDone= false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (FinishDone) return;
        if (collision.tag == "Player")
        {
            FinishDone = true;
            Hero hero = collision.GetComponent<Hero>();

            if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                hero.FinishAnim();

            }
            else
            {
                FinishStats();
            }
            
        }
    }
    public void FinishStats()
    {
        TimeManager.FreezeTime(); // ������������ ����
        levelStats.CompleteLevel(); // ���������� ����������
        FinishDone = false;

        Destroy(gameObject);
    }
}