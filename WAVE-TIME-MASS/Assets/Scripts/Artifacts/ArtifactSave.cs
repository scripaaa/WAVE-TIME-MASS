using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactSave : MonoBehaviour
{
    private Hero player; // ������ �� ������
    private PlayerDash playerDash; // ������ �� ������ PlayerDash

    private void Start()
    {
        player = FindObjectOfType<Hero>(); // ���� ������
        playerDash = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDash>(); // ������� ������ PlayerDash �� ������� ������

        ApplyArtifactEffects();
    }

    public void ApplyArtifactEffects()
    {
        // ���������, ����� ��������� ���� � ���������, � ��������� �� �������
        if (InventoryUI.Instance.HasArtifact(0)) // DoubleJump
        {
            player.DoubleJump(); // ��������� ������ �������� ������
        }

        if (InventoryUI.Instance.HasArtifact(1)) // DoubleHP
        {
            player.AddHeart(); // ��������� ������ ��������������� ��������
        }

        if (InventoryUI.Instance.HasArtifact(3)) // JumpAttack
        {
            player.Active_Damage_Jump(); // ��������� ������ ����� �������
        }

        if (InventoryUI.Instance.HasArtifact(5)) // Dash
        {
            playerDash.canDash = true; // ��������� �����
        }

        if (InventoryUI.Instance.HasArtifact(2)) // MeleeAttack
        {
            player.Active_Melee_Attacking(); // ��������� ������� �����
        }
        // �������� �������� ��� ������ ����������
    }
}
