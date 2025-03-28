using UnityEngine;

public class Projectile3 : MonoBehaviour
{
    public float speed = 10f;          // Скорость полёта
    public int damage = 1;             // Урон
    public float lifetime = 3f;        // Время жизни
    public bool moveRight = true;      // Летит вправо (или влево)

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);

        // Задаём начальную скорость
        float direction = moveRight ? 1 : -1;
        rb.velocity = new Vector2(direction * speed, 0);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Hero.Instance.GetDamageHero();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}