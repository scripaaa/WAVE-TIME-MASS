using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class SawUnable : Entity
{
    public GameObject deathMenu;

    private void OnTriggerEnter2D(Collider2D other) // �������� �� ������������
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // �������� ��������� Hero � ������
            Hero hero = other.gameObject.GetComponent<Hero>();

            if (hero != null)
            {
                // �������� ����� Die() � �����
                hero.Die();
            }
            else
            {
                deathMenu.SetActive(true);
            }
        }
    }
}
