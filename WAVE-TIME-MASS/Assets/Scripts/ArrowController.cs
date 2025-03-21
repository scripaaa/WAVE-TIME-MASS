using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float speed = 5f; // Скорость стрелы
    public Vector2 direction = Vector2.left; // Направление движения стрелы

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Двигаем стрелу в заданном направлении
        rb.velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, столкнулась ли стрела с героем
        if (collision.CompareTag("Player"))
        {
            // Получаем компонент Hero и вызываем метод GetDamageHero
            Hero hero = collision.GetComponent<Hero>();
            if (hero != null)
            {
                hero.GetDamageHero();
            }

            // Уничтожаем стрелу при столкновении
            Destroy(gameObject);
        }
        // Проверяем, столкнулась ли стрела со стеной
        else if (collision.CompareTag("Ground"))
        {
            // Уничтожаем стрелу при столкновении
            Destroy(gameObject);
        }
    }
}