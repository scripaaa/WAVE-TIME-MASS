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
            SaveInventory(); // ��������� ���������
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
            SaveInventory(); // ��������� ���������
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

    // � InventoryUI.cs
    public void SaveInventory()
    {
        // ��������� ���������� ����������
        PlayerPrefs.SetInt("InventoryCount", inventory.Count);

        // ��������� ������ �������� �� ��� ID
        for (int i = 0; i < inventory.Count; i++)
        {
            PlayerPrefs.SetInt($"InventoryArtifact_{i}", inventory[i]);
        }
        PlayerPrefs.Save(); // �����!
    }

    public void LoadInventory(int count)
    {
        ClearInventory(); // ������� ������� ���������

        for (int i = 0; i < count - 2; i++)
        {
            int artifactID = PlayerPrefs.GetInt($"InventoryArtifact_{i}", -1);
            if (artifactID != -1)
            {
                inventory.Add(artifactID);
            }
        }

        UpdateInventoryUI(); // ��������� UI
    }
}