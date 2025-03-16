using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathByObject : Entity
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Hero.Instance.GetDamageHero();
            if (livess < 1)
                Die();
        }

    }
}
