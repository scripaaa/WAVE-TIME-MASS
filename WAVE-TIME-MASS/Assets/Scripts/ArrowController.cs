using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float speed = 5f; // �������� ������
    public Vector2 direction = Vector2.left; // ����������� �������� ������

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ������� ������ � �������� �����������
        rb.velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // ���������, ����������� �� ������ � ������
        if (collision.CompareTag("Player"))
        {
            // �������� ��������� Hero � �������� ����� GetDamageHero
            Hero hero = collision.GetComponent<Hero>();
            if (hero != null)
            {
                hero.GetDamageHero();
            }

            // ���������� ������ ��� ������������
            Destroy(gameObject);
        }
        // ���������, ����������� �� ������ �� ������
        else if (collision.CompareTag("Ground"))
        {
            // ���������� ������ ��� ������������
            Destroy(gameObject);
        }
    }
}