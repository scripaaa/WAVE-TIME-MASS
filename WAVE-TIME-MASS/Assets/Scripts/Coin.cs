using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isCollected)
        {
            isCollected = true;
            collision.GetComponent<Hero>().AddCoin();
            Destroy(gameObject);
        }
    }
}
