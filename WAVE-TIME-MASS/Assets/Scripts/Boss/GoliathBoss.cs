using UnityEngine;

public class GoliathBoss : MonoBehaviour
{
    public float groundPoundInterval = 3f; // �������� ����� �������
    public float rockThrowInterval = 2f;   // �������� ����� �������� ������
    public GameObject rockPrefab;         // ������ �����
    public Transform rockSpawnPoint;      // ����� ��������� �����
    public GameObject shockwavePrefab;    // ������ ������� �����

    private Animator animator;
    private float nextGroundPoundTime;
    private float nextRockThrowTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        nextGroundPoundTime = Time.time + groundPoundInterval;
        nextRockThrowTime = Time.time + rockThrowInterval;
    }

    void Update()
    {
        if (Time.time >= nextGroundPoundTime)
        {
            GroundPound();
            nextGroundPoundTime = Time.time + groundPoundInterval;
        }

        if (Time.time >= nextRockThrowTime)
        {
            ThrowRock();
            nextRockThrowTime = Time.time + rockThrowInterval;
        }
    }

    private void GroundPound()
    {
        //animator.SetTrigger("GroundPound"); // ��������� �������� �����
        Instantiate(shockwavePrefab, transform.position, Quaternion.identity); // ������� ������� �����
    }

    private void ThrowRock()
    {
        //animator.SetTrigger("ThrowRock"); // ��������� �������� ������
        Instantiate(rockPrefab, rockSpawnPoint.position, Quaternion.identity); // ������� ������
    }
}