using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : Artifact
{
    public override void Use()
    {
        hero.SetJumpForce(hero.GetJumpForce()+7);
    }
}
