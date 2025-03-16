using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        Debug.Log("����� ����");
        TimeManager.ResetFreezeCount(); // �������� ��������� �������
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Continue()
    {
        TimeManager.ResetFreezeCount(); // �������� ��������� �������
        SceneManager.LoadScene(PlayerPrefs.GetInt("SavedScene"));
    }
    public void Exit()
    {
        Debug.Log("�������� ����");
        Application.Quit();
    }
}
