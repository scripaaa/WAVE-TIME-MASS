using UnityEngine;

public class Projectile3 : MonoBehaviour
{
    public float speed = 10f;          // �������� �����
    public int damage = 1;             // ����
    public float lifetime = 3f;        // ����� �����
    public bool moveRight = true;      // ����� ������ (��� �����)

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);

        // ����� ��������� ��������
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