using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class WalkingEnemies : Entity
{
    public float speed;
    public Vector3[] positions;
    private int currentTarget;
   

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
    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
        }
    }

    
}
