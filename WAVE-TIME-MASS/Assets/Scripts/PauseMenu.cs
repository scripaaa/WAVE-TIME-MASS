using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    GameObject finish; 

    public void Continue() 
    {
        pauseMenu.SetActive(false);
        TimeManager.UnfreezeTime();
    }

    public void MainMenu() 
    {
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex); // Сохраниение сцены при выходе
        SceneManager.LoadScene(0);
        TimeManager.UnfreezeTime(); // Размораживаем игру

        if (finish == null)
            TimeManager.UnfreezeTime(); // Размораживаем игру второй раз из-за конца игры
    }

    public void NewGame() 
    {
        SceneManager.LoadScene(1);
        TimeManager.UnfreezeTime(); // Размораживаем игру

        if (finish == null)
            TimeManager.UnfreezeTime(); // Размораживаем игру второй раз из-за конца игры
    }

    public void Quit() 
    {
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex); // Сохраниение сцены при выходе
        Application.Quit();
    }

    void Update()
    {
        GameObject finish = GameObject.FindWithTag("Finish"); // Проверяем есть ли кубок на уровне

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (!pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(true);
                TimeManager.FreezeTime(); // Замораживаем игру
            }
            else
            {
                pauseMenu.SetActive(false);
                TimeManager.UnfreezeTime(); // Размораживаем игру
            }
        }
    }
}
