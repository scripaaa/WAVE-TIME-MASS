using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class SawUnable : Entity
{
    public GameObject deathMenu;

    private void OnTriggerEnter2D(Collider2D other) // проверка на столкновение
    {
        if (other.gameObject.CompareTag("Player"))
        {
            deathMenu.SetActive(true);
        }
    }
}
