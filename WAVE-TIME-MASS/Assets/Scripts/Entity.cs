using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entity : MonoBehaviour
{
    public virtual void GetDamage()
    {

    }

    public virtual void Die()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
