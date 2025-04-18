using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        // Очищаем инвентарь перед рестартом
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.ClearInventory();
            InventoryUI.Instance.SaveInventory(); // Сохраняем пустой инвентарь
        }

        Debug.Log("новая игра");
        TimeManager.ResetFreezeCount(); // Сбросить заморозку времени
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Continue()
    {
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.LoadInventory(PlayerPrefs.GetInt("SavedScene")); // Загружаем сохранённый инвентарь
        }

        TimeManager.ResetFreezeCount(); // Сбросить заморозку времени
        SceneManager.LoadScene(PlayerPrefs.GetInt("SavedScene"));
    }
    public void Exit()
    {
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.SaveInventory(); // Сохраняем инвентарь перед выходом
        }

        Debug.Log("закрытие игры");
        Application.Quit();
    }
}
