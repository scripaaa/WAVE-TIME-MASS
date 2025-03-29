using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactSave : MonoBehaviour
{
    private Hero player; // ������ �� ������
    private PlayerDash playerDash; // ������ �� ������ PlayerDash
    private Slowdown slowdown; // ������ �� Slowdown

    private void Start()
    {
        player = FindObjectOfType<Hero>(); // ���� ������
        playerDash = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDash>(); // ������� ������ PlayerDash �� ������� ������
        slowdown = GameObject.FindGameObjectWithTag("Player").GetComponent<Slowdown>(); // ������� ������ Slowdown �� ������� ������

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

        if (InventoryUI.Instance.HasArtifact(6)) // Slowdown
        {
            slowdown.ActivateSlowdown(); // ��������� �����
        }

        if (InventoryUI.Instance.HasArtifact(4)) // Range_Attacking
        {
            player.Active_Range_Attacking(); // ��������� ������� �����
        }


        // �������� �������� ��� ������ ����������
    }
}
