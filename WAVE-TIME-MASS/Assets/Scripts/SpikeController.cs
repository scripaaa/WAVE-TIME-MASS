using UnityEngine;

public class SpikeController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, что столкнулись с игроком
        if (collision.CompareTag("Player"))
        {
            // Вызываем метод смерти у игрока
            Hero player = collision.GetComponent<Hero>();
            if (player != null)
            {
                player.Die();
            }
        }
    }
}