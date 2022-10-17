using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstWallTouch : MonoBehaviour
{
    public PlayerControl myPlayerControl;
    private BoxCollider2D playerCollider; 

    void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>(); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isTouchingWall = Physics2D.BoxCast(playerCollider.bounds.center, 
            playerCollider.bounds.size, 0f, transform.right * myPlayerControl.facingDirection, 0.1f, myPlayerControl.whatIsWall);

        if (isTouchingWall)
        {
            Debug.Log("Is touching wall"); 
            myPlayerControl.SetTouchingWall(true); 
        }
    }
}
