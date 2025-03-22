using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private List<Image> inventorySlots; // ������ ����� ���������
    [SerializeField] private List<Sprite> artifactIcons; // ������ ������ ����������

    private List<int> inventory = new List<int>(); // ������ ID ���������� � ���������

    public static InventoryUI Instance; // �������� ��� ���������� ������� ����� �������

    private void Awake()
    {
        // ���������� ���������
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ���������� ������ ��� �������� ����� �����
        }
        else
        {
            // ���� ��� ���� ������ InventoryUI, ���������� ���� ������
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // ������� ��������� ��� ������ (���� �����)
        //ClearInventory();
    }

    // ���������� ��������� � ���������
    public void AddArtifact(int artifactID)
    {
        if (inventory.Count < 3) // ���������, ���� �� ��������� ������
        {
            inventory.Add(artifactID); // ��������� �������� � ������
            UpdateInventoryUI(); // ��������� UI
        }
        else
        {
            Debug.Log("��������� �����! ������� ��������, ����� �������� �����.");
        }
    }

    // �������� ��������� �� ���������
    public void RemoveArtifact(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < inventory.Count)
        {
            inventory.RemoveAt(slotIndex); // ������� �������� �� ������
            UpdateInventoryUI(); // ��������� UI
        }
    }

    // ������� ���������
    public void ClearInventory()
    {
        inventory.Clear();
        UpdateInventoryUI();
    }

    // �������� ������� ��������� � ���������
    public bool HasArtifact(int artifactID)
    {
        return inventory.Contains(artifactID); // ���������� true, ���� �������� ���� � ���������
    }

    // ���������� UI ���������
    private void UpdateInventoryUI()
    {
        // ������� ��� ������
        foreach (var slot in inventorySlots)
        {
            slot.sprite = null;
            slot.color = Color.clear; // ������ ������ ����������
        }

        // ��������� ������ �������� ����������
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] >= 0 && inventory[i] < artifactIcons.Count)
            {
                inventorySlots[i].sprite = artifactIcons[inventory[i]];
                inventorySlots[i].color = Color.white; // ������ ������ �������
            }
        }
    }
}