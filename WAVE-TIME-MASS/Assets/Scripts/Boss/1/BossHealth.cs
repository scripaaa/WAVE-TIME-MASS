using UnityEngine;
using UnityEngine.UI;

public class BossHealth : Entity  // ����������� �� Entity
{
    [Header("Health Settings")]
    [SerializeField] private int _maxHealth = 10;  // ������������ ��������
    [SerializeField] private Slider _healthBar;    // ������� ��� ����������� ��������

    public GameObject leftDoor;  // ����� �����
    public GameObject rightDoor; // ������ �����

    private void Start()
    {
        livess = _maxHealth;  // �������������� livess �� ������������� ������
        UpdateHealthBar();
    }

    // �������������� ����� GetDamage
    public override void GetDamage()
    {
        base.GetDamage();  // �������� ������������ ����� (��������� livess)
        UpdateHealthBar();

        if (livess <= 0)
        {
            Die();  // ������� ������������ Die() ��� ���������������� (��. ����)
        }
    }

    // �������������� ����� Die (�����������)
    public override void Die()
    {
        Debug.Log("���� ��������!");
        // �������������� ��������: �������� ������, ������� � �.�.
        base.Die();  // ���������� ������ (������������ �����)

        // ��������� �����
        if (leftDoor != null) leftDoor.SetActive(false);
        if (rightDoor != null) rightDoor.SetActive(false);
    }

    private void UpdateHealthBar()
    {
        if (_healthBar != null)
        {
            _healthBar.value = livess;
        }
    }
}