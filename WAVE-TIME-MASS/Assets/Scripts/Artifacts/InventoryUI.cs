using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private List<Image> inventorySlots; // Список ячеек инвентаря
    [SerializeField] private List<Sprite> artifactIcons; // Список иконок артефактов

    private List<int> inventory = new List<int>(); // Список ID артефактов в инвентаре

    public static InventoryUI Instance; // Синглтон для сохранения объекта между сценами

    private void Awake()
    {
        // Реализация синглтона
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожать объект при загрузке новой сцены
        }
        else
        {
            // Если уже есть другой InventoryUI, уничтожаем этот объект
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Очищаем инвентарь при старте (если нужно)
        //ClearInventory();
    }

    // Добавление артефакта в инвентарь
    public void AddArtifact(int artifactID)
    {
        if (inventory.Count < 3) // Проверяем, есть ли свободные ячейки
        {
            inventory.Add(artifactID); // Добавляем артефакт в список
            UpdateInventoryUI(); // Обновляем UI
        }
        else
        {
            Debug.Log("Инвентарь полон! Удалите артефакт, чтобы добавить новый.");
        }
    }

    // Удаление артефакта из инвентаря
    public void RemoveArtifact(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < inventory.Count)
        {
            inventory.RemoveAt(slotIndex); // Удаляем артефакт из списка
            UpdateInventoryUI(); // Обновляем UI
        }
    }

    // Очистка инвентаря
    public void ClearInventory()
    {
        inventory.Clear();
        UpdateInventoryUI();
    }

    // Проверка наличия артефакта в инвентаре
    public bool HasArtifact(int artifactID)
    {
        return inventory.Contains(artifactID); // Возвращает true, если артефакт есть в инвентаре
    }

    // Обновление UI инвентаря
    private void UpdateInventoryUI()
    {
        // Очищаем все ячейки
        foreach (var slot in inventorySlots)
        {
            slot.sprite = null;
            slot.color = Color.clear; // Делаем ячейку прозрачной
        }

        // Заполняем ячейки иконками артефактов
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] >= 0 && inventory[i] < artifactIcons.Count)
            {
                inventorySlots[i].sprite = artifactIcons[inventory[i]];
                inventorySlots[i].color = Color.white; // Делаем ячейку видимой
            }
        }
    }
}