using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingEnemies : MonoBehaviour
{
    public float speed;
    public Vector3[] positions;

    private int currentTarget;
    private SpriteRenderer sprite;
    
    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    
    void Flip() // ����� ����������� ��� ������������
    {
        var target = transform.position;
        if (target.x == positions[1].x )
        {
            sprite.flipX = true;
        }
        if (target.x == positions[0].x)
        {
            sprite.flipX = false;
        }
    }

    public void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, positions[currentTarget], speed);

        if (transform.position == positions[currentTarget])
        {
            if (currentTarget < positions.Length - 1)
            {
                currentTarget++;
            } else 
            {
                currentTarget = 0;
            }
        }
    }   
        Flip();
    }        

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
        }
    }
}
