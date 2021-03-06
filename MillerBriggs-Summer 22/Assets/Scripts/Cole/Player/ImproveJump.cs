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

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myPlayerControl = GetComponent<PlayerControl>(); 
    }

    // Update is called once per frame
    void Update()
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
        else if (myPlayerControl.isWallSliding)
        {
            if (myRigidBody2D.velocity.y < -myPlayerControl.wallSlideSpeed)
            {
                Debug.Log("Wall Slide");
                myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x, -myPlayerControl.wallSlideSpeed);
            }
        }
    }
}
