using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject controls_panel;
    GameObject finish;

    public void Continue() 
    {
        pauseMenu.SetActive(false);
        TimeManager.UnfreezeTime();
    }

    public void MainMenu() 
    {
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex); // ����������� ����� ��� ������
        SceneManager.LoadScene(0);
        TimeManager.UnfreezeTime(); // ������������� ����

        if (finish == null)
            TimeManager.UnfreezeTime(); // ������������� ���� ������ ��� ��-�� ����� ����

        TimeManager.ResetFreezeCount(); // �������� ��������� �������
    }

    public void NewGame() 
    {
        SceneManager.LoadScene(1);
        TimeManager.UnfreezeTime(); // ������������� ����

        if (finish == null)
            TimeManager.UnfreezeTime(); // ������������� ���� ������ ��� ��-�� ����� ����

        TimeManager.ResetFreezeCount(); // �������� ��������� �������
    }

    public void Quit() 
    {
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex); // ����������� ����� ��� ������
        Application.Quit();
    }

    public void Controls()
    {
        controls_panel.SetActive(true); // ���������� ����������
    }

    public void Close_�ontrols()
    {
        controls_panel.SetActive(false); // �������� ����������
    }

    void Update()
    {
        GameObject finish = GameObject.FindWithTag("Finish"); // ��������� ���� �� ����� �� ������

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (!pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(true);
                TimeManager.FreezeTime(); // ������������ ����
            }
            else
            {
                if (controls_panel.activeSelf)
                {
                    controls_panel.SetActive(false);
                }
                else
                {
                    pauseMenu.SetActive(false);
                    TimeManager.UnfreezeTime(); // ������������� ����
                    TimeManager.ResetFreezeCount(); // �������� ��������� �������
                }
                
            }
   
            
        }
    }
}
