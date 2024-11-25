using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Artifact : ScriptableObject
{
    protected Hero hero=FindObjectOfType<Hero>();
    public virtual void Use() { }
}
