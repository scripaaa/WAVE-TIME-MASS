using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject controls_panel;
    GameObject finish;

    private Slowdown slowdown; // ������ �� Slowdown

    void Start()
    {
        slowdown = GameObject.FindGameObjectWithTag("Player").GetComponent<Slowdown>(); // ������� ������ Slowdown �� ������� ������
        TimeManager.ResetFreezeCount(); // �������� ��� ���������
    }

    public void Continue() 
    {
        pauseMenu.SetActive(false);

        MusicManager musicManager = FindObjectOfType<MusicManager>();
        musicManager.SetPause(false); // ��������� �����

        TimeManager.UnfreezeTime();
    }

    public void MainMenu() 
    {
        // ������� ��������� ����� ���������
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.ClearInventory();
        }

        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex); // ����������� ����� ��� ������
        SceneManager.LoadScene(0);
        TimeManager.UnfreezeTime(); // ������������� ����

        // ������� ��������� ����� ���������
        if (slowdown != null)
        {
            slowdown.NotActivateSlowdown();
        }
    }

    public void NewGame() 
    {
        // ������� ��������� ����� ���������
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.ClearInventory();
        }

        SceneManager.LoadScene(1);
        TimeManager.UnfreezeTime(); // ������������� ����

        slowdown.NotActivateSlowdown();
    }

    public void Quit() 
    {
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.SaveInventory(); // ��������� ��������� ����� �������
        }

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

                MusicManager musicManager = FindObjectOfType<MusicManager>();
                musicManager.SetPause(true); // �������� �����

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

                    MusicManager musicManager = FindObjectOfType<MusicManager>();
                    musicManager.SetPause(false); // ��������� �����

                    TimeManager.UnfreezeTime(); // ������������� ����
                }
                
            }
   
            
        }
    }
}
