using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class SawUnable : Entity
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other) // проверка на столкновение 
    {
        // Перезагрузка сцена при столкновении игрока и пилы
        if (other.gameObject.CompareTag("Player"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
