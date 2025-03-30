using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entity : MonoBehaviour
{
    public int livess;
    public virtual void GetDamageHero()
    {

    }

    public virtual void GetDamage()
    {
        livess--;
        if (livess < 1)
            Die();
        Debug.Log("У врага " + livess + " хп");
    }

    public virtual void Die()
    {
        Destroy(this.gameObject);

    }
}
