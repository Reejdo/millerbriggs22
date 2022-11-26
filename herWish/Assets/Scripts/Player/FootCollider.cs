using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootCollider : MonoBehaviour
{
    public LayerMask whatIsGround;
    public PlayerControl myPlayerControl;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((whatIsGround.value & (1 << collision.gameObject.layer)) > 0)
        {
            myPlayerControl.jumpColliderHit = true; 

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((whatIsGround.value & (1 << collision.gameObject.layer)) > 0)
        {
            myPlayerControl.jumpColliderHit = false; 
        }
    }
}
