using UnityEngine;

public class GoliathBoss : MonoBehaviour
{
    public float groundPoundInterval = 3f; // Интервал между ударами
    public float rockThrowInterval = 2f;   // Интервал между бросками камней
    public GameObject rockPrefab;         // Префаб камня
    public Transform rockSpawnPoint;      // Точка появления камня
    public GameObject shockwavePrefab;    // Префаб ударной волны

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
        //animator.SetTrigger("GroundPound"); // Запускаем анимацию удара
        Instantiate(shockwavePrefab, transform.position, Quaternion.identity); // Создаем ударную волну
    }

    private void ThrowRock()
    {
        //animator.SetTrigger("ThrowRock"); // Запускаем анимацию броска
        Instantiate(rockPrefab, rockSpawnPoint.position, Quaternion.identity); // Создаем камень
    }
}