using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject controls_panel;
    GameObject finish;

    private Slowdown slowdown; // Ссылка на Slowdown

    void Start()
    {
        slowdown = GameObject.FindGameObjectWithTag("Player").GetComponent<Slowdown>(); // Находим скрипт Slowdown на объекте игрока
        TimeManager.ResetFreezeCount(); // Сбросить все заморозки
    }

    public void Continue() 
    {
        pauseMenu.SetActive(false);

        MusicManager musicManager = FindObjectOfType<MusicManager>();
        musicManager.SetPause(false); // Выключить паузу

        TimeManager.UnfreezeTime();
    }

    public void MainMenu() 
    {
        // Очищаем инвентарь перед рестартом
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.ClearInventory();
        }

        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex); // Сохраниение сцены при выходе
        SceneManager.LoadScene(0);
        TimeManager.UnfreezeTime(); // Размораживаем игру

        // Очищаем инвентарь перед рестартом
        if (slowdown != null)
        {
            slowdown.NotActivateSlowdown();
        }
    }

    public void NewGame() 
    {
        // Очищаем инвентарь перед рестартом
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.ClearInventory();
        }

        SceneManager.LoadScene(1);
        TimeManager.UnfreezeTime(); // Размораживаем игру

        slowdown.NotActivateSlowdown();
    }

    public void Quit() 
    {
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.SaveInventory(); // Сохраняем инвентарь перед выходом
        }

        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex); // Сохраниение сцены при выходе
        Application.Quit();
    }

    public void Controls()
    {
        controls_panel.SetActive(true); // Отображаем управление
    }

    public void Close_Сontrols()
    {
        controls_panel.SetActive(false); // Скрываем управление
    }

    void Update()
    {
        GameObject finish = GameObject.FindWithTag("Finish"); // Проверяем есть ли кубок на уровне

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (!pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(true);

                MusicManager musicManager = FindObjectOfType<MusicManager>();
                musicManager.SetPause(true); // Включить паузу

                TimeManager.FreezeTime(); // Замораживаем игру
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
                    musicManager.SetPause(false); // Выключить паузу

                    TimeManager.UnfreezeTime(); // Размораживаем игру
                }
                
            }
   
            
        }
    }
}
