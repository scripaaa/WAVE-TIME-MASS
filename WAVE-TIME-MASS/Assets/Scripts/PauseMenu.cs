using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Continue() 
    {
        pauseMenu.SetActive(false);
        TimeManager.UnfreezeTime();
    }

    public void MainMenu() 
    {
        SceneManager.LoadScene(0);
        TimeManager.UnfreezeTime();
    }

    public void NewGame() 
    {
        SceneManager.LoadScene(1);
        TimeManager.UnfreezeTime(); // Размораживаем игру
    }

    public void Quit() 
    {
        Application.Quit();
    }


    // Update is called once per frame
    void Update()
    {
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
