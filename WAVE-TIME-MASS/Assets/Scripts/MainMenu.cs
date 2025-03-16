using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        Debug.Log("нова€ игра");
        TimeManager.ResetFreezeCount(); // —бросить заморозку времени
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Continue()
    {
        TimeManager.ResetFreezeCount(); // —бросить заморозку времени
        SceneManager.LoadScene(PlayerPrefs.GetInt("SavedScene"));
    }
    public void Exit()
    {
        Debug.Log("закрытие игры");
        Application.Quit();
    }
}
