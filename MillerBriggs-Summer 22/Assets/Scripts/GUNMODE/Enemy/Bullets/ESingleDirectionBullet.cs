using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESingleDirectionBullet : EnemyBullet
{
    protected override void MoveTowardsPlayer()
    {
        //base.MoveTowardsPlayer();
        //This won't have them move towards the player, but since it's in the update funciton, we'll just override it
        //Have this move in the upwards direction from whatever it's rotation is

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * moveSpeed; 

    }


}
