using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImproveJump : MonoBehaviour
{
    //allows for snappier jumps, two different kinds of jumps as well
    [SerializeField]
    private float fallMultiplier = 2.5f, lowJumpMultiplier = 2f;
    private Rigidbody2D myRigidBody2D;
    private PlayerControl myPlayerControl;
    private PControlOld myPlayerControlOld; 

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myPlayerControl = GetComponent<PlayerControl>();
        myPlayerControlOld = GetComponent<PControlOld>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (myPlayerControl != null)
        {
            //subtract 1 to account for normal gravity on player
            if (myRigidBody2D.velocity.y < 0)
            {
                myRigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (Input.GetKeyUp(myPlayerControl.jumpKeyCode) || !Input.GetKey(myPlayerControl.jumpKeyCode))
            {
                myRigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
        else if (myPlayerControlOld != null)
        {
            //subtract 1 to account for normal gravity on player
            if (myRigidBody2D.velocity.y < 0)
            {
                myRigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (Input.GetKeyUp(myPlayerControlOld.jumpKeyCode) || !Input.GetKey(myPlayerControlOld.jumpKeyCode))
            {
                myRigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }
}
