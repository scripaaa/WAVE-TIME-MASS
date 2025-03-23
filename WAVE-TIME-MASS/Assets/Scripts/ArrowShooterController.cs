using UnityEngine;

public class ArrowShooterController : MonoBehaviour
{
    public GameObject arrowPrefab; // ������ ������
    public float spawnInterval = 2f; // �������� ����� ����������

    private float timer;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        // ���������� Time.unscaledDeltaTime, ����� �������� ����� ���������� �� ������� �� Time.timeScale
        timer -= Time.unscaledDeltaTime;

        if (timer <= 0)
        {
            SpawnArrow();
            timer = spawnInterval;
        }
    }

    void SpawnArrow()
    {
        // ������� ������ �� ������� ��������
        Instantiate(arrowPrefab, transform.position, Quaternion.identity);
    }
}