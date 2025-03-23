using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashSpeed = 20f; // Скорость рывка
    public float dashDuration = 0.12f; // Длительность рывка
    public float dashCooldown = 1f; // Время перезарядки рывка

    private float lastDashTime = -10f; // Время последнего рывка
    private bool isDashing = false; // Флаг, указывающий, что игрок в рывке
    private Vector2 dashDirection; // Направление рывка

    private Rigidbody2D rb; // Компонент Rigidbody2D для управления движением

    private bool isFacingRight = true; // Направление взгляда персонажа (true - вправо, false - влево)
    public bool canDash = false; // Разрешено ли использовать рывок

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Получаем компонент Rigidbody2D
    }

    private void Update()
    {
        // Обновляем направление взгляда персонажа
        UpdateFacingDirection();

        // Проверяем, нажата ли клавиша для рывка (например, Shift)
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && !isDashing && Time.time - lastDashTime >= dashCooldown && canDash)
        {
            StartDash();
        }

        // Если игрок в рывке, продолжаем движение
        if (isDashing)
        {
            rb.velocity = dashDirection * dashSpeed;
        }
    }

    private void UpdateFacingDirection()
    {
        // Определяем направление взгляда персонажа
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0)
        {
            isFacingRight = true;
        }
        else if (moveInput < 0)
        {
            isFacingRight = false;
        }

        // Если персонаж стоит на месте, направление взгляда не меняется
    }

    private void StartDash()
    {
        // Определяем направление рывка
        if (isFacingRight)
        {
            dashDirection = Vector2.right; // Рывок вправо
        }
        else
        {
            dashDirection = Vector2.left; // Рывок влево
        }

        // Запускаем рывок
        isDashing = true;
        lastDashTime = Time.time;

        // Останавливаем рывок через dashDuration секунд
        Invoke("StopDash", dashDuration);
    }

    private void StopDash()
    {
        isDashing = false;
        rb.velocity = Vector2.zero; // Останавливаем движение после рывка
    }
}