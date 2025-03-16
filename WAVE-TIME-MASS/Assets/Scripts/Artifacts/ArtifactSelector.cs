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
        playerController = FindObjectOfType<Hero>(); // ���� ������

        TimeManager.FreezeTime(); // ������������ ����

        artifactPanel.SetActive(true); // ���������� ����

        // ����������� ������
        doubleJumpButton.onClick.AddListener(() => SelectArtifact("DoubleJump"));
        doubleHPButton.onClick.AddListener(() => SelectArtifact("DoubleHP"));
        jumpAttackButton.onClick.AddListener(() => SelectArtifact("JumpAttack"));
    }

    void SelectArtifact(string artifact)
    {
        // ���������� ��� ���������
        PlayerPrefs.SetInt("DoubleJump", 0);
        PlayerPrefs.SetInt("DoubleHP", 0);
        PlayerPrefs.SetInt("JumpAttack", 0);

        // ��������� ��������� ��������
        switch (artifact)
        {
            case "DoubleJump":
                PlayerPrefs.SetInt("DoubleJump", 1);
                Debug.Log("������ DoubleJump");
                break;
            case "DoubleHP":
                PlayerPrefs.SetInt("DoubleHP", 1);
                Debug.Log("������ DoubleHP");
                break;
            case "JumpAttack":
                PlayerPrefs.SetInt("JumpAttack", 1);
                Debug.Log("������ JumpAttack");
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