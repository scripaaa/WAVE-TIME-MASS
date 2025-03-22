using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashSpeed = 20f; // �������� �����
    public float dashDuration = 0.12f; // ������������ �����
    public float dashCooldown = 1f; // ����� ����������� �����

    private float lastDashTime = -10f; // ����� ���������� �����
    private bool isDashing = false; // ����, �����������, ��� ����� � �����
    private Vector2 dashDirection; // ����������� �����

    private Rigidbody2D rb; // ��������� Rigidbody2D ��� ���������� ���������

    private bool isFacingRight = true; // ����������� ������� ��������� (true - ������, false - �����)
    public bool canDash = false; // ��������� �� ������������ �����

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // �������� ��������� Rigidbody2D
    }

    private void Update()
    {
        // ��������� ����������� ������� ���������
        UpdateFacingDirection();

        // ���������, ������ �� ������� ��� ����� (��������, Shift)
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && !isDashing && Time.time - lastDashTime >= dashCooldown && canDash)
        {
            StartDash();
        }

        // ���� ����� � �����, ���������� ��������
        if (isDashing)
        {
            rb.velocity = dashDirection * dashSpeed;
        }
    }

    private void UpdateFacingDirection()
    {
        // ���������� ����������� ������� ���������
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0)
        {
            isFacingRight = true;
        }
        else if (moveInput < 0)
        {
            isFacingRight = false;
        }

        // ���� �������� ����� �� �����, ����������� ������� �� ��������
    }

    private void StartDash()
    {
        // ���������� ����������� �����
        if (isFacingRight)
        {
            dashDirection = Vector2.right; // ����� ������
        }
        else
        {
            dashDirection = Vector2.left; // ����� �����
        }

        // ��������� �����
        isDashing = true;
        lastDashTime = Time.time;

        // ������������� ����� ����� dashDuration ������
        Invoke("StopDash", dashDuration);
    }

    private void StopDash()
    {
        isDashing = false;
        rb.velocity = Vector2.zero; // ������������� �������� ����� �����
    }
}