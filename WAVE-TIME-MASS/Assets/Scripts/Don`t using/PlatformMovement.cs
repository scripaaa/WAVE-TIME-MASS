using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float speed = 2f; // Скорость движения платформы
    public float leftBound = -5f; // Левая граница движения (координата X)
    public float rightBound = 5f; // Правая граница движения (координата X)
    private bool movingRight = true; // Направление движения

    void Update()
    {
        // Движение платформы
        if (movingRight)
        {
            // Двигаемся вправо
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            // Если достигли правой границы, меняем направление
            if (transform.position.x >= rightBound)
            {
                movingRight = false;
            }
        }
        else
        {
            // Двигаемся влево
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            // Если достигли левой границы, меняем направление
            if (transform.position.x <= leftBound)
            {
                movingRight = true;
            }
        }
    }
}