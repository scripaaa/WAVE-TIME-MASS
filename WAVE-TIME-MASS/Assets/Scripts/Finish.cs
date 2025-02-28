using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public LevelStats levelStats; // ������ �� LevelStats

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TimeManager.FreezeTime(); // ������������ ����
            levelStats.CompleteLevel(); // ���������� ����������

            Destroy(gameObject);
        }
    }
}