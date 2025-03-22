using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactSelector : MonoBehaviour
{
    public GameObject artifactPanel;
    public Button doubleJumpButton; // Двойная высота прыжка
    public Button doubleHPButton; // + ХП
    public Button jumpAttackButton; // Атака прыжком
    public Button dashButton; // Рывок
    public Button meleeAttackButton; // Ближнаяя атака
    public Button meleeAttackButton2; // Ближнаяя атака (по факту не нужна, только для красивого отображения)
    public Button rangedAttackButton; // Дальняя атака
    public Button slowdownButton; // Замедление

    private Hero playerController;
    public PlayerDash playerDash; // Ссылка на скрипт PlayerDash
    //public InventoryUI inventoryUI; // Ссылка на скрипт инвентаря

    public int currentLevel = 1; // Текущий уровень (можно задавать извне)

    void Start()
    {
        playerController = FindObjectOfType<Hero>(); // Ищем игрока
        playerDash = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDash>(); // Находим скрипт PlayerDash на объекте игрока

        TimeManager.FreezeTime(); // Замораживаем игру

        artifactPanel.SetActive(true); // Показываем окно

        // Настраиваем кнопки в зависимости от уровня
        SetupButtonsForLevel(currentLevel);

        // Привязываем кнопки
        doubleJumpButton.onClick.AddListener(() => SelectArtifact("DoubleJump"));
        doubleHPButton.onClick.AddListener(() => SelectArtifact("DoubleHP"));
        jumpAttackButton.onClick.AddListener(() => SelectArtifact("JumpAttack"));
        dashButton.onClick.AddListener(() => SelectArtifact("Dash"));
        meleeAttackButton.onClick.AddListener(() => SelectArtifact("MeleeAttack"));
        meleeAttackButton2.onClick.AddListener(() => SelectArtifact("MeleeAttack"));
        rangedAttackButton.onClick.AddListener(() => SelectArtifact("RangedAttack"));
        slowdownButton.onClick.AddListener(() => SelectArtifact("Slowdown"));
    }

    // Настройка кнопок в зависимости от уровня
    void SetupButtonsForLevel(int level)
    {
        // По умолчанию все кнопки неактивны
        doubleJumpButton.gameObject.SetActive(false);
        doubleHPButton.gameObject.SetActive(false);
        jumpAttackButton.gameObject.SetActive(false);
        dashButton.gameObject.SetActive(false);
        meleeAttackButton.gameObject.SetActive(false);
        meleeAttackButton2.gameObject.SetActive(false);
        rangedAttackButton.gameObject.SetActive(false);
        slowdownButton.gameObject.SetActive(false);

        // Включаем только нужные кнопки для текущего уровня
        switch (level)
        {
            case 2:
                jumpAttackButton.gameObject.SetActive(true);
                dashButton.gameObject.SetActive(true);
                meleeAttackButton2.gameObject.SetActive(true);
                break;
            case 3:
                if (InventoryUI.Instance.HasArtifact(2))
                {
                    doubleJumpButton.gameObject.SetActive(true);
                    dashButton.gameObject.SetActive(true);
                }
                else
                {
                    meleeAttackButton.gameObject.SetActive(true);
                    rangedAttackButton.gameObject.SetActive(true);
                }
                break;
            case 4:
                if (InventoryUI.Instance.HasArtifact(2) && (InventoryUI.Instance.HasArtifact(0) || InventoryUI.Instance.HasArtifact(5)))
                {
                    rangedAttackButton.gameObject.SetActive(true);
                    doubleHPButton.gameObject.SetActive(true);
                    slowdownButton.gameObject.SetActive(true);
                }
                else if (InventoryUI.Instance.HasArtifact(3) && InventoryUI.Instance.HasArtifact(2))
                {
                    rangedAttackButton.gameObject.SetActive(true);
                    doubleHPButton.gameObject.SetActive(true);
                    dashButton.gameObject.SetActive(true);
                }
                else if (InventoryUI.Instance.HasArtifact(5) && InventoryUI.Instance.HasArtifact(2))
                {
                    rangedAttackButton.gameObject.SetActive(true);
                    doubleHPButton.gameObject.SetActive(true);
                    slowdownButton.gameObject.SetActive(true);
                }
                else if ((InventoryUI.Instance.HasArtifact(3) || InventoryUI.Instance.HasArtifact(5)) && InventoryUI.Instance.HasArtifact(4))
                {
                    doubleHPButton.gameObject.SetActive(true);
                    meleeAttackButton.gameObject.SetActive(true);
                    doubleJumpButton.gameObject.SetActive(true);
                }
                break;
            default:
                // Если уровень не определен, показываем все кнопки
                doubleJumpButton.gameObject.SetActive(true);
                doubleHPButton.gameObject.SetActive(true);
                jumpAttackButton.gameObject.SetActive(true);
                dashButton.gameObject.SetActive(true);
                meleeAttackButton.gameObject.SetActive(true);
                rangedAttackButton.gameObject.SetActive(true);
                slowdownButton.gameObject.SetActive(true);
                break;
        }
    }

    void SelectArtifact(string artifact)
    {
        // Сбрасываем все артефакты
        /*PlayerPrefs.SetInt("DoubleJump", 0);
        PlayerPrefs.SetInt("DoubleHP", 0);
        PlayerPrefs.SetInt("JumpAttack", 0);
        PlayerPrefs.SetInt("Dash", 0);
        PlayerPrefs.SetInt("MeleeAttack", 0);
        PlayerPrefs.SetInt("RangedAttack", 0);
        PlayerPrefs.SetInt("Slowdown", 0);*/

        // Сохраняем выбранный артефакт
        switch (artifact)
        {
            case "DoubleJump":
                PlayerPrefs.SetInt("DoubleJump", 1);
                Debug.Log("Выбран DoubleJump");
                InventoryUI.Instance.AddArtifact(0);
                if (playerController != null)
                {
                    playerController.DoubleJump();
                }
                break;
            case "DoubleHP":
                PlayerPrefs.SetInt("DoubleHP", 1);
                Debug.Log("Выбран DoubleHP");
                InventoryUI.Instance.AddArtifact(1);
                if (playerController != null)
                {
                    playerController.AddHeart();
                }
                break;
            case "JumpAttack":
                PlayerPrefs.SetInt("JumpAttack", 1);
                Debug.Log("Выбран JumpAttack");
                InventoryUI.Instance.AddArtifact(3);
                if (playerController != null)
                {
                    playerController.Active_Damage_Jump();
                }
                break;
            case "Dash":
                PlayerPrefs.SetInt("Dash", 1);
                Debug.Log("Выбран Dash");
                if (playerDash != null)
                {
                    playerDash.canDash = true;
                }
                InventoryUI.Instance.AddArtifact(5);
                break;
            case "MeleeAttack":
                PlayerPrefs.SetInt("MeleeAttack", 1);
                Debug.Log("Выбран MeleeAttack");
                InventoryUI.Instance.AddArtifact(2);
                if (playerController != null)
                {
                    playerController.Active_Melee_Attacking();
                }
                break;
        }

        PlayerPrefs.Save(); // Сохраняем изменения
        HideArtifactPanel();
    }

    void HideArtifactPanel()
    {
        artifactPanel.SetActive(false); // Скрываем окно

        TimeManager.UnfreezeTime(); // Размораживаем игру
        TimeManager.ResetFreezeCount(); // Сбросить заморозку времени

        // Применяем артефакты к игроку
        //ApplyArtifacts();
    }

    /*void ApplyArtifacts()
    {
        if (playerController != null)
        {
            playerController.ApplyArtifactEffects();
        }
    }*/
}