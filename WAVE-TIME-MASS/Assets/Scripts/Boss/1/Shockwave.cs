using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public float speed = 5f;      // �������� ��������
    public float lifeTime = 2f;   // ����� �����
    public int damage = 1;        // ���� ������

    void Start()
    {
        Destroy(gameObject, lifeTime); // ���������� ������ ����� �����
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime); // ������� �����
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ���� ����������� � �������
        {
            // �������� ��������� Hero � �������� ����� GetDamageHero
            Hero hero = collision.GetComponent<Hero>();
            if (hero != null)
            {
                hero.GetDamageHero();
            }
        }
    }
}