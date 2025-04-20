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
