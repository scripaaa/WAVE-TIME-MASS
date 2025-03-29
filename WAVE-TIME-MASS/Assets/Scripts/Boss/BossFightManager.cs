using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossFightManager : MonoBehaviour
{
    public GameObject leftDoor;  // ����� �����
    public GameObject rightDoor; // ������ �����
    public Text bossText;        // ����� "��� � ������"
    public GameObject boss;      // ������ �����

    private Hero playerController;
    private bool hasTriggered = false; // ���� ��� ������������ ������������

    void Start()
    {
        playerController = FindObjectOfType<Hero>(); // ���� ������
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ������� ��� ���������� ��� ��� �� ����� - �������
        if (hasTriggered || !collision.CompareTag("Player"))
            return;

        hasTriggered = true; // �������� ������� ��� �����������
        StartCoroutine(StartBossFight());
    }

    private IEnumerator StartBossFight()
    {
        // ��������� �����
        if (leftDoor != null) leftDoor.SetActive(true);
        if (rightDoor != null) rightDoor.SetActive(true);

        // ���������� �������
        if (bossText != null)
        {
            bossText.text = "Boss fight!";
            bossText.gameObject.SetActive(true);
        }

        if (playerController != null)
        {
            playerController.AddHeart();
            playerController.AddHeart();
        }

        // ���� 5 ������
        yield return new WaitForSeconds(5f);

        // �������� �������
        if (bossText != null) bossText.gameObject.SetActive(false);

        // ���������� �����
        if (boss != null) boss.SetActive(true);
    }

    // �����������: ����� ��� ������ �������� (���� ����� ������������� �����)
    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}