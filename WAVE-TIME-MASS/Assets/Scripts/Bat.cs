using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class Bat : Entity
{
    public Transform[] waypoints; // ����� �����������
    public float speed; // ��������

    private int currentPointIndex = 0;
    private Transform player; // ������ �� ������
    private Vector3 initialScale;
    private bool isChasing;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // ���������� ������
        initialScale = transform.localScale; // ���������� ���������� ����� 
    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentPointIndex].position, speed * Time.deltaTime);
        LookAtTarget(waypoints[currentPointIndex].position); // ���� ������� �� ����������� ��������
        if (Vector2.Distance(transform.position, waypoints[currentPointIndex].position) < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % waypoints.Length;
        }
    }

    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        LookAtTarget(player.position);
    }

    private void OnTriggerEnter2D(Collider2D other) // ���� ������ � ���� ���������
    {
        if (other.CompareTag("Player"))
        {
            isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) // ����� ������ �� ���� ���������
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false;
        }
    }

    void LookAtTarget(Vector2 targetPosition)
    {
        float directionX = targetPosition.x - transform.position.x;
        if (directionX > 0)
        {
            transform.localScale = new Vector2(initialScale.x, initialScale.y);
        }
        else if (directionX < 0)
        {
            transform.localScale = new Vector2(-initialScale.x, initialScale.y);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
        }
    }
}
