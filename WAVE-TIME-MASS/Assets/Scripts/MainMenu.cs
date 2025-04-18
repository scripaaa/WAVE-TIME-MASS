using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        // ������� ��������� ����� ���������
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.ClearInventory();
            InventoryUI.Instance.SaveInventory(); // ��������� ������ ���������
        }

        Debug.Log("����� ����");
        TimeManager.ResetFreezeCount(); // �������� ��������� �������
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Continue()
    {
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.LoadInventory(PlayerPrefs.GetInt("SavedScene")); // ��������� ���������� ���������
        }

        TimeManager.ResetFreezeCount(); // �������� ��������� �������
        SceneManager.LoadScene(PlayerPrefs.GetInt("SavedScene"));
    }
    public void Exit()
    {
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.SaveInventory(); // ��������� ��������� ����� �������
        }

        Debug.Log("�������� ����");
        Application.Quit();
    }
}
