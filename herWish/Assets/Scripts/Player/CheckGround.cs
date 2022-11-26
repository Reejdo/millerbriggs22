using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public LayerMask whatIsGround;
    public PlayerControl myPlayerControl;

    public bool isGround_1; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((whatIsGround.value & (1 << collision.gameObject.layer)) > 0)
        {
            if (isGround_1)
            {
                myPlayerControl.groundCheck_1 = true;
            }
            else
            {
                myPlayerControl.groundCheck_2 = true;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((whatIsGround.value & (1 << collision.gameObject.layer)) > 0)
        {
            if (isGround_1)
            {
                myPlayerControl.groundCheck_1 = false;
            }
            else
            {
                myPlayerControl.groundCheck_2 = false;
            }

        }
    }
}
