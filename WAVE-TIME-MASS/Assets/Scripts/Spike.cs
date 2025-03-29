using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    public GameObject deathMenu;

    private void OnTriggerEnter2D(Collider2D other) // �������� �� ������������ 
    {
        // ������������ ����� ��� ������������ ������ � ����
        if (other.gameObject.CompareTag("Player"))
        {
            deathMenu.SetActive(true);
        }
    
    }


}
