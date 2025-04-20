using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Saw : Entity
{
    public float speed;
    public bool MovingRight;
    public Vector2 startPosition;
    public GameObject deathMenu;


    private void OnEnable()
    {
        transform.position = startPosition;
    }

    private void Update()
    {
        if (MovingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other) // �������� �� ������������ 
    {
        // ������������ ����� ��� ������������ ������ � ����
        if (other.gameObject.CompareTag("Player"))
        {
            // �������� ��������� Hero � ������
            Hero hero = other.gameObject.GetComponent<Hero>();

            if (hero != null)
            {
                // �������� ����� Die() � �����
                hero.Die();
            }
            else
            {
                deathMenu.SetActive(true);
            }
        }

        if ((other.CompareTag("Obstacle")) || (other.CompareTag("Saw")))
        {
            MovingRight = !MovingRight;
        }
    }
    
}
