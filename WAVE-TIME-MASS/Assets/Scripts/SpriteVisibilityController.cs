using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteVisibilityController : MonoBehaviour
{
    public Transform player;
    public SpriteRenderer spriteRenderer;
    public float maxDistance = 10f; // ������������ ����������, ��� ������� ����� ����� �����
    public float minAlpha = 0f; // ����������� ������� ������������ ������
    public float maxAlpha = 3f;  // ������������ ������� ������������ ������




    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = Hero.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        float alpha = Mathf.Clamp01(1 - (distance / maxDistance));
        Color spriteColor = spriteRenderer.color;
        spriteColor.a = Mathf.Lerp(minAlpha, maxAlpha, alpha);
        spriteRenderer.color = spriteColor;


    }
}
