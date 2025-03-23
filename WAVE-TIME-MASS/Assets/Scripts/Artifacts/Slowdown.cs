using System.Collections.Generic;
using UnityEngine;

public class Slowdown : MonoBehaviour
{
    public float slowdownFactor = 0.5f; // ���������� ������� � 2 ����
    private static bool isTimeSlowed = false;
    public float playerSpeedMultiplier = 2f; // ��������� �������� ��� �����

    [SerializeField] private ArrowShooterController arrowShooterController_present; // �������� �������� � ���������
    [SerializeField] private ArrowShooterController arrowShooterController_past; // �������� �������� � ������

    void Update()
    {
        // ���� ����� ���������� ����� TimeManager, ���������� ����������
        if (TimeManager.IsTimeFrozen())
        {
            return;
        }

        // ��������� ���������� �������, ���� ��� ������������
        if (isTimeSlowed)
        {
            Time.timeScale = slowdownFactor;
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // ������������ ������������� ����� ��� ������
        }
        else
        {
            Time.timeScale = 1f; // ��������������� ���������� �������� �������
            Time.fixedDeltaTime = 0.02f; // ��������������� ������������� ����� ��� ������
        }
    }

    // ����� ��� ��������� ���������� �������
    public void ActivateSlowdown()
    {
        if (arrowShooterController_present != null)
        {
            arrowShooterController_present.spawnInterval *= 2; // ����������� �������� ����� ���������� ��������
            arrowShooterController_past.spawnInterval *= 2; // ����������� �������� ����� ���������� ��������
        }

        isTimeSlowed = true;
    }

    // ����� ��� ����������� ���������� �������
    public void NotActivateSlowdown()
    {
        if (arrowShooterController_present != null)
        {
            arrowShooterController_present.spawnInterval /= 2; // ��������� �������� ����� ���������� ��������
            arrowShooterController_past.spawnInterval /= 2; // ��������� �������� ����� ���������� ��������
        }
    
        isTimeSlowed = false;
    }

    // ����� ��� �������� ������������ �� ���������� �������
    public bool IsTimeSlowed()
    {
        return isTimeSlowed;
    }
}