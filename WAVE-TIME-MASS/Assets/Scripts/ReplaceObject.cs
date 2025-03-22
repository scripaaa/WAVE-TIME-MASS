using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplaceObject : MonoBehaviour
{
    public GameObject PresentObject; // Объект в настоящем
    public GameObject PastObject; // Объект в прошлом
    public GameObject FutureObject; // Объект в будущем

    private KeyCode replaceKey_Future = KeyCode.E; // Клавиша для перехода в будущее
    private KeyCode replaceKey_Present = KeyCode.W; // Клавиша для перехода в настоящее
    private KeyCode replaceKey_Past = KeyCode.Q; // Клавиша для перехода в прошлое

    public GameObject pause_menu; // Меню паузы

    private float lastSwitchTime = -10f; // Время последнего переключения
    private float cooldownDuration = 5f; // Длительность кулдауна (5 секунд)
    public Image cooldownIndicator; // UI Image для отображения кулдауна
    public Text cooldownText; // UI Text для отображения статуса перезарядки

    private Times currentTime = Times.present; // Текущее время (по умолчанию - настоящее)

    private void Start()
    {
        PastObject.SetActive(false);
        FutureObject.SetActive(false);

        // Инициализация текста
        if (cooldownText != null)
        {
            cooldownText.text = "Можно путешествовать";
        }
    }

    private void Update()
    {
        if (!pause_menu.activeSelf)
        {
            // Обновляем индикатор кулдауна
            if (cooldownIndicator != null)
            {
                float cooldownProgress = Mathf.Clamp01((Time.time - lastSwitchTime) / cooldownDuration);
                cooldownIndicator.fillAmount = cooldownProgress;
            }

            // Обновляем текст в зависимости от состояния кулдауна
            if (cooldownText != null)
            {
                if (Time.time - lastSwitchTime >= cooldownDuration)
                {
                    cooldownText.text = "Можно путешествовать";
                }
                else
                {
                    cooldownText.text = "Немного подождите";
                }
            }

            // Остальная логика переключения
            if (Input.GetKeyDown(replaceKey_Future))
            {
                TrySwitchTime(Times.future);
            }
            else if (Input.GetKeyDown(replaceKey_Present))
            {
                TrySwitchTime(Times.present);
            }
            else if (Input.GetKeyDown(replaceKey_Past))
            {
                TrySwitchTime(Times.past);
            }
        }
    }

    void TrySwitchTime(Times newTime)
    {
        // Если игрок пытается переключиться в то же время, ничего не делаем
        if (newTime == currentTime)
        {
            Debug.Log("Вы уже в этом времени!");
            return;
        }

        // Проверяем, прошло ли достаточно времени с момента последнего переключения
        if (Time.time - lastSwitchTime >= cooldownDuration)
        {
            SwitchTime(newTime);
            lastSwitchTime = Time.time; // Обновляем время последнего переключения
            currentTime = newTime; // Обновляем текущее время
        }
        else
        {
            Debug.Log("Кулдаун еще не закончился!");
        }
    }

    void SwitchTime(Times newTime)
    {
        // Выключаем все объекты
        if (PresentObject != null) PresentObject.SetActive(false);
        if (PastObject != null) PastObject.SetActive(false);
        if (FutureObject != null) FutureObject.SetActive(false);

        // Включаем нужный объект в зависимости от выбранного времени
        switch (newTime)
        {
            case Times.present:
                if (PresentObject != null) PresentObject.SetActive(true);
                break;
            case Times.past:
                if (PastObject != null) PastObject.SetActive(true);
                break;
            case Times.future:
                if (FutureObject != null) FutureObject.SetActive(true);
                break;
        }
    }
}

public enum Times
{
    present,
    past,
    future
}