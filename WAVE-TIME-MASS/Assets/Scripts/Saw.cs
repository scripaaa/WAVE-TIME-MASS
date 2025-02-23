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
    private void OnTriggerEnter2D(Collider2D other) // проверка на столкновение 
    {
        // Перезагрузка сцена при столкновении игрока и пилы
        if (other.gameObject.CompareTag("Player"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if ((other.CompareTag("Obstacle")) || (other.CompareTag("Saw")))
        {
            MovingRight = !MovingRight;
        }
    }
    
}
