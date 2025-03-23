using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public float speed = 5f;      // Скорость движения
    public float lifeTime = 2f;   // Время жизни
    public int damage = 1;        // Урон игроку

    void Start()
    {
        Destroy(gameObject, lifeTime); // Уничтожаем объект через время
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime); // Двигаем волну
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Если столкнулись с игроком
        {
            // Получаем компонент Hero и вызываем метод GetDamageHero
            Hero hero = collision.GetComponent<Hero>();
            if (hero != null)
            {
                hero.GetDamageHero();
            }
        }
    }
}