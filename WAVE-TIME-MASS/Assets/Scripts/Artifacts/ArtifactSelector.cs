using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactSelector : MonoBehaviour
{
    public GameObject artifactPanel;
    public Button doubleJumpButton; // ������� ������ ������
    public Button doubleHPButton; // + ��
    public Button jumpAttackButton; // ����� �������
    public Button dashButton; // �����
    public Button meleeAttackButton; // �������� �����
    public Button meleeAttackButton2; // �������� ����� (�� ����� �� �����, ������ ��� ��������� �����������)
    public Button rangedAttackButton; // ������� �����
    public Button slowdownButton; // ����������

    private Hero playerController;
    public PlayerDash playerDash; // ������ �� ������ PlayerDash
    //public InventoryUI inventoryUI; // ������ �� ������ ���������

    public int currentLevel = 1; // ������� ������� (����� �������� �����)

    void Start()
    {
        playerController = FindObjectOfType<Hero>(); // ���� ������
        playerDash = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDash>(); // ������� ������ PlayerDash �� ������� ������

        TimeManager.FreezeTime(); // ������������ ����

        artifactPanel.SetActive(true); // ���������� ����

        // ����������� ������ � ����������� �� ������
        SetupButtonsForLevel(currentLevel);

        // ����������� ������
        doubleJumpButton.onClick.AddListener(() => SelectArtifact("DoubleJump"));
        doubleHPButton.onClick.AddListener(() => SelectArtifact("DoubleHP"));
        jumpAttackButton.onClick.AddListener(() => SelectArtifact("JumpAttack"));
        dashButton.onClick.AddListener(() => SelectArtifact("Dash"));
        meleeAttackButton.onClick.AddListener(() => SelectArtifact("MeleeAttack"));
        meleeAttackButton2.onClick.AddListener(() => SelectArtifact("MeleeAttack"));
        rangedAttackButton.onClick.AddListener(() => SelectArtifact("RangedAttack"));
        slowdownButton.onClick.AddListener(() => SelectArtifact("Slowdown"));
    }

    // ��������� ������ � ����������� �� ������
    void SetupButtonsForLevel(int level)
    {
        // �� ��������� ��� ������ ���������
        doubleJumpButton.gameObject.SetActive(false);
        doubleHPButton.gameObject.SetActive(false);
        jumpAttackButton.gameObject.SetActive(false);
        dashButton.gameObject.SetActive(false);
        meleeAttackButton.gameObject.SetActive(false);
        meleeAttackButton2.gameObject.SetActive(false);
        rangedAttackButton.gameObject.SetActive(false);
        slowdownButton.gameObject.SetActive(false);

        // �������� ������ ������ ������ ��� �������� ������
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
                // ���� ������� �� ���������, ���������� ��� ������
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
        // ���������� ��� ���������
        /*PlayerPrefs.SetInt("DoubleJump", 0);
        PlayerPrefs.SetInt("DoubleHP", 0);
        PlayerPrefs.SetInt("JumpAttack", 0);
        PlayerPrefs.SetInt("Dash", 0);
        PlayerPrefs.SetInt("MeleeAttack", 0);
        PlayerPrefs.SetInt("RangedAttack", 0);
        PlayerPrefs.SetInt("Slowdown", 0);*/

        // ��������� ��������� ��������
        switch (artifact)
        {
            case "DoubleJump":
                PlayerPrefs.SetInt("DoubleJump", 1);
                Debug.Log("������ DoubleJump");
                InventoryUI.Instance.AddArtifact(0);
                if (playerController != null)
                {
                    playerController.DoubleJump();
                }
                break;
            case "DoubleHP":
                PlayerPrefs.SetInt("DoubleHP", 1);
                Debug.Log("������ DoubleHP");
                InventoryUI.Instance.AddArtifact(1);
                if (playerController != null)
                {
                    playerController.AddHeart();
                }
                break;
            case "JumpAttack":
                PlayerPrefs.SetInt("JumpAttack", 1);
                Debug.Log("������ JumpAttack");
                InventoryUI.Instance.AddArtifact(3);
                if (playerController != null)
                {
                    playerController.Active_Damage_Jump();
                }
                break;
            case "Dash":
                PlayerPrefs.SetInt("Dash", 1);
                Debug.Log("������ Dash");
                if (playerDash != null)
                {
                    playerDash.canDash = true;
                }
                InventoryUI.Instance.AddArtifact(5);
                break;
            case "MeleeAttack":
                PlayerPrefs.SetInt("MeleeAttack", 1);
                Debug.Log("������ MeleeAttack");
                InventoryUI.Instance.AddArtifact(2);
                if (playerController != null)
                {
                    playerController.Active_Melee_Attacking();
                }
                break;
        }

        PlayerPrefs.Save(); // ��������� ���������
        HideArtifactPanel();
    }

    void HideArtifactPanel()
    {
        artifactPanel.SetActive(false); // �������� ����

        TimeManager.UnfreezeTime(); // ������������� ����
        TimeManager.ResetFreezeCount(); // �������� ��������� �������

        // ��������� ��������� � ������
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