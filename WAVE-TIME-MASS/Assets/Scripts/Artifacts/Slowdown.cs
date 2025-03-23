using System.Collections.Generic;
using UnityEngine;

public class Slowdown : MonoBehaviour
{
    public float slowdownFactor = 0.5f; // «амедление времени в 2 раза
    private static bool isTimeSlowed = false;
    public float playerSpeedMultiplier = 2f; // ћножитель скорости дл€ геро€

    [SerializeField] private ArrowShooterController arrowShooterController_present; // ћетатель стрелами в насто€щем
    [SerializeField] private ArrowShooterController arrowShooterController_past; // ћетатель стрелами в пошлом

    void Update()
    {
        // ≈сли врем€ заморожено через TimeManager, игнорируем замедление
        if (TimeManager.IsTimeFrozen())
        {
            return;
        }

        // ѕримен€ем замедление времени, если оно активировано
        if (isTimeSlowed)
        {
            Time.timeScale = slowdownFactor;
            Time.fixedDeltaTime = 0.02f * Time.timeScale; //  орректируем фиксированное врем€ дл€ физики
        }
        else
        {
            Time.timeScale = 1f; // ¬осстанавливаем нормальную скорость времени
            Time.fixedDeltaTime = 0.02f; // ¬осстанавливаем фиксированное врем€ дл€ физики
        }
    }

    // ћетод дл€ активации замедлени€ времени
    public void ActivateSlowdown()
    {
        if (arrowShooterController_present != null)
        {
            arrowShooterController_present.spawnInterval *= 2; // ”величиваем интервал между выстрелами метател€
            arrowShooterController_past.spawnInterval *= 2; // ”величиваем интервал между выстрелами метател€
        }

        isTimeSlowed = true;
    }

    // ћетод дл€ деактивации замедлени€ времени
    public void NotActivateSlowdown()
    {
        if (arrowShooterController_present != null)
        {
            arrowShooterController_present.spawnInterval /= 2; // ”меньшаем интервал между выстрелами метател€
            arrowShooterController_past.spawnInterval /= 2; // ”меньшаем интервал между выстрелами метател€
        }
    
        isTimeSlowed = false;
    }

    // ћетод дл€ проверки активировано ли замедление времени
    public bool IsTimeSlowed()
    {
        return isTimeSlowed;
    }
}