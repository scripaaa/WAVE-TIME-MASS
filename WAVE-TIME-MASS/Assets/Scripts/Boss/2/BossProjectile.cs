using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = transform.right * speed;
        Destroy(gameObject, 5f); // Автоуничтожение через 5 сек
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Нанесение урона игроку
            // collision.GetComponent<PlayerHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}