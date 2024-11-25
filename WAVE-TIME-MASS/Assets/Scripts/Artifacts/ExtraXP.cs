using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraXP : Artifact
{
    public override void Use()
    {
        hero.SetLives(hero.GetLives() + 1);

    }
}
