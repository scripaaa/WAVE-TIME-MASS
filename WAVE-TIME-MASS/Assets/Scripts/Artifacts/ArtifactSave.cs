using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactSave : MonoBehaviour
{
    private Hero player; // Ссылка на игрока
    private PlayerDash playerDash; // Ссылка на скрипт PlayerDash

    private void Start()
    {
        player = FindObjectOfType<Hero>(); // Ищем игрока
        playerDash = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDash>(); // Находим скрипт PlayerDash на объекте игрока

        ApplyArtifactEffects();
    }

    public void ApplyArtifactEffects()
    {
        // Проверяем, какие артефакты есть в инвентаре, и применяем их эффекты
        if (InventoryUI.Instance.HasArtifact(0)) // DoubleJump
        {
            player.DoubleJump(); // Применяем эффект двойного прыжка
        }

        if (InventoryUI.Instance.HasArtifact(1)) // DoubleHP
        {
            player.AddHeart(); // Применяем эффект дополнительного здоровья
        }

        if (InventoryUI.Instance.HasArtifact(3)) // JumpAttack
        {
            player.Active_Damage_Jump(); // Применяем эффект атаки прыжком
        }

        if (InventoryUI.Instance.HasArtifact(5)) // Dash
        {
            playerDash.canDash = true; // Разрешаем рывок
        }

        if (InventoryUI.Instance.HasArtifact(2)) // MeleeAttack
        {
            player.Active_Melee_Attacking(); // Разрешаем ближнюю атаку
        }
        // Добавьте проверки для других артефактов
    }
}
