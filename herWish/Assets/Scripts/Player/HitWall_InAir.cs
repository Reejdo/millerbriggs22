using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitWall_InAir : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerControl myPlayerControl;
    private Animator myPlayerAnim;

    public float knockBackForce; 

    [Header("Lowest y velocity to hit off wall")]
    public float lowVelocityLimit;
    public string animWallTriggerName = "hitWall"; 

    void Start()
    {
        myPlayerControl = gameObject.GetComponentInParent<PlayerControl>();
        myPlayerAnim = gameObject.GetComponentInParent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
        Vector3 pos = contact.point; 

        if (!myPlayerControl.OneGroundedTrue() && !collision.gameObject.CompareTag("BounceObject"))
        {
            //if hit wall, not grounded, and above low velocity, hit player backwards
            if (myPlayerControl.myRigidBody2D.velocity.y > lowVelocityLimit)
            {
                //if collision contact point was to the right
                if (pos.x > gameObject.transform.position.x)
                {
                    Vector2 knockBack = new Vector2(-knockBackForce, 0); 
                    myPlayerControl.myRigidBody2D.AddForce(knockBack, ForceMode2D.Force);

                    Debug.Log("Knockback Left");
                }
                else
                {
                    Debug.Log("Knockback Right");

                    Vector2 knockBack = new Vector2(knockBackForce, 0);
                    myPlayerControl.myRigidBody2D.AddForce(knockBack, ForceMode2D.Force);
                }
            }
        }
    }
}
