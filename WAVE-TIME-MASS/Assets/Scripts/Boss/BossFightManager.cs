using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossFightManager : MonoBehaviour
{
    public GameObject leftDoor;  // Левая дверь
    public GameObject rightDoor; // Правая дверь
    public Text bossText;        // Текст "Бой с боссом"
    public GameObject boss;      // Объект босса

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Триггер сработал!"); // Добавьте эту строку

        if (collision.CompareTag("Player")) // Проверяем, что это игрок
        {
            StartCoroutine(StartBossFight()); // Запускаем битву
        }
    }

    private IEnumerator StartBossFight()
    {
        // Закрываем двери
        leftDoor.SetActive(true);
        rightDoor.SetActive(true);

        // Показываем надпись "Бой с боссом"
        bossText.text = "Бой с боссом";
        bossText.gameObject.SetActive(true);

        // Ждем 5 секунд
        yield return new WaitForSeconds(5f);

        // Скрываем надпись
        bossText.gameObject.SetActive(false);

        // Активируем босса
        boss.SetActive(true);
    }
}