using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DynamicTextVisibility : MonoBehaviour
{
    private TextMeshPro textMeshPro;

    public Transform player;
    public float maxDistance = 10f; // ������������ ����������, ��� ������� ����� ����� �����
    public float minAlpha = 0f; // ����������� ������� ������������ ������
    public float maxAlpha = 3f; // ������������ ������� ������������ ������

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        player = Hero.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position); // ��������� ���������� ����� ������� � �������
        float alpha = Mathf.Clamp01(1 - (distance / maxDistance));// ����������� ���������� �� ������ maxDistance
        textMeshPro.alpha = Mathf.Lerp(minAlpha, maxAlpha, alpha); // ������������� ������� ����� �� ������� ��� ������������

        
    }
}
