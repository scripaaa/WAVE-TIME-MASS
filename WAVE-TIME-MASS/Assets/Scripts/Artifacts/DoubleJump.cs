using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : Artifact
{
    public override void Use()
    {
        hero.SetJumpForse(hero.GetJumpForse()+7);
    }
}
