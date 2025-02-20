using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float speed = 2f; // �������� �������� ���������
    public float leftBound = -5f; // ����� ������� �������� (���������� X)
    public float rightBound = 5f; // ������ ������� �������� (���������� X)
    private bool movingRight = true; // ����������� ��������

    void Update()
    {
        // �������� ���������
        if (movingRight)
        {
            // ��������� ������
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            // ���� �������� ������ �������, ������ �����������
            if (transform.position.x >= rightBound)
            {
                movingRight = false;
            }
        }
        else
        {
            // ��������� �����
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            // ���� �������� ����� �������, ������ �����������
            if (transform.position.x <= leftBound)
            {
                movingRight = true;
            }
        }
    }
}