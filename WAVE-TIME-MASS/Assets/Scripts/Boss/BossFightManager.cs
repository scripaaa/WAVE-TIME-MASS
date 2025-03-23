using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossFightManager : MonoBehaviour
{
    public GameObject leftDoor;  // ����� �����
    public GameObject rightDoor; // ������ �����
    public Text bossText;        // ����� "��� � ������"
    public GameObject boss;      // ������ �����

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("������� ��������!"); // �������� ��� ������

        if (collision.CompareTag("Player")) // ���������, ��� ��� �����
        {
            StartCoroutine(StartBossFight()); // ��������� �����
        }
    }

    private IEnumerator StartBossFight()
    {
        // ��������� �����
        leftDoor.SetActive(true);
        rightDoor.SetActive(true);

        // ���������� ������� "��� � ������"
        bossText.text = "��� � ������";
        bossText.gameObject.SetActive(true);

        // ���� 5 ������
        yield return new WaitForSeconds(5f);

        // �������� �������
        bossText.gameObject.SetActive(false);

        // ���������� �����
        boss.SetActive(true);
    }
}