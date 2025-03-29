using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossFightManager : MonoBehaviour
{
    public GameObject leftDoor;  // Левая дверь
    public GameObject rightDoor; // Правая дверь
    public Text bossText;        // Текст "Бой с боссом"
    public GameObject boss;      // Объект босса

    private Hero playerController;
    private bool hasTriggered = false; // Флаг для отслеживания срабатывания

    void Start()
    {
        playerController = FindObjectOfType<Hero>(); // Ищем игрока
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если триггер уже срабатывал или это не игрок - выходим
        if (hasTriggered || !collision.CompareTag("Player"))
            return;

        hasTriggered = true; // Помечаем триггер как сработавший
        StartCoroutine(StartBossFight());
    }

    private IEnumerator StartBossFight()
    {
        // Закрываем двери
        if (leftDoor != null) leftDoor.SetActive(true);
        if (rightDoor != null) rightDoor.SetActive(true);

        // Показываем надпись
        if (bossText != null)
        {
            bossText.text = "Boss fight!";
            bossText.gameObject.SetActive(true);
        }

        if (playerController != null)
        {
            playerController.AddHeart();
            playerController.AddHeart();
        }

        // Ждем 5 секунд
        yield return new WaitForSeconds(5f);

        // Скрываем надпись
        if (bossText != null) bossText.gameObject.SetActive(false);

        // Активируем босса
        if (boss != null) boss.SetActive(true);
    }

    // Опционально: метод для сброса триггера (если нужно перезапустить битву)
    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}