using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction = Vector2.left;
    public float damageCooldown = 0.5f; // Задержка между попаданиями

    private Rigidbody2D rb;
    private float lastDamageTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastDamageTime = -damageCooldown; // Чтобы можно было сразу нанести урон
    }

    void Update()
    {
        rb.velocity = direction * speed * Time.timeScale;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Hero hero = collision.GetComponent<Hero>();
            if (hero != null && Time.time > lastDamageTime + damageCooldown)
            {
                hero.GetDamageHero();
                lastDamageTime = Time.time;
                Destroy(gameObject);
            }
        }
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}