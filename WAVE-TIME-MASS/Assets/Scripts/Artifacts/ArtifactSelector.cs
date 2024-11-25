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
        // Ищем игрока
        playerController = FindObjectOfType<Hero>();
        if (playerController != null)
        {
            playerController.canControl = false; // Блокируем управление
        }

        artifactPanel.SetActive(true); // Показываем окно

        // Привязываем кнопки
        doubleJumpButton.onClick.AddListener(() => SelectArtifact("DoubleJump"));
        doubleHPButton.onClick.AddListener(() => SelectArtifact("DoubleHP"));
        jumpAttackButton.onClick.AddListener(() => SelectArtifact("JumpAttack"));
    }

    void SelectArtifact(string artifact)
    {
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
        if (playerController != null)
        {
            playerController.canControl = true; // Разрешаем управление
        }

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