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
        Time.timeScale = 1f;
    }

    public void MainMenu() 
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void NewGame() 
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
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
                Time.timeScale = 0f;
            }
            else
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }
}
