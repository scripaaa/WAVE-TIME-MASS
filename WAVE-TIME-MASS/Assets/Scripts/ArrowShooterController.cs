using UnityEngine;

public class ArrowShooterController : MonoBehaviour
{
    public GameObject arrowPrefab; // Префаб стрелы
    public float spawnInterval = 2f; // Интервал между выстрелами

    private float timer;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        // Используем Time.unscaledDeltaTime, чтобы интервал между выстрелами не зависел от Time.timeScale
        timer -= Time.unscaledDeltaTime;

        if (timer <= 0)
        {
            SpawnArrow();
            timer = spawnInterval;
        }
    }

    void SpawnArrow()
    {
        // Создаем стрелу на позиции спавнера
        Instantiate(arrowPrefab, transform.position, Quaternion.identity);
    }
}