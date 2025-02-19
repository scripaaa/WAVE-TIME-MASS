using UnityEngine;

public class SpikeController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���������, ��� ����������� � �������
        if (collision.CompareTag("Player"))
        {
            // �������� ����� ������ � ������
            Hero player = collision.GetComponent<Hero>();
            if (player != null)
            {
                player.Die();
            }
        }
    }
}