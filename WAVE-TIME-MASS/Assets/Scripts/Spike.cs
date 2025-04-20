using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    public GameObject deathMenu;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Получаем компонент Hero у игрока
            Hero hero = other.gameObject.GetComponent<Hero>();

            if (hero != null)
            {
                // Вызываем метод Die() у героя
                hero.Die();
            }
            else
            {
                deathMenu.SetActive(true);
            }
        }
    }


}
