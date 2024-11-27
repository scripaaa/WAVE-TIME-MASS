using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactSelector : MonoBehaviour
{
    public GameObject artifactPanel;
    public Button doubleJumpButton;
    public Button doubleHPButton;
    public Button jumpAttackButton;

    private Hero playerController;

    void Start()
    {
        playerController = FindObjectOfType<Hero>(); // Ищем игрока

        TimeManager.FreezeTime(); // Замораживаем игру

        artifactPanel.SetActive(true); // Показываем окно

        // Привязываем кнопки
        doubleJumpButton.onClick.AddListener(() => SelectArtifact("DoubleJump"));
        doubleHPButton.onClick.AddListener(() => SelectArtifact("DoubleHP"));
        jumpAttackButton.onClick.AddListener(() => SelectArtifact("JumpAttack"));
    }

    void SelectArtifact(string artifact)
    {
        // Сбрасываем все артефакты
        PlayerPrefs.SetInt("DoubleJump", 0);
        PlayerPrefs.SetInt("DoubleHP", 0);
        PlayerPrefs.SetInt("JumpAttack", 0);

        // Сохраняем выбранный артефакт
        switch (artifact)
        {
            case "DoubleJump":
                PlayerPrefs.SetInt("DoubleJump", 1);
                Debug.Log("Выбран DoubleJump");
                break;
            case "DoubleHP":
                PlayerPrefs.SetInt("DoubleHP", 1);
                Debug.Log("Выбран DoubleHP");
                break;
            case "JumpAttack":
                PlayerPrefs.SetInt("JumpAttack", 1);
                Debug.Log("Выбран JumpAttack");
                break;
        }

        PlayerPrefs.Save(); // Сохраняем изменения
        HideArtifactPanel();
    }

    void HideArtifactPanel()
    {
        artifactPanel.SetActive(false); // Скрываем окно

        TimeManager.UnfreezeTime(); // Размораживаем игру

        // Применяем артефакты к игроку
        ApplyArtifacts();
    }

    void ApplyArtifacts()
    {
        if (playerController != null)
        {
            playerController.ApplyArtifactEffects();
        }
    }
}