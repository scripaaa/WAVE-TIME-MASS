using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public GameObject panel; // ��������� �����
    public LevelStats levelStats; // ������ �� LevelStats

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (SceneManager.GetActiveScene().buildIndex < 3)
            {
                TimeManager.FreezeTime(); // ������������ ����
                levelStats.CompleteLevel(); // ���������� ����������
                // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // ������� �� ��������� �������
            }
            else
            {
                panel.SetActive(true); // ���������� ��������� �����
                TimeManager.FreezeTime(); // ������������ ����
            }

            Destroy(gameObject);
        }
    }
}