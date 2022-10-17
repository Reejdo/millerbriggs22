using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBullet : Bullet
{
    protected override void StartDirection()
    {
        //Need to override this so Shooting script can determine direction
        //base.StartDirection();
    }
}
