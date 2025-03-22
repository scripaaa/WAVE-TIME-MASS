using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplaceObject : MonoBehaviour
{
    public GameObject PresentObject; // ������ � ���������
    public GameObject PastObject; // ������ � �������
    public GameObject FutureObject; // ������ � �������

    private KeyCode replaceKey_Future = KeyCode.E; // ������� ��� �������� � �������
    private KeyCode replaceKey_Present = KeyCode.W; // ������� ��� �������� � ���������
    private KeyCode replaceKey_Past = KeyCode.Q; // ������� ��� �������� � �������

    public GameObject pause_menu; // ���� �����

    private float lastSwitchTime = -10f; // ����� ���������� ������������
    private float cooldownDuration = 5f; // ������������ �������� (5 ������)
    public Image cooldownIndicator; // UI Image ��� ����������� ��������
    public Text cooldownText; // UI Text ��� ����������� ������� �����������

    private Times currentTime = Times.present; // ������� ����� (�� ��������� - ���������)

    private void Start()
    {
        PastObject.SetActive(false);
        FutureObject.SetActive(false);

        // ������������� ������
        if (cooldownText != null)
        {
            cooldownText.text = "����� ��������������";
        }
    }

    private void Update()
    {
        if (!pause_menu.activeSelf)
        {
            // ��������� ��������� ��������
            if (cooldownIndicator != null)
            {
                float cooldownProgress = Mathf.Clamp01((Time.time - lastSwitchTime) / cooldownDuration);
                cooldownIndicator.fillAmount = cooldownProgress;
            }

            // ��������� ����� � ����������� �� ��������� ��������
            if (cooldownText != null)
            {
                if (Time.time - lastSwitchTime >= cooldownDuration)
                {
                    cooldownText.text = "����� ��������������";
                }
                else
                {
                    cooldownText.text = "������� ���������";
                }
            }

            // ��������� ������ ������������
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
        // ���� ����� �������� ������������� � �� �� �����, ������ �� ������
        if (newTime == currentTime)
        {
            Debug.Log("�� ��� � ���� �������!");
            return;
        }

        // ���������, ������ �� ���������� ������� � ������� ���������� ������������
        if (Time.time - lastSwitchTime >= cooldownDuration)
        {
            SwitchTime(newTime);
            lastSwitchTime = Time.time; // ��������� ����� ���������� ������������
            currentTime = newTime; // ��������� ������� �����
        }
        else
        {
            Debug.Log("������� ��� �� ����������!");
        }
    }

    void SwitchTime(Times newTime)
    {
        // ��������� ��� �������
        if (PresentObject != null) PresentObject.SetActive(false);
        if (PastObject != null) PastObject.SetActive(false);
        if (FutureObject != null) FutureObject.SetActive(false);

        // �������� ������ ������ � ����������� �� ���������� �������
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